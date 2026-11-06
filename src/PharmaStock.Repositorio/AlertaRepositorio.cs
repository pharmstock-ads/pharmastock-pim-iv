// Consultas Dapper de alertas A7.
using Dapper;
using PharmaStock.Modelos;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Repositorio;

public class AlertaRepositorio : IAlertaRepositorio
{
    private readonly ISqlConnectionFactory _factory;

    public AlertaRepositorio(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<IReadOnlyList<Lote>> ListarLotesAVencerAsync(int dias, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        var rows = await conn.QueryAsync<Lote>(new CommandDefinition(
            """
            SELECT id AS Id, id_produto AS IdProduto, numero_lote AS NumeroLote,
                   validade AS Validade, quantidade AS Quantidade
            FROM dbo.lote
            WHERE quantidade > 0
              AND validade <= DATEADD(day, @dias, CAST(GETDATE() AS date))
            ORDER BY validade
            """,
            new { dias },
            cancellationToken: ct));
        return rows.ToList();
    }

    public async Task<IReadOnlyList<Lote>> ListarEstoqueBaixoAsync(decimal limite, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        var rows = await conn.QueryAsync<Lote>(new CommandDefinition(
            """
            SELECT id AS Id, id_produto AS IdProduto, numero_lote AS NumeroLote,
                   validade AS Validade, quantidade AS Quantidade
            FROM dbo.lote
            WHERE quantidade > 0 AND quantidade <= @limite
            ORDER BY quantidade, validade
            """,
            new { limite },
            cancellationToken: ct));
        return rows.ToList();
    }
}
