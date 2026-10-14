namespace PharmaStock.Modelos;

/// <summary>Lote de estoque de um produto, com validade (base do FIFO).</summary>
public class Lote
{
    public int Id { get; set; }

    public int IdProduto { get; set; }

    public string NumeroLote { get; set; } = string.Empty;

    public DateTime Validade { get; set; }

    public decimal Quantidade { get; set; }
}
