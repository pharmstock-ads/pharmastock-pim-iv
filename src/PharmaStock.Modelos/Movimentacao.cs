namespace PharmaStock.Modelos;

/// <summary>Registro de entrada ou saída de estoque.</summary>
public class Movimentacao
{
    public int Id { get; set; }

    public int IdFuncionario { get; set; }

    public int IdProduto { get; set; }

    public int IdLote { get; set; }

    public decimal ValorTotal { get; set; }

    public decimal PrecoUnitario { get; set; }

    public int Quantidade { get; set; }

    public DateTime DataMovimentacao { get; set; }

    public string? Pagamento { get; set; }

    public TipoMovimentacao Tipo { get; set; }
}
