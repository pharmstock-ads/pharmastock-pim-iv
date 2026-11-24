// Validacoes de cadastro (CNPJ, tamanhos) com repositorios fake.
using PharmaStock.Modelos;
using PharmaStock.Negocio;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Tests;

file class FornecedorRepoFake : IFornecedorRepositorio
{
    private readonly List<Fornecedor> _itens = [];
    private int _seq = 1;

    public Task<IReadOnlyList<Fornecedor>> ListarAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Fornecedor>>(_itens);

    public Task<Fornecedor?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_itens.FirstOrDefault(f => f.Id == id));

    public Task<Fornecedor?> ObterPorCnpjAsync(string cnpj, CancellationToken ct = default)
        => Task.FromResult(_itens.FirstOrDefault(f => f.Cnpj == cnpj));

    public Task<int> InserirAsync(Fornecedor fornecedor, CancellationToken ct = default)
    {
        fornecedor.Id = _seq++;
        _itens.Add(fornecedor);
        return Task.FromResult(fornecedor.Id);
    }
}

file class ProdutoRepoFake : IProdutoRepositorio
{
    private readonly List<Produto> _itens = [];
    private int _seq = 1;

    public Task<IReadOnlyList<Produto>> ListarAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Produto>>(_itens);

    public Task<Produto?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_itens.FirstOrDefault(p => p.Id == id));

    public Task<Produto?> ObterPorCodigoAsync(int codigo, CancellationToken ct = default)
        => Task.FromResult(_itens.FirstOrDefault(p => p.Codigo == codigo));

    public Task<Produto?> ObterPorCodBarraAsync(string codBarra, CancellationToken ct = default)
        => Task.FromResult(_itens.FirstOrDefault(p => p.CodBarra == codBarra));

    public Task<int> InserirAsync(Produto produto, CancellationToken ct = default)
    {
        produto.Id = _seq++;
        _itens.Add(produto);
        return Task.FromResult(produto.Id);
    }
}

file class LoteRepoFake : ILoteRepositorio
{
    public Task<IReadOnlyList<Lote>> ListarPorProdutoAsync(int idProduto, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Lote>>([]);

    public Task<IReadOnlyList<Lote>> ListarComEstoquePorProdutoAsync(int idProduto, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Lote>>([]);
}

public class CadastroNegocioTests
{
    private static CadastroNegocio Novo(
        IFornecedorRepositorio? fornecedores = null,
        IProdutoRepositorio? produtos = null)
        => new(fornecedores ?? new FornecedorRepoFake(), produtos ?? new ProdutoRepoFake(), new LoteRepoFake());

    [Fact]
    public async Task CriarFornecedor_RemoveMascaraEGrava14Digitos()
    {
        var repo = new FornecedorRepoFake();
        var negocio = Novo(repo);

        var id = await negocio.CriarFornecedorAsync("Acme Farma", "12.345.678/0001-95", "1633334444");

        var salvo = await repo.ObterPorIdAsync(id);
        Assert.NotNull(salvo);
        Assert.Equal("12345678000195", salvo!.Cnpj);
    }

    [Fact]
    public async Task CriarFornecedor_CnpjCurto_Lanca()
    {
        var negocio = Novo();
        await Assert.ThrowsAsync<DominioException>(() =>
            negocio.CriarFornecedorAsync("Acme", "123", null));
    }

    [Fact]
    public async Task CriarFornecedor_CnpjDuplicado_Lanca()
    {
        var repo = new FornecedorRepoFake();
        var negocio = Novo(repo);
        await negocio.CriarFornecedorAsync("Acme", "12345678000195", null);

        await Assert.ThrowsAsync<DominioException>(() =>
            negocio.CriarFornecedorAsync("Outra", "12345678000195", null));
    }

    [Fact]
    public async Task CriarFornecedor_NomeLongo_Lanca()
    {
        var negocio = Novo();
        await Assert.ThrowsAsync<DominioException>(() =>
            negocio.CriarFornecedorAsync(new string('A', 101), "12345678000195", null));
    }

    [Fact]
    public async Task CriarProduto_NomeOuCodBarraLongo_Lanca()
    {
        var negocio = Novo();

        await Assert.ThrowsAsync<DominioException>(() =>
            negocio.CriarProdutoAsync(new Produto
            {
                Codigo = 1,
                Nome = new string('N', 101)
            }));

        await Assert.ThrowsAsync<DominioException>(() =>
            negocio.CriarProdutoAsync(new Produto
            {
                Codigo = 2,
                Nome = "Ok",
                CodBarra = new string('1', 26)
            }));
    }

    [Fact]
    public async Task CriarProduto_SemCodigoBarras_Lanca()
    {
        var negocio = Novo();

        await Assert.ThrowsAsync<DominioException>(() =>
            negocio.CriarProdutoAsync(new Produto { Codigo = 10, Nome = "A" }));

        await Assert.ThrowsAsync<DominioException>(() =>
            negocio.CriarProdutoAsync(new Produto { Codigo = 11, Nome = "B", CodBarra = "   " }));
    }

    [Fact]
    public async Task CriarProduto_ComCodigoBarras_Ok()
    {
        var negocio = Novo();
        var id = await negocio.CriarProdutoAsync(new Produto
        {
            Codigo = 10,
            Nome = "Dipirona",
            CodBarra = "7891000100028"
        });
        Assert.True(id > 0);
    }
}
