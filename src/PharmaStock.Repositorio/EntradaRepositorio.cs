// Entrada via SPs em uma unica transacao (C15).
using System.Data;
using Dapper;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Repositorio;

public class EntradaRepositorio : IEntradaRepositorio
{
    private readonly ISqlConnectionFactory _factory;

    public EntradaRepositorio(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<int> ObterOuCadastrarLoteAsync(
        int idProduto,
        string numeroLote,
        DateTime validade,
        CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        var p = new DynamicParameters();
        p.Add("@pIdProduto", idProduto);
        p.Add("@pNumeroLote", numeroLote);
        p.Add("@pValidade", validade.Date);
        p.Add("@pIdLote", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(new CommandDefinition(
            "prc_ObterOuCadastrarLote",
            p,
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));

        return p.Get<int>("@pIdLote");
    }

    public async Task<int> RegistrarEntradaCompletaAsync(
        int idFuncionario,
        IReadOnlyList<EntradaItemRegistro> itens,
        CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        await using var tx = (Microsoft.Data.SqlClient.SqlTransaction)await conn.BeginTransactionAsync(ct);

        try
        {
            var pEntrada = new DynamicParameters();
            pEntrada.Add("@pIdFuncionario", idFuncionario);
            pEntrada.Add("@pIdEntrada", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync(new CommandDefinition(
                "prc_RegistrarEntrada",
                pEntrada,
                transaction: tx,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct));

            var idEntrada = pEntrada.Get<int>("@pIdEntrada");

            foreach (var item in itens)
            {
                var pLote = new DynamicParameters();
                pLote.Add("@pIdProduto", item.IdProduto);
                pLote.Add("@pNumeroLote", item.NumeroLote);
                pLote.Add("@pValidade", item.Validade.Date);
                pLote.Add("@pIdLote", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(new CommandDefinition(
                    "prc_ObterOuCadastrarLote",
                    pLote,
                    transaction: tx,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: ct));

                var idLote = pLote.Get<int>("@pIdLote");

                await conn.ExecuteAsync(new CommandDefinition(
                    "prc_RegistrarEntradaItem",
                    new
                    {
                        pIdEntrada = idEntrada,
                        pIdProduto = item.IdProduto,
                        pIdLote = idLote,
                        pQuantidade = item.Quantidade,
                        pPrecoUnitario = item.PrecoUnitario
                    },
                    transaction: tx,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: ct));
            }

            await tx.CommitAsync(ct);
            return idEntrada;
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}
