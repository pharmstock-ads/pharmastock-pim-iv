using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Negocio;
using PharmaStock.Web.Models;
using PharmaStock.Web.ViewModels;

namespace PharmaStock.Web.Controllers;

public class HomeController : Controller
{
    private readonly AlertaNegocio _alertas;
    private readonly CadastroNegocio _cadastro;
    private readonly VendaNegocio _vendas;

    public HomeController(AlertaNegocio alertas, CadastroNegocio cadastro, VendaNegocio vendas)
    {
        _alertas = alertas;
        _cadastro = cadastro;
        _vendas = vendas;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetInt32("FuncionarioId") is null)
            return RedirectToAction("Login", "Account");

        var produtos = await _cadastro.ListarProdutosAsync();
        var fornecedores = await _cadastro.ListarFornecedoresAsync();
        var nomes = produtos.ToDictionary(p => p.Id, p => p.Nome);

        var aVencer = await _alertas.LotesAVencerAsync();
        var estoqueBaixo = await _alertas.EstoqueBaixoAsync();
        var vendas = await _vendas.ListarAsync();

        var hoje = DateTime.Today;
        var alertas = aVencer
            .Take(5)
            .Select(l => new HomeAlertaItemViewModel
            {
                Tipo = "A vencer",
                IdProduto = l.IdProduto,
                ProdutoNome = nomes.GetValueOrDefault(l.IdProduto, $"Produto #{l.IdProduto}"),
                NumeroLote = l.NumeroLote,
                Validade = l.Validade,
                Quantidade = l.Quantidade
            })
            .Concat(estoqueBaixo
                .Take(5)
                .Select(l => new HomeAlertaItemViewModel
                {
                    Tipo = "Estoque baixo",
                    IdProduto = l.IdProduto,
                    ProdutoNome = nomes.GetValueOrDefault(l.IdProduto, $"Produto #{l.IdProduto}"),
                    NumeroLote = l.NumeroLote,
                    Validade = l.Validade,
                    Quantidade = l.Quantidade
                }))
            .Take(8)
            .ToList();

        var model = new HomeResumoViewModel
        {
            NomeFuncionario = HttpContext.Session.GetString("FuncionarioNome") ?? "funcionário",
            TotalProdutos = produtos.Count,
            TotalFornecedores = fornecedores.Count,
            VendasHoje = vendas.Count(v => v.DataVenda.Date == hoje),
            LotesAVencerCount = aVencer.Count,
            EstoqueBaixoCount = estoqueBaixo.Count,
            Alertas = alertas,
            VendasRecentes = vendas
                .OrderByDescending(v => v.DataVenda)
                .Take(5)
                .Select(v => new HomeVendaItemViewModel
                {
                    IdVenda = v.IdVenda,
                    DataVenda = v.DataVenda,
                    ValorTotal = v.ValorTotal
                })
                .ToList()
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
