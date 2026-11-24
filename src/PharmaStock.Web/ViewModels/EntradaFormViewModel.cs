using System.ComponentModel.DataAnnotations;
using PharmaStock.Modelos;

namespace PharmaStock.Web.ViewModels;

public class EntradaFormViewModel
{
    [Required(ErrorMessage = "Selecione o produto.")]
    [Display(Name = "Produto")]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "Informe o número do lote.")]
    [Display(Name = "Número do lote")]
    [StringLength(50, ErrorMessage = "Número do lote deve ter no máximo 50 caracteres.")]
    public string NumeroLote { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a validade.")]
    [DataType(DataType.Date)]
    [Display(Name = "Validade")]
    public DateTime Validade { get; set; } = DateTime.Today.AddMonths(6);

    [Required(ErrorMessage = "Informe a quantidade.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser um número inteiro maior que zero.")]
    [Display(Name = "Quantidade")]
    public int Quantidade { get; set; } = 1;

    [Required(ErrorMessage = "Informe o preço unitário.")]
    [Range(0, double.MaxValue, ErrorMessage = "Preço unitário não pode ser negativo.")]
    [Display(Name = "Preço unitário")]
    public decimal PrecoUnitario { get; set; }

    public IReadOnlyList<Produto> ProdutosDisponiveis { get; set; } = Array.Empty<Produto>();
}
