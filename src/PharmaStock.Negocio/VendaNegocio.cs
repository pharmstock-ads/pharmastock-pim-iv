// Orquestra venda com escolha de lote FEFO.
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Negocio.Fefo;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Negocio;

public record VendaItemPedido(int IdProduto, int Quantidade, decimal PrecoUnitario);

public class VendaNegocio
{
    private readonly IFuncionarioRepositorio _funcionarios;
    private readonly IProdutoRepositorio _produtos;
    private readonly ILoteRepositorio _lotes;
    private readonly IVendaRepositorio _vendas;

    public VendaNegocio(
        IFuncionarioRepositorio funcionarios,
        IProdutoRepositorio produtos,
        ILoteRepositorio lotes,
        IVendaRepositorio vendas)
    {
        _funcionarios = funcionarios;
        _produtos = produtos;
        _lotes = lotes;
        _vendas = vendas;
    }

    public async Task<int> RegistrarAsync(
        int idFuncionario,
        IReadOnlyList<VendaItemPedido> itens,
        CancellationToken ct = default)
    {
        if (itens is null || itens.Count == 0)
            throw new DominioException("Informe ao menos um item de venda.");

        var funcionario = await _funcionarios.ObterPorIdAsync(idFuncionario, ct)
            ?? throw new DominioException("Funcionário não encontrado.");
        if (!funcionario.EstaAtivo)
            throw new DominioException("Funcionário inativo.");

        var registros = new List<VendaItemRegistro>();

        foreach (var item in itens)
        {
            if (item.Quantidade <= 0)
                throw new DominioException("Quantidade deve ser maior que zero.");
            if (item.PrecoUnitario < 0)
                throw new DominioException("Preço unitário não pode ser negativo.");

            _ = await _produtos.ObterPorIdAsync(item.IdProduto, ct)
                ?? throw new DominioException($"Produto {item.IdProduto} não encontrado.");

            var lotes = await _lotes.ListarComEstoquePorProdutoAsync(item.IdProduto, ct);
            var alocacoes = FefoAlocador.Alocar(lotes, item.Quantidade);

            foreach (var aloc in alocacoes)
            {
                registros.Add(new VendaItemRegistro(
                    item.IdProduto,
                    aloc.IdLote,
                    aloc.Quantidade,
                    item.PrecoUnitario));
            }
        }

        return await _vendas.RegistrarVendaCompletaAsync(idFuncionario, registros, ct);
    }

    public Task<IReadOnlyList<(int IdVenda, int IdFuncionario, DateTime DataVenda, decimal ValorTotal)>> ListarAsync(
        CancellationToken ct = default)
        => _vendas.ListarAsync(ct);
}
