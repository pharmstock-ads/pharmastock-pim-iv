// Entidade funcionario (login e operacoes).
namespace PharmaStock.Modelos;

public class Funcionario
{
    public int IdFuncionario { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public string? Cargo { get; set; }
    public string? Email { get; set; }
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime DataAdmissao { get; set; }
    public char Ativo { get; set; } = 's';

    public bool EstaAtivo => Ativo is 's' or 'S';
}
