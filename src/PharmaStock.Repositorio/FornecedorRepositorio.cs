// Acesso Dapper a fornecedor.
using Dapper;
using PharmaStock.Modelos;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Repositorio;

public class FornecedorRepositorio : IFornecedorRepositorio
{
    private readonly ISqlConnectionFactory _factory;

    public FornecedorRepositorio(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<IReadOnlyList<Fornecedor>> ListarAsync(CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        var rows = await conn.QueryAsync<Fornecedor>(new CommandDefinition(
            """
            SELECT id AS Id, nome_empresa AS NomeEmpresa, cnpj AS Cnpj, telefone AS Telefone
            FROM dbo.fornecedor
            ORDER BY nome_empresa
            """,
            cancellationToken: ct));
        return rows.ToList();
    }

    public async Task<Fornecedor?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        return await conn.QuerySingleOrDefaultAsync<Fornecedor>(new CommandDefinition(
            """
            SELECT id AS Id, nome_empresa AS NomeEmpresa, cnpj AS Cnpj, telefone AS Telefone
            FROM dbo.fornecedor
            WHERE id = @id
            """,
            new { id },
            cancellationToken: ct));
    }

    public async Task<int> InserirAsync(Fornecedor fornecedor, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        return await conn.ExecuteScalarAsync<int>(new CommandDefinition(
            """
            INSERT INTO dbo.fornecedor (nome_empresa, cnpj, telefone)
            OUTPUT INSERTED.id
            VALUES (@NomeEmpresa, @Cnpj, @Telefone)
            """,
            fornecedor,
            cancellationToken: ct));
    }
}
