// DTOs de autenticacao (compartilhados Web/Api).
namespace PharmaStock.Modelos.Dtos;

public record LoginRequestDto(string CpfOuEmail, string Senha);

public record LoginResponseDto(int FuncionarioId, string Nome, string? Cargo);
