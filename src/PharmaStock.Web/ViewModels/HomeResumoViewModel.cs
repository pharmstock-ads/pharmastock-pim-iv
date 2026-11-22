namespace PharmaStock.Web.ViewModels;

public class HomeResumoViewModel
{
    public string NomeFuncionario { get; set; } = "funcionário";

    public int TotalProdutos { get; set; }
    public int TotalFornecedores { get; set; }
    public int VendasHoje { get; set; }

    public int LotesAVencerCount { get; set; }
    public int EstoqueBaixoCount { get; set; }

    public IReadOnlyList<HomeAlertaItemViewModel> Alertas { get; set; } = Array.Empty<HomeAlertaItemViewModel>();
    public IReadOnlyList<HomeVendaItemViewModel> VendasRecentes { get; set; } = Array.Empty<HomeVendaItemViewModel>();
}

public class HomeAlertaItemViewModel
{
    public string Tipo { get; set; } = string.Empty;
    public string ProdutoNome { get; set; } = string.Empty;
    public string NumeroLote { get; set; } = string.Empty;
    public DateTime Validade { get; set; }
    public decimal Quantidade { get; set; }
    public int IdProduto { get; set; }
}

public class HomeVendaItemViewModel
{
    public int IdVenda { get; set; }
    public DateTime DataVenda { get; set; }
    public decimal ValorTotal { get; set; }
}
