// Entidade produto do catalogo.
namespace PharmaStock.Modelos;

public class Produto
{
    public int Id { get; set; }
    public int Codigo { get; set; }
    public string? CodBarra { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int? IdCategoria { get; set; }
    public int? IdFornecedor { get; set; }
}
