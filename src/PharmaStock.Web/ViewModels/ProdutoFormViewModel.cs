using System.ComponentModel.DataAnnotations;
using PharmaStock.Modelos;

namespace PharmaStock.Web.ViewModels;

public class ProdutoFormViewModel
{
    [Required(ErrorMessage = "Informe o código do produto.")]
    [Range(1, int.MaxValue, ErrorMessage = "Código deve ser maior que zero.")]
    [Display(Name = "Código")]
    public int Codigo { get; set; }

    [Required(ErrorMessage = "Informe o nome do produto.")]
    [Display(Name = "Nome")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Código de barras")]
    [StringLength(25, ErrorMessage = "Código de barras deve ter no máximo 25 caracteres.")]
    public string? CodBarra { get; set; }

    [Display(Name = "Fornecedor")]
    public int? IdFornecedor { get; set; }

    public IReadOnlyList<Fornecedor> FornecedoresDisponiveis { get; set; } = Array.Empty<Fornecedor>();
}
