using Microsoft.AspNetCore.Mvc;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Web.ViewModels;

namespace PharmaStock.Web.Controllers;

public class EntradasController : Controller
{
    private readonly EntradaNegocio _entrada;
    private readonly CadastroNegocio _cadastro;

    public EntradasController(EntradaNegocio entrada, CadastroNegocio cadastro)
    {
        _entrada = entrada;
        _cadastro = cadastro;
    }

    [HttpGet]
    public async Task<IActionResult> Registrar()
    {
        var funcionarioId = HttpContext.Session.GetInt32("FuncionarioId");
        if (funcionarioId is null)
            return RedirectToAction("Login", "Account", new { returnUrl = "/Entradas/Registrar" });

        var model = new EntradaFormViewModel
        {
            ProdutosDisponiveis = await _cadastro.ListarProdutosAsync()
        };

        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(EntradaFormViewModel model)
    {
        var funcionarioId = HttpContext.Session.GetInt32("FuncionarioId");
        if (funcionarioId is null)
            return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
        {
            model.ProdutosDisponiveis = await _cadastro.ListarProdutosAsync();
            return View(model);
        }

        try
        {
            var itens = new List<EntradaItemPedido>
            {
                new(model.ProdutoId, model.NumeroLote, model.Validade, model.Quantidade, model.PrecoUnitario)
            };

            await _entrada.RegistrarAsync(funcionarioId.Value, itens);
            TempData["Sucesso"] = "Entrada registrada com sucesso.";
            return RedirectToAction("Index", "Home");
        }
        catch (DominioException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            model.ProdutosDisponiveis = await _cadastro.ListarProdutosAsync();
            return View(model);
        }
    }
}
