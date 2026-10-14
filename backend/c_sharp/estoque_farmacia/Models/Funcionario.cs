using System;
namespace estoque_farmacia.Models;

public class Funcionario
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string CPF { get; set; }

    public string Cargo { get; set; }

    public string SenhaHash { get; set; }

    public DateTime DataAdmissao { get; set; }

    public bool Ativo { get; set; }

    public DateTime? DataDemissao { get; set; }

}
