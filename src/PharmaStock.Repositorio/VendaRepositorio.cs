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

    public async Task<IReadOnlyList<VendaListagem>> ListarAsync(CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        var rows = await conn.QueryAsync<(
            int IdVenda,
            int IdFuncionario,
            DateTime DataVenda,
            decimal ValorTotal,
            string? FuncionarioNome,
            string? ProdutoNome,
            decimal? Quantidade)>(new CommandDefinition(
            """
            SELECT
                v.id_venda AS IdVenda,
                v.id_funcionario AS IdFuncionario,
                v.data_venda AS DataVenda,
                v.valor_total AS ValorTotal,
                f.nome AS FuncionarioNome,
                p.nome AS ProdutoNome,
                vi.quantidade AS Quantidade
            FROM dbo.Venda v
            LEFT JOIN dbo.Funcionario f ON f.id_funcionario = v.id_funcionario
            LEFT JOIN dbo.VendaItem vi ON vi.id_venda = v.id_venda
            LEFT JOIN dbo.Produto p ON p.id = vi.id_produto
            ORDER BY v.data_venda DESC, p.nome
            """,
            cancellationToken: ct));

        return rows
            .GroupBy(r => new { r.IdVenda, r.IdFuncionario, r.DataVenda, r.ValorTotal, r.FuncionarioNome })
            .Select(g =>
            {
                var nomes = string.Join(
                    ", ",
                    g.Select(x => x.ProdutoNome).Where(n => !string.IsNullOrWhiteSpace(n)).Distinct());
                return new VendaListagem(
                    g.Key.IdVenda,
                    g.Key.IdFuncionario,
                    g.Key.DataVenda,
                    g.Key.ValorTotal,
                    string.IsNullOrWhiteSpace(g.Key.FuncionarioNome)
                        ? $"#{g.Key.IdFuncionario}"
                        : g.Key.FuncionarioNome!,
                    string.IsNullOrEmpty(nomes) ? "—" : nomes,
                    (int)Math.Floor(g.Sum(x => x.Quantidade ?? 0m)));
            })
            .ToList();
    }
}
