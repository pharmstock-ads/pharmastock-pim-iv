// Contrato de acesso a fornecedores.
using PharmaStock.Modelos;

namespace PharmaStock.Repositorio.Abstracoes;

public interface IFornecedorRepositorio
{
    Task<IReadOnlyList<Fornecedor>> ListarAsync(CancellationToken ct = default);
    Task<Fornecedor?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<int> InserirAsync(Fornecedor fornecedor, CancellationToken ct = default);
}
