namespace PharmaStock.Modelos;

/// <summary>Categoria de produto (ex.: Analgésico, Antibiótico).</summary>
public class Categoria
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;
}
