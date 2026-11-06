// Login de funcionario (MVP sem JWT).
using PharmaStock.Modelos;
using PharmaStock.Negocio.Excecoes;
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
        if (funcionario is null || !funcionario.EstaAtivo || funcionario.SenhaHash != senha)
            throw new DominioException("Credenciais inválidas.");

        return funcionario;
    }
}
