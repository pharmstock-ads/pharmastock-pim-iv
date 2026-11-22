using Microsoft.AspNetCore.Mvc;
using PharmaStock.Modelos;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Web.ViewModels;

namespace PharmaStock.Web.Controllers;

public class ProdutosController : Controller
{
    private readonly CadastroNegocio _cadastro;

    public ProdutosController(CadastroNegocio cadastro) => _cadastro = cadastro;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetInt32("FuncionarioId") is null)
            return RedirectToAction("Login", "Account", new { returnUrl = "/Produtos" });

        var lista = await _cadastro.ListarProdutosAsync();
        return View(lista);
    }

    [HttpGet]
    public async Task<IActionResult> Detalhe(int id)
    {
        if (HttpContext.Session.GetInt32("FuncionarioId") is null)
            return RedirectToAction("Login", "Account", new { returnUrl = $"/Produtos/Detalhe/{id}" });

        var produto = await _cadastro.ObterProdutoAsync(id);
        if (produto is null)
            return NotFound();

        if (produto.IdFornecedor is int idFornecedor)
        {
            var fornecedores = await _cadastro.ListarFornecedoresAsync();
            ViewBag.FornecedorNome = fornecedores
                .FirstOrDefault(f => f.Id == idFornecedor)?.NomeEmpresa;
        }

        return View(produto);
    }

    [HttpGet]
    public async Task<IActionResult> Criar()
    {
        if (HttpContext.Session.GetInt32("FuncionarioId") is null)
            return RedirectToAction("Login", "Account", new { returnUrl = "/Produtos/Criar" });

        return View(await NovoFormAsync());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(ProdutoFormViewModel model)
    {
        if (HttpContext.Session.GetInt32("FuncionarioId") is null)
            return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
        {
            model.FornecedoresDisponiveis = await _cadastro.ListarFornecedoresAsync();
            return View(model);
        }

        try
        {
            var id = await _cadastro.CriarProdutoAsync(new Produto
            {
                Codigo = model.Codigo,
                Nome = model.Nome.Trim(),
                CodBarra = string.IsNullOrWhiteSpace(model.CodBarra) ? null : model.CodBarra.Trim(),
                IdFornecedor = model.IdFornecedor,
                IdCategoria = null
            });

            TempData["Sucesso"] = "Produto cadastrado com sucesso.";
            return RedirectToAction(nameof(Detalhe), new { id });
        }
        catch (DominioException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            model.FornecedoresDisponiveis = await _cadastro.ListarFornecedoresAsync();
            return View(model);
        }
    }

    private async Task<ProdutoFormViewModel> NovoFormAsync()
        => new()
        {
            FornecedoresDisponiveis = await _cadastro.ListarFornecedoresAsync()
        };
}
