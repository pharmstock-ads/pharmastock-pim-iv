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
}
