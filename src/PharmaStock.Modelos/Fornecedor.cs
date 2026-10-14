namespace PharmaStock.Modelos;

/// <summary>Empresa fornecedora de produtos.</summary>
public class Fornecedor
{
    public int Id { get; set; }

    public string NomeEmpresa { get; set; } = string.Empty;

    public string Cnpj { get; set; } = string.Empty;

    public string? Telefone { get; set; }
}
