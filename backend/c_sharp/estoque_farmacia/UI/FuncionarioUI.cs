using estoque_farmacia.Models;
using estoque_farmacia.Services;

namespace estoque_farmacia.UI;

public class FuncionarioUI
{
    private readonly FuncionarioService _funcionarioService;

    public FuncionarioUI(FuncionarioService funcionarioService)
    {
        _funcionarioService = funcionarioService;
    }

    public void MostrarMenuFuncionario()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n  CONTROLE DE FUNCIONARIOS");
        Console.WriteLine("  ================================\n");
        Console.ResetColor();

        Console.WriteLine("  [1] Cadastrar novo funcionario");
        Console.WriteLine("  [2] Listar todos os funcionarios");
        Console.WriteLine("  [3] Buscar funcionario por ID");
        Console.WriteLine("  [4] Atualizar funcionario");
        Console.WriteLine("  [5] Inativar funcionario");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  [6] Voltar");
        Console.ResetColor();

        Console.Write("\n  Opcao: ");
    }

    public void ProcessarMenuFuncionario()
    {
        bool continuar = true;

        while (continuar)
        {
            MostrarMenuFuncionario();
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    CadastrarFuncionario();
                    break;

                case "2":
                    ListarTodosFuncionarios();
                    break;

                case "3":
                    BuscarFuncionarioPorId();
                    break;

                case "4":
                    AtualizarFuncionario();
                    break;

                case "5":
                    InativarFuncionario();
                    break;

                case "6":
                    continuar = false;
                    Console.WriteLine("\n  Voltando ao menu principal.");
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

    private void CadastrarFuncionario()
    {
        Console.Clear();
        Console.WriteLine("\n  === CADASTRAR FUNCIONARIO ===\n");

        try
        {
            Console.Write("  Nome completo: ");
            string nome = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("\n  Nome nao pode ser vazio!");
                Pausar();
                return;
            }

            Console.Write("  CPF (formato: 000.000.000-00): ");
            string cpf = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cpf))
            {
                Console.WriteLine("\n  CPF nao pode ser vazio!");
                Pausar();
                return;
            }

            Console.Write("  Cargo (Gerente/Farmaceutico/Atendente/Estoquista): ");
            string cargo = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cargo))
            {
                Console.WriteLine("\n  Cargo nao pode ser vazio!");
                Pausar();
                return;
            }

            Console.Write("  Senha: ");
            string senha = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(senha))
            {
                Console.WriteLine("\n  Senha nao pode ser vazia!");
                Pausar();
                return;
            }

            Funcionario novoFunc = new Funcionario
            {
                Nome = nome,
                CPF = cpf,
                Cargo = cargo,
                SenhaHash = SenhaHasher.HashSha256(senha)
            };

            bool sucesso = _funcionarioService.Salvar(novoFunc);

            if (!sucesso)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  Erro ao cadastrar funcionario!");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  Erro: {ex.Message}");
            Console.ResetColor();
        }

        Pausar();
    }

    private void ListarTodosFuncionarios()
    {
        Console.Clear();
        Console.WriteLine("\n  === LISTA DE FUNCIONARIOS ===\n");

        try
        {
            var funcionarios = _funcionarioService.ListarTodos();

            if (funcionarios.Count == 0)
            {
                Console.WriteLine("  Nenhum funcionario cadastrado.");
            }
            else
            {
                Console.WriteLine($"  {"ID",-5} {"Nome",-25} {"CPF",-15} {"Cargo",-15} {"Status",-10}");
                Console.WriteLine("  " + new string('-', 70));

                foreach (var func in funcionarios)
                {
                    string status = func.Ativo ? "Ativo" : "Inativo";
                    Console.WriteLine($"  {func.Id,-5} {func.Nome,-25} {func.CPF,-15} {func.Cargo,-15} {status,-10}");
                }

                Console.WriteLine("  " + new string('-', 70));
                int total = _funcionarioService.Contar();
                int ativos = _funcionarioService.ContarAtivos();
                Console.WriteLine($"\n  Total: {total} | Ativos: {ativos} | Inativos: {total - ativos}");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  Erro: {ex.Message}");
            Console.ResetColor();
        }

        Pausar();
    }

    private void BuscarFuncionarioPorId()
    {
        Console.Clear();
        Console.WriteLine("\n  === BUSCAR FUNCIONARIO POR ID ===\n");

        try
        {
            Console.Write("  ID do funcionario: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n  ID invalido!");
                Pausar();
                return;
            }

            var func = _funcionarioService.BuscarPorId(id);

            if (func != null)
            {
                Console.WriteLine($"\n  ID: {func.Id}");
                Console.WriteLine($"  Nome: {func.Nome}");
                Console.WriteLine($"  CPF: {func.CPF}");
                Console.WriteLine($"  Cargo: {func.Cargo}");
                Console.WriteLine($"  Admissao: {func.DataAdmissao:dd/MM/yyyy}");
                Console.WriteLine($"  Status: {(func.Ativo ? "Ativo" : "Inativo")}");

                if (func.DataDemissao.HasValue)
                    Console.WriteLine($"  Demissao: {func.DataDemissao?.ToString("dd/MM/yyyy")}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  Funcionario com ID {id} nao encontrado!");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  Erro: {ex.Message}");
            Console.ResetColor();
        }

        Pausar();
    }

    private void AtualizarFuncionario()
    {
        Console.Clear();
        Console.WriteLine("\n  === ATUALIZAR FUNCIONARIO ===\n");

        try
        {
            Console.Write("  ID do funcionario a atualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n  ID invalido!");
                Pausar();
                return;
            }

            var funcExistente = _funcionarioService.BuscarPorId(id);
            if (funcExistente == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  Funcionario com ID {id} nao encontrado!");
                Console.ResetColor();
                Pausar();
                return;
            }

            Console.WriteLine($"\n  Dados atuais: {funcExistente.Nome} | {funcExistente.CPF} | {funcExistente.Cargo}");
            Console.WriteLine("  (deixe em branco para manter)\n");

            Console.Write("  Novo nome: ");
            string novoNome = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoNome))
                funcExistente.Nome = novoNome;

            Console.Write("  Novo CPF: ");
            string novoCpf = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoCpf))
                funcExistente.CPF = novoCpf;

            Console.Write("  Novo cargo: ");
            string novoCargo = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoCargo))
                funcExistente.Cargo = novoCargo;

            bool sucesso = _funcionarioService.Atualizar(funcExistente);
            if (!sucesso)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  Erro ao atualizar!");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  Erro: {ex.Message}");
            Console.ResetColor();
        }

        Pausar();
    }

    private void InativarFuncionario()
    {
        Console.Clear();
        Console.WriteLine("\n  === INATIVAR FUNCIONARIO ===\n");

        try
        {
            Console.Write("  ID do funcionario a inativar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n  ID invalido!");
                Pausar();
                return;
            }

            var func = _funcionarioService.BuscarPorId(id);
            if (func == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  Funcionario com ID {id} nao encontrado!");
                Console.ResetColor();
                Pausar();
                return;
            }

            Console.Write($"\n  Deseja inativar '{func.Nome}'? (S/N): ");
            string confirmacao = (Console.ReadLine() ?? "").Trim().ToUpper();

            if (confirmacao != "S")
            {
                Console.WriteLine("\n  Operacao cancelada.");
                Pausar();
                return;
            }

            bool sucesso = _funcionarioService.Inativar(id);
            if (!sucesso)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  Erro ao inativar!");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  Erro: {ex.Message}");
            Console.ResetColor();
        }

        Pausar();
    }

    private static void Pausar()
    {
        Console.WriteLine("\n  Pressione ENTER para continuar...");
        Console.ReadLine();
    }
}
