namespace estoque_farmacia.Models;

public class ItemVenda
{
    public int IdProduto { get; set; }

    public string NomeProduto { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }

    public decimal Subtotal => PrecoUnitario * Quantidade;
}

public class Venda
{
    public int Id { get; set; }

    public int IdFuncionario { get; set; }

    public DateTime DataVenda { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Desconto { get; set; }

    public decimal ValorTotal { get; set; }

    public List<ItemVenda> Itens { get; set; } = new();
}
