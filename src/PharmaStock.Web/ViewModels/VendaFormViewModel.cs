using System.ComponentModel.DataAnnotations;
using PharmaStock.Modelos;

namespace PharmaStock.Web.ViewModels;

public class VendaFormViewModel
{
    [Required(ErrorMessage = "Selecione o produto.")]
    [Display(Name = "Produto")]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "Informe a quantidade.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero.")]
    [Display(Name = "Quantidade")]
    public int Quantidade { get; set; } = 1;

    [Required(ErrorMessage = "Informe o preço unitário.")]
    [Range(0, double.MaxValue, ErrorMessage = "Preço unitário não pode ser negativo.")]
    [Display(Name = "Preço unitário")]
    public decimal PrecoUnitario { get; set; }

    public IReadOnlyList<Produto> ProdutosDisponiveis { get; set; } = Array.Empty<Produto>();
}
