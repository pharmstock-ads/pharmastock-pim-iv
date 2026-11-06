// DTOs de entrada, venda e alertas (compartilhados Web/Api).
namespace PharmaStock.Modelos.Dtos;

public record EntradaItemRequestDto(
    int ProdutoId,
    string NumeroLote,
    DateTime Validade,
    decimal Quantidade,
    decimal PrecoUnitario);

public record EntradaRequestDto(int FuncionarioId, IReadOnlyList<EntradaItemRequestDto> Itens);

public record EntradaResponseDto(int EntradaId);

public record VendaItemRequestDto(int ProdutoId, int Quantidade, decimal PrecoUnitario);

public record VendaRequestDto(int FuncionarioId, IReadOnlyList<VendaItemRequestDto> Itens);

public record VendaResponseDto(int VendaId);

public record VendaResumoDto(int IdVenda, int IdFuncionario, DateTime DataVenda, decimal ValorTotal);
