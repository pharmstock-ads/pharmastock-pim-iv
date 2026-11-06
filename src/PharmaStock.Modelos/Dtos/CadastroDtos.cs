// DTOs de cadastro (compartilhados Web/Api).
namespace PharmaStock.Modelos.Dtos;

public record FornecedorDto(int Id, string NomeEmpresa, string Cnpj, string? Telefone);

public record FornecedorSalvarDto(string NomeEmpresa, string Cnpj, string? Telefone);

public record ProdutoDto(
    int Id,
    int Codigo,
    string? CodBarra,
    string Nome,
    int? IdCategoria,
    int? IdFornecedor);

public record ProdutoSalvarDto(
    int Codigo,
    string? CodBarra,
    string Nome,
    int? IdCategoria,
    int? IdFornecedor);

public record LoteDto(
    int Id,
    int IdProduto,
    string NumeroLote,
    DateTime Validade,
    decimal Quantidade);
