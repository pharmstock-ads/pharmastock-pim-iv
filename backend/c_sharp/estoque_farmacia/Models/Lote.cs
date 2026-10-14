namespace estoque_farmacia.Models;

public class Lote
{
    public int Id { get; set; }

    public int IdProduto { get; set; }

    public int NumeroLote { get; set; }

    public DateTime Validade { get; set; }

    public decimal Quantidade { get; set; }
}
