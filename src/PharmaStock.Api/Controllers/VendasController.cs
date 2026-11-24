// Vendas com FEFO no Negocio.
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Modelos.Dtos;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Api.Controllers;

[ApiController]
[Route("api/vendas")]
public class VendasController : ControllerBase
{
    private readonly VendaNegocio _vendas;

    public VendasController(VendaNegocio vendas) => _vendas = vendas;

    [HttpPost]
    public async Task<ActionResult<VendaDetalheDto>> Registrar([FromBody] VendaRequestDto dto, CancellationToken ct)
    {
        try
        {
            if (dto.Itens is null)
                return BadRequest(new { mensagem = "Informe ao menos um item de venda." });

            var itens = dto.Itens.Select(i => new VendaItemPedido(i.ProdutoId, i.Quantidade, i.PrecoUnitario)).ToList();
            var id = await _vendas.RegistrarAsync(dto.FuncionarioId, itens, ct);
            var criada = await _vendas.ObterAsync(id, ct)
                ?? throw new DominioException("Venda criada não encontrada.");
            return Created($"/api/vendas/{id}", Map(criada));
        }
        catch (DominioException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VendaDetalheDto>>> Listar(CancellationToken ct)
    {
        var lista = await _vendas.ListarAsync(ct);
        return Ok(lista.Select(Map).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VendaDetalheDto>> Obter(int id, CancellationToken ct)
    {
        var venda = await _vendas.ObterAsync(id, ct);
        return venda is null ? NotFound() : Ok(Map(venda));
    }

    private static VendaDetalheDto Map(VendaListagem v)
        => new(v.IdVenda, v.IdFuncionario, v.DataVenda, v.ValorTotal, v.FuncionarioNome, v.Produtos, v.QuantidadeItens);
}
