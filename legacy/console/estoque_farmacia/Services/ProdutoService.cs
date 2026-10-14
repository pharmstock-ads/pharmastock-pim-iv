// precisamos do Model Produto pra trabalhar com ele aqui
using estoque_farmacia.Models;

// namespace de serviços — onde fica a lógica de negócio
namespace estoque_farmacia.Services;

// service responsável por tudo que envolve produtos:
// salvar, listar, buscar e remover
public class ProdutoService
{
    // lista que guarda os produtos em memória enquanto o programa roda
    private readonly List<Produto> Itens = new();

    // contador pra gerar IDs únicos em sequência (1, 2, 3...)
    private int _nextId = 1;

    // precisamos do FornecedorService pra validar se o fornecedor existe antes de salvar
    private readonly FornecedorService _fornecedorService;

    // propriedade que guarda a última mensagem de erro
    // private set = só este service pode escrever, quem usa só lê
    public string UltimoErro { get; private set; } = string.Empty;

    // construtor: recebe o FornecedorService por injeção de dependência
    // assim não criamos um novo service dentro daqui (boa prática de OO)
    public ProdutoService(FornecedorService fornecedorService)
    {
        _fornecedorService = fornecedorService;
    }

    // salva um novo produto — retorna true se deu certo, false se teve erro
    public bool Salvar(Produto novoProduto)
    {
        try
        {
            // validação 1: nome obrigatório
            if (novoProduto == null || string.IsNullOrWhiteSpace(novoProduto.NomeProduto))
            {
                UltimoErro = "  ERRO: Nome do produto e obrigatorio."; Console.WriteLine(UltimoErro);
                return false;
            }

            // validação 2: código de barras precisa ser numérico
            if (!Produto.CodigoBarrasValido(novoProduto.CodigoBarras))
            {
                UltimoErro = "  ERRO: Codigo de barras deve ser um numero inteiro."; Console.WriteLine(UltimoErro);
                return false;
            }

            // validação 3: precisa ter um fornecedor informado
            if (novoProduto.IdFornecedor <= 0)
            {
                UltimoErro = "  ERRO: ID do fornecedor e obrigatorio."; Console.WriteLine(UltimoErro);
                return false;
            }

            // validação 4: o fornecedor informado precisa existir de verdade
            if (_fornecedorService.BuscarPorId(novoProduto.IdFornecedor) == null)
            {
                UltimoErro = "  ERRO: Fornecedor nao encontrado."; Console.WriteLine(UltimoErro);
                return false;
            }

            // checa se já existe produto com o mesmo código de barras
            // Any() = retorna true se encontrar pelo menos um
            if (Itens.Any(p => p.CodigoBarras == novoProduto.CodigoBarras))
            {
                UltimoErro = "  ERRO: Ja existe produto com este codigo de barras."; Console.WriteLine(UltimoErro);
                return false;
            }

            // tudo ok — atribui o próximo ID e adiciona na lista
            novoProduto.Id = _nextId++;
            Itens.Add(novoProduto);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  Produto '{novoProduto.NomeProduto}' salvo com sucesso! ID: {novoProduto.Id}");
            Console.ResetColor();
            return true;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            UltimoErro = $"\n  ERRO ao salvar produto: {ex.Message}"; Console.WriteLine(UltimoErro);
            Console.ResetColor();
            return false;
        }
    }

    // retorna uma cópia da lista — .ToList() evita que alguém mexa na lista interna
    public List<Produto> ListarTodos() => Itens.ToList();

    // busca um produto pelo ID — retorna null se não encontrar
    public Produto? BuscarPorId(int id) => Itens.FirstOrDefault(p => p.Id == id);

    // remove produto da lista pelo ID
    public bool Remover(int id)
    {
        try
        {
            // FirstOrDefault retorna o produto ou null se não existir
            var p = Itens.FirstOrDefault(x => x.Id == id);
            if (p == null) return false;
            Itens.Remove(p);
            return true;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            UltimoErro = $"\n  ERRO ao remover produto: {ex.Message}"; Console.WriteLine(UltimoErro);
            Console.ResetColor();
            return false;
        }
    }
}
