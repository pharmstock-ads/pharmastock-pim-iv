// Contrato de acesso a funcionarios.
using PharmaStock.Modelos;

namespace PharmaStock.Repositorio.Abstracoes;

public interface IFuncionarioRepositorio
{
    Task<Funcionario?> ObterPorCpfOuEmailAsync(string cpfOuEmail, CancellationToken ct = default);
    Task<Funcionario?> ObterPorIdAsync(int idFuncionario, CancellationToken ct = default);
    Task<int> InserirAsync(Funcionario funcionario, CancellationToken ct = default);
}
