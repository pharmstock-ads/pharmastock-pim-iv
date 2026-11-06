// Alertas A7: lotes a vencer e estoque baixo.
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Modelos.Dtos;
using PharmaStock.Negocio;

namespace PharmaStock.Api.Controllers;

[ApiController]
[Route("api/alertas")]
public class AlertasController : ControllerBase
{
    private readonly AlertaNegocio _alertas;

    public AlertasController(AlertaNegocio alertas) => _alertas = alertas;

    [HttpGet("lotes-a-vencer")]
    public async Task<ActionResult<IReadOnlyList<LoteDto>>> LotesAVencer([FromQuery] int? dias, CancellationToken ct)
    {
        var lista = await _alertas.LotesAVencerAsync(dias, ct);
        return Ok(lista.Select(l => new LoteDto(l.Id, l.IdProduto, l.NumeroLote, l.Validade, l.Quantidade)).ToList());
    }

    [HttpGet("estoque-baixo")]
    public async Task<ActionResult<IReadOnlyList<LoteDto>>> EstoqueBaixo([FromQuery] decimal? limite, CancellationToken ct)
    {
        var lista = await _alertas.EstoqueBaixoAsync(limite, ct);
        return Ok(lista.Select(l => new LoteDto(l.Id, l.IdProduto, l.NumeroLote, l.Validade, l.Quantidade)).ToList());
    }
}
