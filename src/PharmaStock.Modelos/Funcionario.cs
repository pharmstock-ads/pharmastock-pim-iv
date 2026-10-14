namespace PharmaStock.Modelos;

/// <summary>Colaborador do sistema (login e operações de estoque/venda).</summary>
public class Funcionario
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public string? Cargo { get; set; }

    public string? Email { get; set; }

    /// <summary>Hash SHA-256 (hex) da senha — nunca armazenar senha em texto puro.</summary>
    public string SenhaHash { get; set; } = string.Empty;

    public DateTime DataAdmissao { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime? DataDemissao { get; set; }
}
