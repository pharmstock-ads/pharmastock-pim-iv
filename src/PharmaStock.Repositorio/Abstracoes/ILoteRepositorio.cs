// Contrato de consulta de lotes.
using PharmaStock.Modelos;

namespace PharmaStock.Repositorio.Abstracoes;

public interface ILoteRepositorio
{
    Task<IReadOnlyList<Lote>> ListarPorProdutoAsync(int idProduto, CancellationToken ct = default);
    Task<IReadOnlyList<Lote>> ListarComEstoquePorProdutoAsync(int idProduto, CancellationToken ct = default);
}
