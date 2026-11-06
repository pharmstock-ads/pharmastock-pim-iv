// Venda via SPs em uma unica transacao (C15).
using System.Data;
using Dapper;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Repositorio;

public class VendaRepositorio : IVendaRepositorio
{
    private readonly ISqlConnectionFactory _factory;

    public VendaRepositorio(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<int> RegistrarVendaCompletaAsync(
        int idFuncionario,
        IReadOnlyList<VendaItemRegistro> itens,
        CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        await using var tx = (Microsoft.Data.SqlClient.SqlTransaction)await conn.BeginTransactionAsync(ct);

        try
        {
            var pVenda = new DynamicParameters();
            pVenda.Add("@pIdFuncionario", idFuncionario);
            pVenda.Add("@pIdVenda", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync(new CommandDefinition(
                "prc_RegistrarVenda",
                pVenda,
                transaction: tx,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct));

            var idVenda = pVenda.Get<int>("@pIdVenda");

            foreach (var item in itens)
            {
                await conn.ExecuteAsync(new CommandDefinition(
                    "prc_RegistrarVendaItem",
                    new
                    {
                        pIdVenda = idVenda,
                        pIdProduto = item.IdProduto,
                        pIdLote = item.IdLote,
                        pQuantidade = (decimal)item.Quantidade,
                        pPrecoUnitario = item.PrecoUnitario
                    },
                    transaction: tx,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: ct));
            }

            await tx.CommitAsync(ct);
            return idVenda;
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<IReadOnlyList<(int IdVenda, int IdFuncionario, DateTime DataVenda, decimal ValorTotal)>> ListarAsync(
        CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        var rows = await conn.QueryAsync(new CommandDefinition(
            """
            SELECT id_venda AS IdVenda, id_funcionario AS IdFuncionario,
                   data_venda AS DataVenda, valor_total AS ValorTotal
            FROM dbo.venda
            ORDER BY data_venda DESC
            """,
            cancellationToken: ct));

        return rows
            .Select(r => ((int)r.IdVenda, (int)r.IdFuncionario, (DateTime)r.DataVenda, (decimal)r.ValorTotal))
            .ToList();
    }
}
