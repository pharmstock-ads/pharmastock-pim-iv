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
    [StringLength(14, MinimumLength = 14, ErrorMessage = "CNPJ deve ter exatamente 14 dígitos.")]
    [RegularExpression(@"^\d{14}$", ErrorMessage = "CNPJ deve conter apenas 14 números, sem pontuação.")]
    public string Cnpj { get; set; } = string.Empty;

    [Display(Name = "Telefone")]
    [StringLength(30, ErrorMessage = "Telefone deve ter no máximo 30 caracteres.")]
    public string? Telefone { get; set; }
}
