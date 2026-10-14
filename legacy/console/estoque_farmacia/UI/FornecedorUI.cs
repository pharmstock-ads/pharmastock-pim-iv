using estoque_farmacia.Models;
using estoque_farmacia.Services;
namespace estoque_farmacia.UI;


public class FornecedorUI
{
    private readonly FornecedorService _fornecedorService;

    public FornecedorUI(FornecedorService fornecedorService)
    {
        _fornecedorService = fornecedorService;
    }


    public void MostrarMenuFornecedor() {
        Console.WriteLine("\n");
        Console.Write("===CONTROLE DE FORNECEDOR===\n");
        Thread.Sleep(1500);
        Console.Write("\n");
        Console.Write("Escolha uma opção: \n");
        Console.Write("1 - Cadastrar fornecedor\n");
        Console.Write("2 - Listar fornecedores\n");
        Console.Write("3 - Remover fornecedor\n");
        Console.Write("4 - Voltar\n");
        Console.Write(">>> ");
        Console.Write("\n");
        }

    public void ProcessarMenuFornecedor()
    {
        bool continuar = true;
        while (continuar)
        {
            MostrarMenuFornecedor();
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1": 
                ObterDadosFornecedor();  
                break;

                case "2":
                Console.Clear();
                Console.WriteLine("===LISTA DE FORNECEDORES===");
                Console.WriteLine("\n");
                var lista = _fornecedorService.ListarTodos();
                if (lista.Count == 0)
                    {
                        Console.WriteLine("Nenhum fornecedor cadastrado.");
                        Console.WriteLine("\n");
                        Thread.Sleep(3500);
                    }
                    else
                    {
                        for(int i = 0; i < lista.Count; i++)
                        {
                            Console.WriteLine($"ID: {i+1} | Empresa: {lista[i].NomeEmpresa} | CNPJ: {lista[i].Cnpj} | Tel: {lista[i].Telefone}");
                        }
                    }

                break;


                case "3":
                RemoverFornecedor();
                break;

                case "4":
                continuar = false;
                Console.WriteLine("Voltando ao menu principal.");
                break;


                default:
                Console.WriteLine("Opção inválida.");
                Thread.Sleep(1000); // da um sleep no terminal
                break;

            }


        }

    }






    public void ObterDadosFornecedor()
    {
        Console.Write("\n");
        Console.Write("Digite o nome da empresa: ");
        string nome= Console.ReadLine();
        Console.Write("Digite o cnpj da empresa: ");
        string cnpj = Console.ReadLine();
        Console.Write("Digite o telefone da empresa: ");
        string telefone = Console.ReadLine();

        Fornecedor novo = new Fornecedor
        {
            NomeEmpresa = nome,
            Cnpj = cnpj,
            Telefone = telefone
        };

        //_ indica um campo privado
        _fornecedorService.Salvar(novo);
        Console.WriteLine("\n");
        Console.WriteLine("\nCadastro realizado com sucesso!");

    }


    public void RemoverFornecedor()
    {
        Console.Write("Digite o ID do fornecedor a ser removido: ");
        
        if (int.TryParse(Console.ReadLine(), out int idDigitado))
        {
            int indice = idDigitado - 1; //ajusta o indice
            if (_fornecedorService.Remover(indice) == true) {
            Console.WriteLine("Fornecedor removido com sucesso!");}

            else
            {
                Console.WriteLine("Fornecedor não encontrado.");
            }

        }

        else
        {   Console.Write("\n");
            Console.WriteLine("Digite apenas valores numéricos.");
        }



    }


}
