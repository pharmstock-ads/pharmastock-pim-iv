// Orquestra registro de entrada de estoque.
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Negocio.Servicos;

public record EntradaItemPedido(
    int IdProduto,
    string NumeroLote,
    DateTime Validade,
    decimal Quantidade,
    decimal PrecoUnitario);

public class EntradaServico
{
    private readonly IFuncionarioRepositorio _funcionarios;
    private readonly IProdutoRepositorio _produtos;
    private readonly IEntradaRepositorio _entradas;

    public EntradaServico(
        IFuncionarioRepositorio funcionarios,
        IProdutoRepositorio produtos,
        IEntradaRepositorio entradas)
    {
        _funcionarios = funcionarios;
        _produtos = produtos;
        _entradas = entradas;
    }

    public async Task<int> RegistrarAsync(
        int idFuncionario,
        IReadOnlyList<EntradaItemPedido> itens,
        CancellationToken ct = default)
    {
        if (itens is null || itens.Count == 0)
            throw new DominioException("Informe ao menos um item de entrada.");

        var funcionario = await _funcionarios.ObterPorIdAsync(idFuncionario, ct)
            ?? throw new DominioException("Funcionário não encontrado.");
        if (!funcionario.EstaAtivo)
            throw new DominioException("Funcionário inativo.");

        var registros = new List<EntradaItemRegistro>();

        foreach (var item in itens)
        {
            if (item.Quantidade <= 0)
                throw new DominioException("Quantidade deve ser maior que zero.");
            if (item.PrecoUnitario < 0)
                throw new DominioException("Preço unitário não pode ser negativo.");
            if (string.IsNullOrWhiteSpace(item.NumeroLote))
                throw new DominioException("Número do lote é obrigatório.");

            var produto = await _produtos.ObterPorIdAsync(item.IdProduto, ct)
                ?? throw new DominioException($"Produto {item.IdProduto} não encontrado.");

            registros.Add(new EntradaItemRegistro(
                produto.Id,
                item.NumeroLote.Trim(),
                item.Validade.Date,
                item.Quantidade,
                item.PrecoUnitario));
        }

        return await _entradas.RegistrarEntradaCompletaAsync(idFuncionario, registros, ct);
    }
}
