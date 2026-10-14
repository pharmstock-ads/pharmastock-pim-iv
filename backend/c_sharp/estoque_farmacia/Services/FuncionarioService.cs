using estoque_farmacia.Models;

namespace estoque_farmacia.Services;

public class FuncionarioService
{
    private readonly List<Funcionario> Itens = new();
    private int _nextId = 1;

    public string UltimoErro { get; private set; } = string.Empty;

    public bool Salvar(Funcionario novoFuncionario)
    {
        try
        {
            if (novoFuncionario == null)
            {
                UltimoErro = "ERRO: Funcionário não pode ser nulo!"; Console.WriteLine(UltimoErro);
                return false;
            }

            if (string.IsNullOrWhiteSpace(novoFuncionario.Nome))
            {
                UltimoErro = "ERRO: Nome do funcionário é obrigatório!"; Console.WriteLine(UltimoErro);
                return false;
            }

            if (string.IsNullOrWhiteSpace(novoFuncionario.CPF))
            {
                UltimoErro = "ERRO: CPF é obrigatório!"; Console.WriteLine(UltimoErro);
                return false;
            }

            if (Itens.Any(f => f.CPF == novoFuncionario.CPF))
            {
                UltimoErro = "ERRO: Já existe funcionário com este CPF!"; Console.WriteLine(UltimoErro);
                return false;
            }

            novoFuncionario.DataAdmissao = DateTime.Now;
            novoFuncionario.Ativo = true;
            novoFuncionario.Id = _nextId++;
            Itens.Add(novoFuncionario);

            Console.WriteLine($"Funcionário '{novoFuncionario.Nome}' salvo com sucesso! ID: {novoFuncionario.Id}");
            return true;
        }
        catch (Exception ex)
        {
            UltimoErro = $"ERRO ao salvar funcionário: {ex.Message}"; Console.WriteLine(UltimoErro);
            return false;
        }
    }

    public List<Funcionario> ListarTodos() => Itens.ToList();

    public List<Funcionario> ListarAtivos() => Itens.Where(f => f.Ativo).ToList();

    public Funcionario? BuscarPorId(int id) => Itens.FirstOrDefault(f => f.Id == id);

    public bool Atualizar(Funcionario funcionarioAtualizado)
    {
        try
        {
            if (funcionarioAtualizado == null || funcionarioAtualizado.Id <= 0)
            {
                UltimoErro = "ERRO: Funcionário ou ID inválido!"; Console.WriteLine(UltimoErro);
                return false;
            }

            var funcionarioExistente = BuscarPorId(funcionarioAtualizado.Id);
            if (funcionarioExistente == null)
            {
                Console.WriteLine($"ERRO: Funcionário com ID {funcionarioAtualizado.Id} não encontrado!");
                return false;
            }

            funcionarioExistente.Nome = funcionarioAtualizado.Nome;
            funcionarioExistente.CPF = funcionarioAtualizado.CPF;
            funcionarioExistente.Cargo = funcionarioAtualizado.Cargo;
            funcionarioExistente.SenhaHash = funcionarioAtualizado.SenhaHash;

            Console.WriteLine($"Funcionário '{funcionarioExistente.Nome}' atualizado com sucesso!");
            return true;
        }
        catch (Exception ex)
        {
            UltimoErro = $"ERRO ao atualizar funcionário: {ex.Message}"; Console.WriteLine(UltimoErro);
            return false;
        }
    }

    public bool Inativar(int id)
    {
        try
        {
            var funcionario = BuscarPorId(id);
            if (funcionario == null)
            {
                UltimoErro = $"ERRO: Funcionário com ID {id} não encontrado!";
                Console.WriteLine(UltimoErro);
                return false;
            }

            funcionario.Ativo = false;
            funcionario.DataDemissao = DateTime.Now;

            Console.WriteLine($"Funcionário '{funcionario.Nome}' inativado com sucesso!");
            return true;
        }
        catch (Exception ex)
        {
            UltimoErro = $"ERRO ao inativar funcionário: {ex.Message}"; Console.WriteLine(UltimoErro);
            return false;
        }
    }

    public bool Remover(int id)
    {
        try
        {
            var f = Itens.FirstOrDefault(x => x.Id == id);
            if (f == null)
            {
                Console.WriteLine($"ERRO: Funcionário com ID {id} não encontrado!");
                return false;
            }

            Itens.Remove(f);
            Console.WriteLine("Funcionário removido completamente do sistema!");
            return true;
        }
        catch (Exception ex)
        {
            UltimoErro = $"ERRO ao remover funcionário: {ex.Message}"; Console.WriteLine(UltimoErro);
            return false;
        }
    }

    public int Contar() => Itens.Count;

    public int ContarAtivos() => Itens.Count(f => f.Ativo);
}
