namespace PharmaStock.Modelos;

/// <summary>Item de uma venda (carrinho / linha do pedido).</summary>
public class ItemVenda
{
    public int IdProduto { get; set; }

    public string NomeProduto { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }

    public decimal Subtotal => PrecoUnitario * Quantidade;
}

/// <summary>
/// Venda consolidada. No SQL do III a saída aparecia como movimentação tipo 'S';
/// no IV mantemos Venda no domínio para o fluxo de negócio/API; o mapeamento físico
/// (tabela própria vs só movimentacao) fica com o time de banco.
/// </summary>
public class Venda
{
    public int Id { get; set; }

    public int IdFuncionario { get; set; }

    public DateTime DataVenda { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Desconto { get; set; }

    public decimal ValorTotal { get; set; }

    public List<ItemVenda> Itens { get; set; } = [];
}
