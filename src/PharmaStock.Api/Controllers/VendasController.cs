// Vendas com FEFO no Negocio.
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Modelos.Dtos;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;

namespace PharmaStock.Api.Controllers;

[ApiController]
[Route("api/vendas")]
public class VendasController : ControllerBase
{
    private readonly VendaNegocio _vendas;

    public VendasController(VendaNegocio vendas) => _vendas = vendas;

    [HttpPost]
    public async Task<ActionResult<VendaResponseDto>> Registrar([FromBody] VendaRequestDto dto, CancellationToken ct)
    {
        try
        {
            var itens = dto.Itens.Select(i => new VendaItemPedido(i.ProdutoId, i.Quantidade, i.PrecoUnitario)).ToList();
            var id = await _vendas.RegistrarAsync(dto.FuncionarioId, itens, ct);
            return Created($"/api/vendas/{id}", new VendaResponseDto(id));
        }
        catch (DominioException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VendaResumoDto>>> Listar(CancellationToken ct)
    {
        var lista = await _vendas.ListarAsync(ct);
        return Ok(lista.Select(v => new VendaResumoDto(v.IdVenda, v.IdFuncionario, v.DataVenda, v.ValorTotal)).ToList());
    }
}
