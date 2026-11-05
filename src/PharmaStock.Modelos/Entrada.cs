// Entidades entrada e item de entrada de estoque.
namespace PharmaStock.Modelos;

public class Entrada
{
    public int IdEntrada { get; set; }
    public int IdFuncionario { get; set; }
    public DateTime DataEntrada { get; set; }
    public decimal ValorTotal { get; set; }
    public List<EntradaItem> Itens { get; set; } = [];
}

public class EntradaItem
{
    public int IdEntradaItem { get; set; }
    public int IdEntrada { get; set; }
    public int IdProduto { get; set; }
    public int IdLote { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }

    public decimal Subtotal => PrecoUnitario * Quantidade;
}
