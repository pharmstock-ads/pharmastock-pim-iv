using estoque_farmacia.Models;

namespace estoque_farmacia.Services;

public class LoteService
{
    private readonly List<Lote> Itens = new();
    private int _nextId = 1;

    private readonly ProdutoService _produtoService;

    public LoteService(ProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    public string UltimoErro { get; private set; } = string.Empty;

    public bool Salvar(Lote novo)
    {
        try
        {
            if (novo == null)
            {
                UltimoErro = "  ERRO: Lote invalido."; Console.WriteLine(UltimoErro);
                return false;
            }

            if (_produtoService.BuscarPorId(novo.IdProduto) == null)
            {
                UltimoErro = "  ERRO: Produto nao encontrado."; Console.WriteLine(UltimoErro);
                return false;
            }

            if (novo.NumeroLote <= 0)
            {
                UltimoErro = "  ERRO: Numero do lote deve ser maior que zero."; Console.WriteLine(UltimoErro);
                return false;
            }

            if (novo.Quantidade <= 0)
            {
                UltimoErro = "  ERRO: Quantidade deve ser maior que zero."; Console.WriteLine(UltimoErro);
                return false;
            }

            novo.Id = _nextId++;
            Itens.Add(novo);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  Lote salvo com sucesso! ID: {novo.Id}");
            Console.ResetColor();
            return true;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            UltimoErro = $"\n  ERRO ao salvar lote: {ex.Message}"; Console.WriteLine(UltimoErro);
            Console.ResetColor();
            return false;
        }
    }

    public List<Lote> ListarTodos() => Itens.ToList();

    public decimal ObterQuantidadeDisponivel(int idProduto) =>
        Itens.Where(l => l.IdProduto == idProduto).Sum(l => l.Quantidade);

    public bool BaixarEstoque(int idProduto, int quantidade)
    {
        if (quantidade <= 0) return false;

        var lotes = Itens.Where(l => l.IdProduto == idProduto).OrderBy(l => l.Validade).ToList();
        decimal disponivel = lotes.Sum(l => l.Quantidade);
        if (disponivel < quantidade) return false;

        decimal restante = quantidade;
        foreach (var lote in lotes)
        {
            if (restante <= 0) break;

            if (lote.Quantidade >= restante)
            {
                lote.Quantidade -= restante;
                restante = 0;
            }
            else
            {
                restante -= lote.Quantidade;
                lote.Quantidade = 0;
            }
        }

        Itens.RemoveAll(l => l.Quantidade <= 0);
        return true;
    }

    public bool Remover(int id)
    {
        try
        {
            var l = Itens.FirstOrDefault(x => x.Id == id);
            if (l == null) return false;
            Itens.Remove(l);
            return true;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            UltimoErro = $"\n  ERRO ao remover lote: {ex.Message}"; Console.WriteLine(UltimoErro);
            Console.ResetColor();
            return false;
        }
    }
}
