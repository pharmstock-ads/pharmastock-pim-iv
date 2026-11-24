// Acesso Dapper a funcionario.
using Dapper;
using PharmaStock.Modelos;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Repositorio;

public class FuncionarioRepositorio : IFuncionarioRepositorio
{
    private readonly ISqlConnectionFactory _factory;

    public FuncionarioRepositorio(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<Funcionario?> ObterPorIdAsync(int idFuncionario, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        return await conn.QuerySingleOrDefaultAsync<Funcionario>(new CommandDefinition(
            """
            SELECT id_funcionario AS IdFuncionario, nome AS Nome, cpf AS Cpf, cargo AS Cargo,
                   email AS Email, senha_hash AS SenhaHash, data_admissao AS DataAdmissao, ativo AS Ativo
            FROM dbo.funcionario
            WHERE id_funcionario = @idFuncionario
            """,
            new { idFuncionario },
            cancellationToken: ct));
    }

    public async Task<Funcionario?> ObterPorCpfOuEmailAsync(string cpfOuEmail, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        return await conn.QuerySingleOrDefaultAsync<Funcionario>(new CommandDefinition(
            """
            SELECT id_funcionario AS IdFuncionario, nome AS Nome, cpf AS Cpf, cargo AS Cargo,
                   email AS Email, senha_hash AS SenhaHash, data_admissao AS DataAdmissao, ativo AS Ativo
            FROM dbo.funcionario
            WHERE email = @cpfOuEmail OR cpf = @cpfOuEmail
            """,
            new { cpfOuEmail },
            cancellationToken: ct));
    }

    public async Task<int> InserirAsync(Funcionario funcionario, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        return await conn.ExecuteScalarAsync<int>(new CommandDefinition(
            """
            INSERT INTO dbo.Funcionario
                (nome, cpf, cargo, email, senha_hash, data_admissao, ativo)
            OUTPUT INSERTED.id_funcionario
            VALUES
                (@Nome, @Cpf, @Cargo, @Email, @SenhaHash, @DataAdmissao, @Ativo)
            """,
            new
            {
                funcionario.Nome,
                funcionario.Cpf,
                funcionario.Cargo,
                funcionario.Email,
                funcionario.SenhaHash,
                funcionario.DataAdmissao,
                Ativo = funcionario.Ativo.ToString()
            },
            cancellationToken: ct));
    }
}
