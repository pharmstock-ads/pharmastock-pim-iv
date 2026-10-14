namespace PharmaStock.Modelos;

/// <summary>Registro de login de funcionário.</summary>
public class UsuarioLog
{
    public int Id { get; set; }

    public int IdFuncionario { get; set; }

    public DateTime DataLogin { get; set; }
}
