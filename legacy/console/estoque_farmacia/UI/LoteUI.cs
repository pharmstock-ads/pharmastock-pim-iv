using System.Globalization;
using estoque_farmacia.Models;
using estoque_farmacia.Services;

namespace estoque_farmacia.UI;

public class LoteUI
{
    private readonly LoteService _loteService;

    public LoteUI(LoteService loteService)
    {
        _loteService = loteService;
    }

    public void MostrarMenuLote()
    {
        Console.WriteLine("\n");
        Console.Write("===CONTROLE DE LOTE===\n");
        Thread.Sleep(800);
        Console.Write("\n");
        Console.Write("Escolha uma opcao: \n");
        Console.Write("1 - Cadastrar lote\n");
        Console.Write("2 - Listar lotes\n");
        Console.Write("3 - Remover lote\n");
        Console.Write("4 - Voltar\n");
        Console.Write(">>> ");
        Console.Write("\n");
    }

    public void ProcessarMenuLote()
    {
        bool continuar = true;
        while (continuar)
        {
            MostrarMenuLote();
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    CadastrarLote();
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("===LISTA DE LOTES===");
                    Console.WriteLine();
                    var lista = _loteService.ListarTodos();
                    if (lista.Count == 0)
                    {
                        Console.WriteLine("Nenhum lote cadastrado.");
                        Console.WriteLine();
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        foreach (var l in lista)
                        {
                            Console.WriteLine($"  ID: {l.Id} | Produto: {l.IdProduto} | Lote: {l.NumeroLote} | Validade: {l.Validade:dd/MM/yyyy} | Qtd: {l.Quantidade}");
                        }

                        Console.WriteLine("\n  Pressione ENTER para continuar...");
                        Console.ReadLine();
                    }

                    break;

                case "3":
                    RemoverLote();
                    break;

                case "4":
                    continuar = false;
                    Console.WriteLine("Voltando ao menu principal.");
                    break;

                default:
                    Console.WriteLine("Opcao invalida.");
                    Thread.Sleep(800);
                    break;
            }
        }
    }

    public void CadastrarLote()
    {
        Console.WriteLine();

        Console.Write("ID do produto: ");
        int idProduto;
        while (!int.TryParse(Console.ReadLine(), out idProduto) || idProduto <= 0)
        {
            Console.Write("  Digite um ID valido: ");
        }

        Console.Write("Numero do lote (inteiro): ");
        int numeroLote;
        while (!int.TryParse(Console.ReadLine(), out numeroLote) || numeroLote <= 0)
        {
            Console.Write("  Digite um numero maior que zero: ");
        }

        Console.Write("Validade (dd/mm/aaaa): ");
        DateTime validade;
        var cultura = CultureInfo.GetCultureInfo("pt-BR");
        while (!DateTime.TryParse(Console.ReadLine(), cultura, DateTimeStyles.None, out validade))
        {
            Console.Write("  Data invalida. Use dd/mm/aaaa: ");
        }

        Console.Write("Quantidade: ");
        decimal quantidade;
        while (!decimal.TryParse(Console.ReadLine(), cultura, out quantidade) || quantidade <= 0)
        {
            Console.Write("  Digite uma quantidade maior que zero (use virgula se precisar): ");
        }

        var novo = new Lote
        {
            IdProduto = idProduto,
            NumeroLote = numeroLote,
            Validade = validade.Date,
            Quantidade = decimal.Round(quantidade, 2)
        };

        _loteService.Salvar(novo);

        Console.WriteLine("  Pressione ENTER para continuar...");
        Console.ReadLine();
    }

    public void RemoverLote()
    {
        Console.Write("\nID do lote a remover: ");

        if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
        {
            if (_loteService.Remover(id))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  Lote removido com sucesso.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Lote nao encontrado.");
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
