// Cadastro de fornecedores.
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Modelos.Dtos;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;

namespace PharmaStock.Api.Controllers;

[ApiController]
[Route("api/fornecedores")]
public class FornecedoresController : ControllerBase
{
    private readonly CadastroNegocio _cadastro;

    public FornecedoresController(CadastroNegocio cadastro) => _cadastro = cadastro;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FornecedorDto>>> Listar(CancellationToken ct)
    {
        var lista = await _cadastro.ListarFornecedoresAsync(ct);
        return Ok(lista.Select(f => new FornecedorDto(f.Id, f.NomeEmpresa, f.Cnpj, f.Telefone)).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<object>> Criar([FromBody] FornecedorSalvarDto dto, CancellationToken ct)
    {
        try
        {
            var id = await _cadastro.CriarFornecedorAsync(dto.NomeEmpresa, dto.Cnpj, dto.Telefone, ct);
            return Created($"/api/fornecedores/{id}", new { id });
        }
        catch (DominioException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
