using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Web.ViewModels;

public class FornecedorFormViewModel
{
    [Required(ErrorMessage = "Informe o nome da empresa.")]
    [Display(Name = "Nome da empresa")]
    [StringLength(200, ErrorMessage = "Nome da empresa deve ter no máximo 200 caracteres.")]
    public string NomeEmpresa { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o CNPJ.")]
    [Display(Name = "CNPJ")]
    [StringLength(18, ErrorMessage = "CNPJ deve ter no máximo 18 caracteres.")]
    public string Cnpj { get; set; } = string.Empty;

    [Display(Name = "Telefone")]
    [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres.")]
    public string? Telefone { get; set; }
}
