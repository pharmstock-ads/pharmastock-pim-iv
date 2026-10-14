using estoque_farmacia.Models;
using estoque_farmacia.Services;
namespace estoque_farmacia.UI;

public class Menu
{
    private readonly FornecedorUI _fornecedorUI;
    private readonly ProdutoUI _produtoUI;
    private readonly FuncionarioUI _funcionarioUI;
    private readonly LoteUI _loteUI;
    private readonly VendaUI _vendaUI;

    public Menu(FuncionarioUI funcionarioUI, ProdutoUI produtoUI, FornecedorUI fornecedorUI, LoteUI loteUI, VendaUI vendaUI)
    {
        _funcionarioUI = funcionarioUI;
        _produtoUI = produtoUI;
        _fornecedorUI = fornecedorUI;
        _loteUI = loteUI;
        _vendaUI = vendaUI;
    }

    
    public bool ValidarLogin()
    {
        const int maxTentativas = 3;

        for (int tentativa = 1; tentativa <= maxTentativas; tentativa++)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  PHARMASTOCK");
            Console.WriteLine("  Gestao de farmacia");
            Console.WriteLine("  ================================\n");
            Console.ResetColor();

            if (tentativa > 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  Tentativa {tentativa} de {maxTentativas}.\n");
                Console.ResetColor();
            }

            Console.Write("  Login: ");
            string login = Console.ReadLine() ?? "";

            Console.Write("  Senha: ");
            string senha = Console.ReadLine() ?? "";

            if (login == "admin" && senha == "123")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n  Acesso autorizado.");
                Console.ResetColor();
                Thread.Sleep(1200);
                Console.Clear();
                return true;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n  Usuario ou senha invalidos.");
            int restantes = maxTentativas - tentativa;
            if (restantes > 0)
                Console.WriteLine($"  Restam {restantes} tentativa(s).");
            Console.ResetColor();

            if (tentativa < maxTentativas)
            {
                Console.WriteLine("\n  Pressione ENTER para tentar novamente...");
                Console.ReadLine();
            }
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n  Limite de 3 tentativas excedido. Acesso negado.");
        Console.ResetColor();
        Console.WriteLine("\n  Pressione ENTER para sair...");
        Console.ReadLine();
        return false;
    }

    public void MostrarMenu()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n  MENU PRINCIPAL");
        Console.WriteLine("  ================================\n");
        Console.ResetColor();

        Console.WriteLine("  [1] Controle de Produto");
        Console.WriteLine("  [2] Controle de Funcionario");
        Console.WriteLine("  [3] Controle de Venda");
        Console.WriteLine("  [4] Controle de Fornecedor");
        Console.WriteLine("  [5] Controle de Lote");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  [7] Sair");
        Console.ResetColor();

        Console.Write("\n  Opcao: ");
    }

    public void ProcessarMenu()
    {
        bool continuar = true;
        while (continuar)
        {
            MostrarMenu();
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                _produtoUI.ProcessarMenuProduto();
                break;

                case "2":
                _funcionarioUI.ProcessarMenuFuncionario();
                break;

                case "3":
                _vendaUI.ProcessarMenuVenda();
                break;

                case "4":
                _fornecedorUI.ProcessarMenuFornecedor();
                break;

                case "5":
                _loteUI.ProcessarMenuLote();
                break;

                case "7":
                continuar = false;
                MostrarMensagemEncerramento();
                break;

                default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  Opcao invalida.");
                Console.ResetColor();
                Console.WriteLine("  Pressione ENTER para continuar...");
                Console.ReadLine();
                break;

            }
        }
    }

    public void MostrarMensagemEncerramento()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n  Sistema encerrado. Ate logo.");
        Console.ResetColor();
        Thread.Sleep(1000);
    }




}
