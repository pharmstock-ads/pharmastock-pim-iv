using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Web.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Informe o e-mail ou CPF.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a senha.")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Senha { get; set; } = string.Empty;
}
