// Entidade movimentacao de estoque (e/s) e constantes de tipo.
namespace PharmaStock.Modelos;

public class Movimentacao
{
    public int IdMovimentacao { get; set; }
    public int IdFuncionario { get; set; }
    public int IdProduto { get; set; }
    public int IdLote { get; set; }
    public int? IdEntradaItem { get; set; }
    public int? IdVendaItem { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
    public DateTime DataMov { get; set; }
    public string? Pagamento { get; set; }
    public char TipoMov { get; set; }
}

public static class TipoMovimentacao
{
    public const char Entrada = 'e';
    public const char Saida = 's';
}
