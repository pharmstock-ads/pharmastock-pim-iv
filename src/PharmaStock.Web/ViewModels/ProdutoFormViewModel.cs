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
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Código de barras")]
    [StringLength(50, ErrorMessage = "Código de barras deve ter no máximo 50 caracteres.")]
    public string? CodBarra { get; set; }

    [Display(Name = "Fornecedor")]
    public int? IdFornecedor { get; set; }

    public IReadOnlyList<Fornecedor> FornecedoresDisponiveis { get; set; } = Array.Empty<Fornecedor>();
}
