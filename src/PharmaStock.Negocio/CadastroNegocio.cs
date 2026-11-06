// Cadastro de fornecedor e produto.
using PharmaStock.Modelos;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Negocio;

public class CadastroNegocio
{
    private readonly IFornecedorRepositorio _fornecedores;
    private readonly IProdutoRepositorio _produtos;
    private readonly ILoteRepositorio _lotes;

    public CadastroNegocio(
        IFornecedorRepositorio fornecedores,
        IProdutoRepositorio produtos,
        ILoteRepositorio lotes)
    {
        _fornecedores = fornecedores;
        _produtos = produtos;
        _lotes = lotes;
    }

    public Task<IReadOnlyList<Fornecedor>> ListarFornecedoresAsync(CancellationToken ct = default)
        => _fornecedores.ListarAsync(ct);

    public async Task<int> CriarFornecedorAsync(string nomeEmpresa, string cnpj, string? telefone, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(nomeEmpresa))
            throw new DominioException("Nome da empresa é obrigatório.");
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new DominioException("CNPJ é obrigatório.");

        return await _fornecedores.InserirAsync(new Fornecedor
        {
            NomeEmpresa = nomeEmpresa.Trim(),
            Cnpj = cnpj.Trim(),
            Telefone = telefone
        }, ct);
    }

    public Task<IReadOnlyList<Produto>> ListarProdutosAsync(CancellationToken ct = default)
        => _produtos.ListarAsync(ct);

    public Task<Produto?> ObterProdutoAsync(int id, CancellationToken ct = default)
        => _produtos.ObterPorIdAsync(id, ct);

    public async Task<int> CriarProdutoAsync(Produto produto, CancellationToken ct = default)
    {
        if (produto.Codigo <= 0)
            throw new DominioException("Código do produto inválido.");
        if (string.IsNullOrWhiteSpace(produto.Nome))
            throw new DominioException("Nome do produto é obrigatório.");

        return await _produtos.InserirAsync(produto, ct);
    }

    public Task<IReadOnlyList<Lote>> ListarLotesPorProdutoAsync(int idProduto, CancellationToken ct = default)
        => _lotes.ListarPorProdutoAsync(idProduto, ct);
}
