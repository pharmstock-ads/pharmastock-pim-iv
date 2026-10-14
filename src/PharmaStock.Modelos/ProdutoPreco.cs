namespace PharmaStock.Modelos;

/// <summary>Preços e margem associados a um produto.</summary>
public class ProdutoPreco
{
    public int Id { get; set; }

    public int IdProduto { get; set; }

    public decimal Margem { get; set; }

    public decimal CustoBruto { get; set; }

    public decimal Acrescimo { get; set; }

    public decimal? CustoLiquido { get; set; }

    public decimal? PrecoVenda { get; set; }
}
