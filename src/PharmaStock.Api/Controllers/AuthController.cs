// Login MVP (sem JWT).
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Modelos.Dtos;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;

namespace PharmaStock.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthNegocio _auth;

    public AuthController(AuthNegocio auth) => _auth = auth;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request, CancellationToken ct)
    {
        try
        {
            var user = await _auth.LoginAsync(request.CpfOuEmail, request.Senha, ct);
            return Ok(new LoginResponseDto(user.IdFuncionario, user.Nome, user.Cargo));
        }
        catch (DominioException ex)
        {
            return Unauthorized(new { mensagem = ex.Message });
        }
    }
}
