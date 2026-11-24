namespace PharmaStock.Web.ViewModels;

public class VendaListaItemViewModel
{
    public int IdVenda { get; set; }
    public DateTime DataVenda { get; set; }
    public string FuncionarioNome { get; set; } = string.Empty;
    public string Produtos { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
}
