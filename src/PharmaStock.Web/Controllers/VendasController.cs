using Microsoft.AspNetCore.Mvc;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Web.ViewModels;

namespace PharmaStock.Web.Controllers;

public class VendasController : Controller
{
    private readonly VendaNegocio _venda;
    private readonly CadastroNegocio _cadastro;

    public VendasController(VendaNegocio venda, CadastroNegocio cadastro)
    {
        _venda = venda;
        _cadastro = cadastro;
    }

    [HttpGet]
    public async Task<IActionResult> Registrar()
    {
        var funcionarioId = HttpContext.Session.GetInt32("FuncionarioId");
        if (funcionarioId is null)
            return RedirectToAction("Login", "Account", new { returnUrl = "/Vendas/Registrar" });

        var model = new VendaFormViewModel
        {
            ProdutosDisponiveis = await _cadastro.ListarProdutosAsync()
        };

        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(VendaFormViewModel model)
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
            var itens = new List<VendaItemPedido>
            {
                new(model.ProdutoId, model.Quantidade, model.PrecoUnitario)
            };

            await _venda.RegistrarAsync(funcionarioId.Value, itens);
            TempData["Sucesso"] = "Venda registrada com sucesso.";
            return RedirectToAction(nameof(Index));
        }
        catch (DominioException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            model.ProdutosDisponiveis = await _cadastro.ListarProdutosAsync();
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetInt32("FuncionarioId") is null)
            return RedirectToAction("Login", "Account", new { returnUrl = "/Vendas" });

        var lista = await _venda.ListarAsync();
        var model = lista
            .Select(v => new VendaListaItemViewModel
            {
                IdVenda = v.IdVenda,
                DataVenda = v.DataVenda,
                FuncionarioNome = v.FuncionarioNome,
                Produtos = v.Produtos,
                QuantidadeItens = v.QuantidadeItens,
                ValorTotal = v.ValorTotal
            })
            .ToList();

        return View(model);
    }
}
