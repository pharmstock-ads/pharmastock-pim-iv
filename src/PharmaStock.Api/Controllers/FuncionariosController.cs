// Cadastro mínimo de funcionário (senha hasheada no Negócio).
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Modelos.Dtos;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;

namespace PharmaStock.Api.Controllers;

[ApiController]
[Route("api/funcionarios")]
[Tags("Funcionarios")]
public class FuncionariosController : ControllerBase
{
    private readonly AuthNegocio _auth;

    public FuncionariosController(AuthNegocio auth) => _auth = auth;

    [HttpPost]
    public async Task<ActionResult<FuncionarioCriadoDto>> Criar(
        [FromBody] FuncionarioSalvarDto dto,
        CancellationToken ct)
    {
        try
        {
            var id = await _auth.CriarAsync(dto.Nome, dto.Cpf, dto.Email, dto.Senha, dto.Cargo, ct);
            return Created($"/api/funcionarios/{id}", new FuncionarioCriadoDto(id));
        }
        catch (DominioException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
