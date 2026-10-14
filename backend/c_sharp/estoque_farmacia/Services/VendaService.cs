using estoque_farmacia.Models;

namespace estoque_farmacia.Services;

public class VendaService
{
    private readonly List<Venda> Vendas = new();
    private int _nextId = 1;

    private readonly ProdutoService _produtoService;
    private readonly Estoque _estoque;

    public string UltimoErro { get; private set; } = string.Empty;

    public VendaService(ProdutoService produtoService, Estoque estoque)
    {
        _produtoService = produtoService;
        _estoque = estoque;
    }

    public bool Finalizar(List<ItemVenda> itens, decimal desconto, int idFuncionario = 0)
    {
        try
        {
            if (itens == null || itens.Count == 0)
            {
                UltimoErro = "  ERRO: Carrinho vazio.";
                Console.WriteLine(UltimoErro);
                return false;
            }

            foreach (var item in itens)
            {
                if (!_estoque.TemEstoque(item.IdProduto, item.Quantidade))
                {
                    var produto = _produtoService.BuscarPorId(item.IdProduto);
                    var nome = produto?.NomeProduto ?? $"ID {item.IdProduto}";
                    var disponivel = _estoque.ObterDisponivel(item.IdProduto);
                    UltimoErro = $"  ERRO: Estoque insuficiente para '{nome}'. Disponivel: {disponivel}.";
                    Console.WriteLine(UltimoErro);
                    return false;
                }
            }

            foreach (var item in itens)
            {
                if (!_estoque.Baixar(item.IdProduto, item.Quantidade))
                {
                    UltimoErro = "  ERRO: Falha ao baixar estoque dos lotes.";
                    Console.WriteLine(UltimoErro);
                    return false;
                }
            }

            decimal subtotal = itens.Sum(i => i.Subtotal);
            decimal total = Math.Max(0, subtotal - desconto);

            var venda = new Venda
            {
                IdFuncionario = idFuncionario,
                DataVenda = DateTime.Now,
                Subtotal = subtotal,
                Desconto = desconto,
                ValorTotal = total,
                Itens = itens.Select(i => new ItemVenda
                {
                    IdProduto = i.IdProduto,
                    NomeProduto = i.NomeProduto,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            };

            venda.Id = _nextId++;
            Vendas.Add(venda);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  Venda #{venda.Id} registrada. Total: R$ {venda.ValorTotal:F2}");
            Console.ResetColor();
            return true;
        }
        catch (Exception ex)
        {
            UltimoErro = $"  ERRO ao finalizar venda: {ex.Message}";
            Console.WriteLine(UltimoErro);
            return false;
        }
    }

    public List<Venda> ListarTodos() => Vendas.ToList();
}
