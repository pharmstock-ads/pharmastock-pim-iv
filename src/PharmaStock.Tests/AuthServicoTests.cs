// Testes de login com repositorio fake.
using PharmaStock.Modelos;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Negocio.Servicos;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Tests;

file class FuncionarioRepoFake : IFuncionarioRepositorio
{
    private readonly Funcionario? _funcionario;

    public FuncionarioRepoFake(Funcionario? funcionario) => _funcionario = funcionario;

    public Task<Funcionario?> ObterPorCpfOuEmailAsync(string cpfOuEmail, CancellationToken ct = default)
        => Task.FromResult(
            _funcionario is not null
            && (string.Equals(_funcionario.Email, cpfOuEmail, StringComparison.OrdinalIgnoreCase)
                || string.Equals(_funcionario.Cpf, cpfOuEmail, StringComparison.OrdinalIgnoreCase))
                ? _funcionario
                : null);

    public Task<Funcionario?> ObterPorIdAsync(int idFuncionario, CancellationToken ct = default)
        => Task.FromResult(_funcionario is not null && _funcionario.IdFuncionario == idFuncionario
            ? _funcionario
            : null);
}

public class AuthServicoTests
{
    private static Funcionario AnaAtiva() => new()
    {
        IdFuncionario = 1,
        Nome = "Ana",
        Email = "ana.souza@pharmastock.local",
        Cpf = "12345678901",
        SenhaHash = "hash_demo_ana",
        Ativo = 's'
    };

    [Fact]
    public async Task Login_Ok()
    {
        var servico = new AuthServico(new FuncionarioRepoFake(AnaAtiva()));
        var user = await servico.LoginAsync("ana.souza@pharmastock.local", "hash_demo_ana");
        Assert.Equal(1, user.IdFuncionario);
    }

    [Fact]
    public async Task Login_SenhaErrada_Lanca()
    {
        var servico = new AuthServico(new FuncionarioRepoFake(AnaAtiva()));
        await Assert.ThrowsAsync<DominioException>(() =>
            servico.LoginAsync("ana.souza@pharmastock.local", "errada"));
    }

    [Fact]
    public async Task Login_Inativo_Lanca()
    {
        var ana = AnaAtiva();
        ana.Ativo = 'n';
        var servico = new AuthServico(new FuncionarioRepoFake(ana));
        await Assert.ThrowsAsync<DominioException>(() =>
            servico.LoginAsync("ana.souza@pharmastock.local", "hash_demo_ana"));
    }
}
