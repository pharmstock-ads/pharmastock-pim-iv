// Entidades venda e item de venda.
namespace PharmaStock.Modelos;

public class Venda
{
    public int IdVenda { get; set; }
    public int IdFuncionario { get; set; }
    public DateTime DataVenda { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemVenda> Itens { get; set; } = [];
}

public class ItemVenda
{
    public int IdVendaItem { get; set; }
    public int IdVenda { get; set; }
    public int IdProduto { get; set; }
    public int IdLote { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }

    public decimal Subtotal => PrecoUnitario * Quantidade;
}
