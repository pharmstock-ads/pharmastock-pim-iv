using Microsoft.AspNetCore.Mvc;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Web.ViewModels;

namespace PharmaStock.Web.Controllers;

public class FornecedoresController : Controller
{
    private readonly CadastroNegocio _cadastro;

    public FornecedoresController(CadastroNegocio cadastro) => _cadastro = cadastro;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetInt32("FuncionarioId") is null)
            return RedirectToAction("Login", "Account", new { returnUrl = "/Fornecedores" });

        var lista = await _cadastro.ListarFornecedoresAsync();
        return View(lista);
    }

    [HttpGet]
    public IActionResult Criar()
    {
        if (HttpContext.Session.GetInt32("FuncionarioId") is null)
            return RedirectToAction("Login", "Account", new { returnUrl = "/Fornecedores/Criar" });

        return View(new FornecedorFormViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(FornecedorFormViewModel model)
    {
        if (HttpContext.Session.GetInt32("FuncionarioId") is null)
            return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _cadastro.CriarFornecedorAsync(model.NomeEmpresa, model.Cnpj, model.Telefone);
            TempData["Sucesso"] = "Fornecedor cadastrado com sucesso.";
            return RedirectToAction(nameof(Index));
        }
        catch (DominioException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }
}
