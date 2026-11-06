// Consultas de lotes a vencer e estoque baixo.
using PharmaStock.Modelos;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Negocio;

public class AlertaNegocio
{
    public const int DiasPadrao = 30;
    public const decimal LimiteEstoquePadrao = 10;

    private readonly IAlertaRepositorio _alertas;

    public AlertaNegocio(IAlertaRepositorio alertas)
    {
        _alertas = alertas;
    }

    public Task<IReadOnlyList<Lote>> LotesAVencerAsync(int? dias = null, CancellationToken ct = default)
        => _alertas.ListarLotesAVencerAsync(dias is null or <= 0 ? DiasPadrao : dias.Value, ct);

    public Task<IReadOnlyList<Lote>> EstoqueBaixoAsync(decimal? limite = null, CancellationToken ct = default)
        => _alertas.ListarEstoqueBaixoAsync(limite is null or < 0 ? LimiteEstoquePadrao : limite.Value, ct);
}
