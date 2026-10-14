using System.Globalization;
using estoque_farmacia.Models;
using estoque_farmacia.Services;

namespace estoque_farmacia.UI;

public class VendaUI
{
    private readonly ProdutoService _produtoService;
    private readonly VendaService _vendaService;
    private readonly Estoque _estoque;

    private readonly List<ItemVenda> _carrinho = new();
    private decimal _desconto;

    public VendaUI(ProdutoService produtoService, VendaService vendaService, Estoque estoque)
    {
        _produtoService = produtoService;
        _vendaService = vendaService;
        _estoque = estoque;
    }

    public void ProcessarMenuVenda()
    {
        bool continuar = true;
        while (continuar)
        {
            MostrarMenuVenda();
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    AdicionarProduto();
                    break;
                case "2":
                    RemoverItem();
                    break;
                case "3":
                    FinalizarVenda();
                    break;
                case "4":
                    CancelarVenda();
                    break;
                case "5":
                    ListarVendas();
                    break;
                case "6":
                    continuar = false;
                    Console.WriteLine("  Voltando ao menu principal.");
                    break;
                default:
                    Console.WriteLine("  Opcao invalida.");
                    Thread.Sleep(800);
                    break;
            }
        }
    }

    private void MostrarMenuVenda()
    {
        Console.Clear();
        Console.WriteLine("\n  === CONTROLE DE VENDA ===\n");
        ExibirResumoCarrinho();
        Console.WriteLine();
        Console.WriteLine("  1 - Adicionar produto");
        Console.WriteLine("  2 - Remover item do carrinho");
        Console.WriteLine("  3 - Finalizar venda");
        Console.WriteLine("  4 - Cancelar venda");
        Console.WriteLine("  5 - Listar vendas realizadas");
        Console.WriteLine("  6 - Voltar");
        Console.Write("\n  Opcao: ");
    }

    private void ExibirResumoCarrinho()
    {
        if (_carrinho.Count == 0)
        {
            Console.WriteLine("  Carrinho vazio.");
            return;
        }

        Console.WriteLine("  --- Carrinho ---");
        foreach (var i in _carrinho)
        {
            Console.WriteLine($"  ID {i.IdProduto} | {i.NomeProduto} | Qtd: {i.Quantidade} | Unit: R$ {i.PrecoUnitario:F2} | Sub: R$ {i.Subtotal:F2}");
        }

        decimal subtotal = _carrinho.Sum(i => i.Subtotal);
        decimal total = Math.Max(0, subtotal - _desconto);
        Console.WriteLine($"  Subtotal: R$ {subtotal:F2} | Desconto: R$ {_desconto:F2} | Total: R$ {total:F2}");
    }

    private void AdicionarProduto()
    {
        Console.Clear();
        Console.WriteLine("\n  Adicionar produto (ID, cod. barras ou nome)\n");

        var produtos = _produtoService.ListarTodos();
        if (produtos.Count == 0)
        {
            Console.WriteLine("  Nenhum produto cadastrado.");
            Pausar();
            return;
        }

        Console.Write("  Busca: ");
        string texto = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrEmpty(texto))
        {
            Console.WriteLine("  Busca vazia.");
            Pausar();
            return;
        }

        Produto? produto = produtos.FirstOrDefault(p =>
            !string.IsNullOrEmpty(p.CodigoBarras) &&
            string.Equals(p.CodigoBarras.Trim(), texto, StringComparison.Ordinal));

        if (produto == null && int.TryParse(texto, NumberStyles.Integer, CultureInfo.InvariantCulture, out int id))
            produto = produtos.FirstOrDefault(p => p.Id == id);

        if (produto == null)
            produto = produtos.FirstOrDefault(p => p.NomeProduto.Contains(texto, StringComparison.OrdinalIgnoreCase));

        if (produto == null)
        {
            Console.WriteLine("  Produto nao encontrado.");
            Pausar();
            return;
        }

        decimal disponivel = _estoque.ObterDisponivel(produto.Id);
        Console.WriteLine($"  Produto: {produto.NomeProduto} | Estoque disponivel: {disponivel}");

        Console.Write("  Quantidade: ");
        if (!int.TryParse(Console.ReadLine(), out int qtd) || qtd <= 0)
        {
            Console.WriteLine("  Quantidade invalida.");
            Pausar();
            return;
        }

        if (!_estoque.TemEstoque(produto.Id, qtd))
        {
            Console.WriteLine($"  Estoque insuficiente. Disponivel: {disponivel}.");
            Pausar();
            return;
        }

        var existente = _carrinho.FirstOrDefault(i => i.IdProduto == produto.Id);
        if (existente != null)
        {
            int novaQtd = existente.Quantidade + qtd;
            if (!_estoque.TemEstoque(produto.Id, novaQtd))
            {
                Console.WriteLine($"  Estoque insuficiente para quantidade total {novaQtd}. Disponivel: {disponivel}.");
                Pausar();
                return;
            }
            existente.Quantidade = novaQtd;
        }
        else
        {
            _carrinho.Add(new ItemVenda
            {
                IdProduto = produto.Id,
                NomeProduto = produto.NomeProduto,
                Quantidade = qtd,
                PrecoUnitario = produto.PrecoVenda
            });
        }

        Console.WriteLine("  Produto adicionado ao carrinho.");
        Pausar();
    }

    private void RemoverItem()
    {
        if (_carrinho.Count == 0)
        {
            Console.WriteLine("\n  Carrinho vazio.");
            Pausar();
            return;
        }

        ExibirResumoCarrinho();
        Console.Write("\n  ID do produto a remover: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("  ID invalido.");
            Pausar();
            return;
        }

        int removidos = _carrinho.RemoveAll(i => i.IdProduto == id);
        Console.WriteLine(removidos > 0 ? "  Item removido." : "  Item nao encontrado no carrinho.");
        Pausar();
    }

    private void FinalizarVenda()
    {
        if (_carrinho.Count == 0)
        {
            Console.WriteLine("\n  Adicione ao menos um produto.");
            Pausar();
            return;
        }

        ExibirResumoCarrinho();
        Console.Write("\n  Desconto (R$, 0 se nao houver): ");
        if (!decimal.TryParse(Console.ReadLine(), out _desconto) || _desconto < 0)
            _desconto = 0;

        if (_vendaService.Finalizar(_carrinho.ToList(), _desconto))
        {
            _carrinho.Clear();
            _desconto = 0;
        }

        Pausar();
    }

    private void CancelarVenda()
    {
        if (_carrinho.Count == 0)
        {
            Console.WriteLine("\n  Nada para cancelar.");
            Pausar();
            return;
        }

        Console.Write("\n  Cancelar venda atual? (S/N): ");
        string resp = (Console.ReadLine() ?? "").Trim().ToUpper();
        if (resp == "S")
        {
            _carrinho.Clear();
            _desconto = 0;
            Console.WriteLine("  Venda cancelada.");
        }

        Pausar();
    }

    private void ListarVendas()
    {
        Console.Clear();
        Console.WriteLine("\n  === VENDAS REALIZADAS ===\n");

        var vendas = _vendaService.ListarTodos();
        if (vendas.Count == 0)
        {
            Console.WriteLine("  Nenhuma venda registrada.");
            Pausar();
            return;
        }

        foreach (var v in vendas.OrderByDescending(x => x.Id))
        {
            Console.WriteLine($"  Venda #{v.Id} | {v.DataVenda:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"    Subtotal: R$ {v.Subtotal:F2} | Desconto: R$ {v.Desconto:F2} | Total: R$ {v.ValorTotal:F2}");
            Console.WriteLine("    Itens:");
            foreach (var i in v.Itens)
            {
                Console.WriteLine($"      - ID {i.IdProduto} | {i.NomeProduto} | Qtd: {i.Quantidade} | Unit: R$ {i.PrecoUnitario:F2} | Sub: R$ {i.Subtotal:F2}");
            }
            Console.WriteLine();
        }

        Pausar();
    }

    private static void Pausar()
    {
        Console.WriteLine("\n  Pressione ENTER para continuar...");
        Console.ReadLine();
    }
}
