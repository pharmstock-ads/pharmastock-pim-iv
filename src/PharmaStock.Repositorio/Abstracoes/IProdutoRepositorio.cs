// Contrato de acesso a produtos.
using PharmaStock.Modelos;

namespace PharmaStock.Repositorio.Abstracoes;

public interface IProdutoRepositorio
{
    Task<IReadOnlyList<Produto>> ListarAsync(CancellationToken ct = default);
    Task<Produto?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<int> InserirAsync(Produto produto, CancellationToken ct = default);
}
