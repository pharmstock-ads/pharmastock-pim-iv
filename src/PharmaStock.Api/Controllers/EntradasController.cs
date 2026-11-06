// Registro de entrada de estoque.
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Modelos.Dtos;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;

namespace PharmaStock.Api.Controllers;

[ApiController]
[Route("api/entradas")]
public class EntradasController : ControllerBase
{
    private readonly EntradaNegocio _entradas;

    public EntradasController(EntradaNegocio entradas) => _entradas = entradas;

    [HttpPost]
    public async Task<ActionResult<EntradaResponseDto>> Registrar([FromBody] EntradaRequestDto dto, CancellationToken ct)
    {
        try
        {
            var itens = dto.Itens.Select(i => new EntradaItemPedido(
                i.ProdutoId, i.NumeroLote, i.Validade, i.Quantidade, i.PrecoUnitario)).ToList();
            var id = await _entradas.RegistrarAsync(dto.FuncionarioId, itens, ct);
            return Created($"/api/entradas/{id}", new EntradaResponseDto(id));
        }
        catch (DominioException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
