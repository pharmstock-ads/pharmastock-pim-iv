using Microsoft.AspNetCore.Mvc;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Web.ViewModels;

namespace PharmaStock.Web.Controllers;

public class AccountController : Controller
{
    private readonly AuthNegocio _auth;

    public AccountController(AuthNegocio auth) => _auth = auth;

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (HttpContext.Session.GetInt32("FuncionarioId") is not null)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var funcionario = await _auth.LoginAsync(model.Email, model.Senha);
            HttpContext.Session.SetInt32("FuncionarioId", funcionario.IdFuncionario);
            HttpContext.Session.SetString("FuncionarioNome", funcionario.Nome);
            return Redirect(returnUrl ?? Url.Action("Index", "Home")!);
        }
        catch (DominioException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
