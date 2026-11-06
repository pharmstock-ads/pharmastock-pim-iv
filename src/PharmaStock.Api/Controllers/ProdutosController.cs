// Cadastro de produtos.
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Modelos;
using PharmaStock.Modelos.Dtos;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;

namespace PharmaStock.Api.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly CadastroNegocio _cadastro;

    public ProdutosController(CadastroNegocio cadastro) => _cadastro = cadastro;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProdutoDto>>> Listar(CancellationToken ct)
    {
        var lista = await _cadastro.ListarProdutosAsync(ct);
        return Ok(lista.Select(Map).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoDto>> Obter(int id, CancellationToken ct)
    {
        var p = await _cadastro.ObterProdutoAsync(id, ct);
        return p is null ? NotFound() : Ok(Map(p));
    }

    [HttpPost]
    public async Task<ActionResult<object>> Criar([FromBody] ProdutoSalvarDto dto, CancellationToken ct)
    {
        try
        {
            var id = await _cadastro.CriarProdutoAsync(new Produto
            {
                Codigo = dto.Codigo,
                CodBarra = dto.CodBarra,
                Nome = dto.Nome,
                IdCategoria = dto.IdCategoria,
                IdFornecedor = dto.IdFornecedor
            }, ct);
            return Created($"/api/produtos/{id}", new { id });
        }
        catch (DominioException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    private static ProdutoDto Map(Produto p)
        => new(p.Id, p.Codigo, p.CodBarra, p.Nome, p.IdCategoria, p.IdFornecedor);
}
