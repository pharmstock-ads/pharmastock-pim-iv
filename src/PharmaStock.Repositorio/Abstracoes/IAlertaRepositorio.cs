// Contrato de consultas de alerta (A7).
using PharmaStock.Modelos;

namespace PharmaStock.Repositorio.Abstracoes;

public interface IAlertaRepositorio
{
    Task<IReadOnlyList<Lote>> ListarLotesAVencerAsync(int dias, CancellationToken ct = default);
    Task<IReadOnlyList<Lote>> ListarEstoqueBaixoAsync(decimal limite, CancellationToken ct = default);
}
