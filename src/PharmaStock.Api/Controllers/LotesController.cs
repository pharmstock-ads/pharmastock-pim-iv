// Listagem de lotes (sem POST — C4).
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Modelos.Dtos;
using PharmaStock.Negocio;

namespace PharmaStock.Api.Controllers;

[ApiController]
[Route("api/lotes")]
public class LotesController : ControllerBase
{
    private readonly CadastroNegocio _cadastro;

    public LotesController(CadastroNegocio cadastro) => _cadastro = cadastro;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LoteDto>>> Listar([FromQuery] int produtoId, CancellationToken ct)
    {
        if (produtoId <= 0)
            return BadRequest(new { mensagem = "Informe produtoId." });

        var lista = await _cadastro.ListarLotesPorProdutoAsync(produtoId, ct);
        return Ok(lista.Select(l => new LoteDto(l.Id, l.IdProduto, l.NumeroLote, l.Validade, l.Quantidade)).ToList());
    }
}
