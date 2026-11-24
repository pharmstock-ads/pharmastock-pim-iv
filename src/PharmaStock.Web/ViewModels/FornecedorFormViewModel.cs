using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Web.ViewModels;

public class FornecedorFormViewModel
{
    [Required(ErrorMessage = "Informe o nome da empresa.")]
    [Display(Name = "Nome da empresa")]
    [StringLength(100, ErrorMessage = "Nome da empresa deve ter no máximo 100 caracteres.")]
    public string NomeEmpresa { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o CNPJ.")]
    [Display(Name = "CNPJ")]
    [StringLength(18, MinimumLength = 14, ErrorMessage = "CNPJ deve ter 14 dígitos (com ou sem máscara).")]
    [RegularExpression(@"^[\d.\-\/]+$", ErrorMessage = "CNPJ deve conter apenas números ou máscara.")]
    public string Cnpj { get; set; } = string.Empty;

    [Display(Name = "Telefone")]
    [StringLength(30, ErrorMessage = "Telefone deve ter no máximo 30 caracteres.")]
    public string? Telefone { get; set; }
}
