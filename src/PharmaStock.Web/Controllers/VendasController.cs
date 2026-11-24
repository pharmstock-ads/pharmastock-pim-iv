using Dapper;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Repositorio;
using PharmaStock.Repositorio.Abstracoes;
using PharmaStock.Web.ViewModels;

namespace PharmaStock.Web.Controllers;

public class VendasController : Controller
{
    private readonly VendaNegocio _venda;
    private readonly CadastroNegocio _cadastro;
    private readonly IFuncionarioRepositorio _funcionarios;
    private readonly ISqlConnectionFactory _db;

    public VendasController(
        VendaNegocio venda,
        CadastroNegocio cadastro,
        IFuncionarioRepositorio funcionarios,
        ISqlConnectionFactory db)
    {
        _venda = venda;
        _cadastro = cadastro;
        _funcionarios = funcionarios;
        _db = db;
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
        var nomesFunc = new Dictionary<int, string>();
        foreach (var id in lista.Select(v => v.IdFuncionario).Distinct())
        {
            var f = await _funcionarios.ObterPorIdAsync(id);
            nomesFunc[id] = f?.Nome ?? $"#{id}";
        }

        var produtosPorVenda = new Dictionary<int, string>();
        if (lista.Count > 0)
        {
            await using var conn = _db.Create();
            var ids = lista.Select(v => v.IdVenda).ToArray();
            var rows = await conn.QueryAsync<(int IdVenda, string Nome)>(
                """
                SELECT vi.id_venda AS IdVenda, p.nome AS Nome
                FROM dbo.VendaItem vi
                INNER JOIN dbo.Produto p ON p.id = vi.id_produto
                WHERE vi.id_venda IN @ids
                ORDER BY p.nome
                """,
                new { ids });

            produtosPorVenda = rows
                .GroupBy(r => r.IdVenda)
                .ToDictionary(
                    g => g.Key,
                    g => string.Join(", ", g.Select(x => x.Nome).Distinct()));
        }

        var model = lista
            .OrderByDescending(v => v.DataVenda)
            .Select(v => new VendaListaItemViewModel
            {
                IdVenda = v.IdVenda,
                DataVenda = v.DataVenda,
                FuncionarioNome = nomesFunc.GetValueOrDefault(v.IdFuncionario, $"#{v.IdFuncionario}"),
                Produtos = produtosPorVenda.GetValueOrDefault(v.IdVenda, "—"),
                ValorTotal = v.ValorTotal
            })
            .ToList();

        return View(model);
    }
}
