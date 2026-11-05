// Entidade fornecedor (cadastro).
namespace PharmaStock.Modelos;

public class Fornecedor
{
    public int Id { get; set; }
    public string NomeEmpresa { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string? Telefone { get; set; }
}
