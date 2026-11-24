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
    public async Task<ActionResult<VendaResumoDto>> Registrar([FromBody] VendaRequestDto dto, CancellationToken ct)
    {
        try
        {
            if (dto.Itens is null)
                return BadRequest(new { mensagem = "Informe ao menos um item de venda." });

            var itens = dto.Itens.Select(i => new VendaItemPedido(i.ProdutoId, i.Quantidade, i.PrecoUnitario)).ToList();
            var id = await _vendas.RegistrarAsync(dto.FuncionarioId, itens, ct);
            var lista = await _vendas.ListarAsync(ct);
            var criada = lista.FirstOrDefault(v => v.IdVenda == id)
                ?? throw new DominioException("Venda criada não encontrada.");
            return Created(
                $"/api/vendas/{id}",
                new VendaResumoDto(criada.IdVenda, criada.IdFuncionario, criada.DataVenda, criada.ValorTotal));
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
        return Ok(lista
            .Select(v => new VendaResumoDto(v.IdVenda, v.IdFuncionario, v.DataVenda, v.ValorTotal))
            .ToList());
    }
}
