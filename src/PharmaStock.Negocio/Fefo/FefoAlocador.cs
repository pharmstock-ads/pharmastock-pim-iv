// Aloca quantidade nos lotes por validade (FEFO / multilote).
using PharmaStock.Modelos;
using PharmaStock.Negocio.Excecoes;

namespace PharmaStock.Negocio.Fefo;

public record AlocacaoLote(int IdLote, int Quantidade);

public static class FefoAlocador
{
    public static IReadOnlyList<AlocacaoLote> Alocar(IEnumerable<Lote> lotes, int quantidadePedida)
    {
        if (quantidadePedida <= 0)
            throw new DominioException("Quantidade pedida deve ser maior que zero.");

        var disponiveis = lotes
            .Where(l => l.Quantidade > 0)
            .OrderBy(l => l.Validade)
            .ThenBy(l => l.Id)
            .ToList();

        var estoqueTotal = disponiveis.Sum(l => l.Quantidade);
        if (estoqueTotal < quantidadePedida)
            throw new EstoqueInsuficienteException(
                $"Estoque insuficiente. Pedido: {quantidadePedida}, disponível: {estoqueTotal}.");

        var restante = quantidadePedida;
        var alocacoes = new List<AlocacaoLote>();

        foreach (var lote in disponiveis)
        {
            if (restante <= 0)
                break;

            var qtdLote = (int)Math.Floor(lote.Quantidade);
            if (qtdLote <= 0)
                continue;

            var usar = Math.Min(qtdLote, restante);
            alocacoes.Add(new AlocacaoLote(lote.Id, usar));
            restante -= usar;
        }

        if (restante > 0)
            throw new EstoqueInsuficienteException(
                $"Estoque insuficiente após alocação FEFO. Faltaram {restante} unidades.");

        return alocacoes;
    }
}
