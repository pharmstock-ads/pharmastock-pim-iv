// Entidade lote (validade e quantidade em estoque).
namespace PharmaStock.Modelos;

public class Lote
{
    public int Id { get; set; }
    public int IdProduto { get; set; }
    public string NumeroLote { get; set; } = string.Empty;
    public DateTime Validade { get; set; }
    public decimal Quantidade { get; set; }
}
