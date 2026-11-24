// Login de funcionario (MVP sem JWT).
using PharmaStock.Modelos;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Negocio.Seguranca;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Negocio;

public class AuthNegocio
{
    private readonly IFuncionarioRepositorio _funcionarios;

    public AuthNegocio(IFuncionarioRepositorio funcionarios)
    {
        _funcionarios = funcionarios;
    }

    public async Task<Funcionario> LoginAsync(string cpfOuEmail, string senha, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(cpfOuEmail) || string.IsNullOrWhiteSpace(senha))
            throw new DominioException("Informe CPF/e-mail e senha.");

        var funcionario = await _funcionarios.ObterPorCpfOuEmailAsync(cpfOuEmail.Trim(), ct);
        if (funcionario is null || !funcionario.EstaAtivo || !SenhaHasher.Verificar(senha, funcionario.SenhaHash))
            throw new DominioException("Credenciais inválidas.");

        return funcionario;
    }

    public async Task<int> CriarAsync(
        string nome,
        string cpf,
        string email,
        string senha,
        string? cargo = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DominioException("Nome é obrigatório.");

        var nomeNormalizado = nome.Trim();
        if (nomeNormalizado.Length > 100)
            throw new DominioException("Nome deve ter no máximo 100 caracteres.");

        var cpfDigitos = SomenteDigitos(cpf);
        if (cpfDigitos.Length != 11)
            throw new DominioException("CPF deve conter 11 dígitos.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DominioException("E-mail é obrigatório.");

        var emailNormalizado = email.Trim();
        if (emailNormalizado.Length > 50)
            throw new DominioException("E-mail deve ter no máximo 50 caracteres.");

        if (string.IsNullOrWhiteSpace(senha))
            throw new DominioException("Senha é obrigatória.");

        string? cargoNormalizado = null;
        if (!string.IsNullOrWhiteSpace(cargo))
        {
            cargoNormalizado = cargo.Trim();
            if (cargoNormalizado.Length > 50)
                throw new DominioException("Cargo deve ter no máximo 50 caracteres.");
        }

        if (await _funcionarios.ObterPorCpfOuEmailAsync(cpfDigitos, ct) is not null)
            throw new DominioException("Já existe um funcionário com este CPF.");

        if (await _funcionarios.ObterPorCpfOuEmailAsync(emailNormalizado, ct) is not null)
            throw new DominioException("Já existe um funcionário com este e-mail.");

        return await _funcionarios.InserirAsync(new Funcionario
        {
            Nome = nomeNormalizado,
            Cpf = cpfDigitos,
            Email = emailNormalizado,
            Cargo = cargoNormalizado,
            SenhaHash = SenhaHasher.Hash(senha),
            DataAdmissao = DateTime.Today,
            Ativo = 's'
        }, ct);
    }

    private static string SomenteDigitos(string? valor)
        => string.IsNullOrWhiteSpace(valor)
            ? string.Empty
            : new string(valor.Where(char.IsDigit).ToArray());
}
