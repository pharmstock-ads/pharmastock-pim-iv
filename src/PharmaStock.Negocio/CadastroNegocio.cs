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

        var nome = nomeEmpresa.Trim();
        if (nome.Length > 100)
            throw new DominioException("Nome da empresa deve ter no máximo 100 caracteres.");

        var cnpjDigitos = SomenteDigitos(cnpj);
        if (cnpjDigitos.Length != 14)
            throw new DominioException("CNPJ deve conter 14 dígitos.");

        if (await _fornecedores.ObterPorCnpjAsync(cnpjDigitos, ct) is not null)
            throw new DominioException("Já existe um fornecedor com este CNPJ.");

        string? telefoneNormalizado = null;
        if (!string.IsNullOrWhiteSpace(telefone))
        {
            telefoneNormalizado = telefone.Trim();
            if (telefoneNormalizado.Length > 30)
                throw new DominioException("Telefone deve ter no máximo 30 caracteres.");
        }

        return await _fornecedores.InserirAsync(new Fornecedor
        {
            NomeEmpresa = nome,
            Cnpj = cnpjDigitos,
            Telefone = telefoneNormalizado
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

        var nome = produto.Nome.Trim();
        if (nome.Length > 100)
            throw new DominioException("Nome do produto deve ter no máximo 100 caracteres.");

        // Obrigatório na aplicação: a coluna codbarra é UNIQUE no SQL Server e
        // um segundo NULL/vazio gera SqlException em vez de mensagem amigável.
        if (string.IsNullOrWhiteSpace(produto.CodBarra))
            throw new DominioException("Código de barras é obrigatório.");

        var codBarra = produto.CodBarra.Trim();
        if (codBarra.Length > 25)
            throw new DominioException("Código de barras deve ter no máximo 25 caracteres.");

        if (await _produtos.ObterPorCodigoAsync(produto.Codigo, ct) is not null)
            throw new DominioException("Já existe um produto com este código.");

        if (await _produtos.ObterPorCodBarraAsync(codBarra, ct) is not null)
            throw new DominioException("Já existe um produto com este código de barras.");

        produto.Nome = nome;
        produto.CodBarra = codBarra;
        return await _produtos.InserirAsync(produto, ct);
    }

    public Task<IReadOnlyList<Lote>> ListarLotesPorProdutoAsync(int idProduto, CancellationToken ct = default)
        => _lotes.ListarPorProdutoAsync(idProduto, ct);

    private static string SomenteDigitos(string? valor)
        => string.IsNullOrWhiteSpace(valor)
            ? string.Empty
            : new string(valor.Where(char.IsDigit).ToArray());
}
