// Testes de login e criação de funcionário com repositorio fake.
using PharmaStock.Modelos;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Negocio.Seguranca;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Tests;

file class FuncionarioRepoFake : IFuncionarioRepositorio
{
    private readonly List<Funcionario> _itens = [];
    private int _seq = 1;

    public FuncionarioRepoFake(Funcionario? funcionario = null)
    {
        if (funcionario is not null)
        {
            _itens.Add(funcionario);
            _seq = Math.Max(_seq, funcionario.IdFuncionario + 1);
        }
    }

    public Task<Funcionario?> ObterPorCpfOuEmailAsync(string cpfOuEmail, CancellationToken ct = default)
        => Task.FromResult(_itens.FirstOrDefault(f =>
            string.Equals(f.Email, cpfOuEmail, StringComparison.OrdinalIgnoreCase)
            || string.Equals(f.Cpf, cpfOuEmail, StringComparison.OrdinalIgnoreCase)));

    public Task<Funcionario?> ObterPorIdAsync(int idFuncionario, CancellationToken ct = default)
        => Task.FromResult(_itens.FirstOrDefault(f => f.IdFuncionario == idFuncionario));

    public Task<int> InserirAsync(Funcionario funcionario, CancellationToken ct = default)
    {
        funcionario.IdFuncionario = _seq++;
        _itens.Add(funcionario);
        return Task.FromResult(funcionario.IdFuncionario);
    }
}

public class AuthNegocioTests
{
    private const string SenhaPlana = "SenhaTeste@1";

    private static Funcionario AnaAtiva() => new()
    {
        IdFuncionario = 1,
        Nome = "Ana",
        Email = "ana.souza@pharmastock.local",
        Cpf = "12345678901",
        SenhaHash = SenhaHasher.Hash(SenhaPlana),
        Ativo = 's'
    };

    [Fact]
    public async Task Login_Ok()
    {
        var negocio = new AuthNegocio(new FuncionarioRepoFake(AnaAtiva()));
        var user = await negocio.LoginAsync("ana.souza@pharmastock.local", SenhaPlana);
        Assert.Equal(1, user.IdFuncionario);
    }

    [Fact]
    public async Task Login_SenhaErrada_Lanca()
    {
        var negocio = new AuthNegocio(new FuncionarioRepoFake(AnaAtiva()));
        await Assert.ThrowsAsync<DominioException>(() =>
            negocio.LoginAsync("ana.souza@pharmastock.local", "errada"));
    }

    [Fact]
    public async Task Login_Inativo_Lanca()
    {
        var ana = AnaAtiva();
        ana.Ativo = 'n';
        var negocio = new AuthNegocio(new FuncionarioRepoFake(ana));
        await Assert.ThrowsAsync<DominioException>(() =>
            negocio.LoginAsync("ana.souza@pharmastock.local", SenhaPlana));
    }

    [Fact]
    public async Task Criar_GravaHashEPermiteLogin()
    {
        var repo = new FuncionarioRepoFake();
        var negocio = new AuthNegocio(repo);

        var id = await negocio.CriarAsync(
            "Novo Func",
            "99988877766",
            "novo@pharmastock.local",
            "@Ana123",
            "Atendente");

        Assert.True(id > 0);
        var salvo = await repo.ObterPorIdAsync(id);
        Assert.NotNull(salvo);
        Assert.NotEqual("@Ana123", salvo!.SenhaHash);
        Assert.True(SenhaHasher.Verificar("@Ana123", salvo.SenhaHash));

        var user = await negocio.LoginAsync("novo@pharmastock.local", "@Ana123");
        Assert.Equal(id, user.IdFuncionario);
    }

    [Fact]
    public async Task Criar_CpfDuplicado_Lanca()
    {
        var negocio = new AuthNegocio(new FuncionarioRepoFake(AnaAtiva()));
        await Assert.ThrowsAsync<DominioException>(() =>
            negocio.CriarAsync("Outro", "12345678901", "outro@pharmastock.local", "@Ana123"));
    }
}
