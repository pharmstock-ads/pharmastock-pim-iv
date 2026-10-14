// importa o Model Produto pra poder criar e usar objetos dele aqui
using estoque_farmacia.Models;
// importa o ProdutoService pra chamar os métodos de negócio
using estoque_farmacia.Services;

// namespace da camada de interface com o usuário (console)
namespace estoque_farmacia.UI;

// essa classe só cuida de mostrar menus e coletar dados do usuário
// a lógica de verdade fica no ProdutoService
public class ProdutoUI
{
    // guarda a referência ao service — readonly = não troca depois de inicializar
    private readonly ProdutoService _produtoService;

    // construtor recebe o service pronto de fora (injeção de dependência)
    public ProdutoUI(ProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    // imprime o menu de opções na tela
    public void MostrarMenuProduto()
    {
        Console.WriteLine("\n");
        Console.Write("===CONTROLE DE PRODUTO===\n");
        Thread.Sleep(1500);
        Console.Write("\n");
        Console.Write("Escolha uma opção: \n");
        Console.Write("1 - Cadastrar produto\n");
        Console.Write("2 - Listar produtos\n");
        Console.Write("3 - Remover produtos\n");
        Console.Write("4 - Voltar\n");
        Console.Write(">>> ");
        Console.Write("\n");
    }

    // loop principal do menu — fica rodando até o usuário escolher voltar
    public void ProcessarMenuProduto()
    {
        bool continuar = true;
        while (continuar)
        {
            MostrarMenuProduto();
            string opcao = Console.ReadLine();

            // switch decide qual ação executar baseado no que o usuário digitou
            switch (opcao)
            {
                case "1":
                CadastrarProduto();
                break;

                case "2":
                Console.Clear();
                Console.WriteLine("===LISTA DE PRODUTOS===");
                Console.WriteLine("\n");
                // pede a lista ao service e exibe cada item
                var lista = _produtoService.ListarTodos();
                if (lista.Count == 0)
                    {
                        Console.WriteLine("Nenhum produto cadastrado.");
                        Console.WriteLine("\n");
                        Thread.Sleep(3500);
                    }
                    else
                    {
                        // percorre cada produto e monta a linha de exibição
                        foreach (var p in lista)
                        {
                            // operador ternário: se requer receita mostra "Sim", senão "Nao"
                            string receita = p.RequerReceita ? "Sim" : "Nao";
                            string cb = string.IsNullOrEmpty(p.CodigoBarras) ? "-" : p.CodigoBarras;
                            Console.WriteLine($"  ID: {p.Id} | {cb} | {p.NomeProduto} | Venda: R$ {p.PrecoVenda:F2} | Custo: R$ {p.PrecoCusto:F2} | Receita: {receita}");
                        }
                    }

                break;

                case "3":
                RemoverProduto();
                break;

                case "4":
                // seta false pra sair do while e voltar pro menu principal
                continuar = false;
                Console.WriteLine("Voltando ao menu principal.");
                break;

                default:
                Console.WriteLine("Opção inválida.");
                Thread.Sleep(1000);
                break;
            }
        }
    }

    // coleta todos os dados do produto com o usuário e manda pro service salvar
    public void CadastrarProduto()
    {
        Console.WriteLine();

        // loop garante que o nome não fique vazio
        Console.Write("Nome do produto: ");
        string nome = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(nome))
        {
            Console.Write("  Nome nao pode ser vazio. Digite novamente: ");
            nome = Console.ReadLine();
        }

        // loop até o código de barras ser um número inteiro válido
        // a validação usa o método estático do próprio Model Produto
        string codigoBarras;
        while (true)
        {
            Console.Write("Codigo de barras (numero inteiro): ");
            codigoBarras = (Console.ReadLine() ?? "").Trim();
            if (Produto.CodigoBarrasValido(codigoBarras))
                break;
            Console.WriteLine("  Informe um numero inteiro valido (ex: 789012).");
        }

        // decimal.TryParse tenta converter o texto digitado em número decimal
        // o loop fica pedindo até conseguir um valor válido e positivo
        Console.Write("Preco de venda (ex: 12,50): R$ ");
        decimal precoVenda;
        while (!decimal.TryParse(Console.ReadLine(), out precoVenda) || precoVenda < 0 || decimal.Round(precoVenda, 2) != precoVenda)
        {
            Console.Write("  Valor invalido. Use virgula e ate 2 casas decimais (ex: 12,50): R$ ");
        }

        Console.Write("Preco de custo (ex: 8,00): R$ ");
        decimal precoCusto;
        while (!decimal.TryParse(Console.ReadLine(), out precoCusto) || precoCusto < 0 || decimal.Round(precoCusto, 2) != precoCusto)
        {
            Console.Write("  Valor invalido. Use virgula e ate 2 casas decimais (ex: 8,00): R$ ");
        }

        Console.Write("ID do fornecedor: ");
        int idFornecedor;
        while (!int.TryParse(Console.ReadLine(), out idFornecedor) || idFornecedor <= 0)
        {
            Console.Write("  Digite um numero valido: ");
        }

        // loop até receber S ou N — o service vai guardar isso como bool
        bool requerReceita;
        while (true)
        {
            Console.Write("Requer receita medica? (S/N): ");
            string resposta = Console.ReadLine()?.Trim().ToUpper();
            if (resposta == "S") { requerReceita = true; break; }
            if (resposta == "N") { requerReceita = false; break; }
            Console.WriteLine("  Digite apenas S ou N.");
        }

        // monta o objeto Produto com os dados coletados
        // o Id ainda é 0 aqui — o service vai atribuir o Id correto ao salvar
        Produto novo = new Produto
        {
            NomeProduto = nome,
            CodigoBarras = codigoBarras,
            PrecoVenda = precoVenda,
            PrecoCusto = precoCusto,
            IdFornecedor = idFornecedor,
            RequerReceita = requerReceita
        };

        // passa pro service — ele faz as validações e salva na lista
        _produtoService.Salvar(novo);

        Console.WriteLine("  Pressione ENTER para continuar...");
        Console.ReadLine();
    }

    // pede o ID e chama o service pra remover
    public void RemoverProduto()
    {
        Console.Write("\nID do produto a remover: ");

        // int.TryParse converte o texto pra int — retorna false se não for número
        if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
        {
            // service retorna true se removeu, false se não encontrou
            if (_produtoService.Remover(id))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  Produto removido com sucesso.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Produto nao encontrado.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  ID invalido.");
            Console.ResetColor();
        }

        Console.WriteLine("  Pressione ENTER para continuar...");
        Console.ReadLine();
    }
}
