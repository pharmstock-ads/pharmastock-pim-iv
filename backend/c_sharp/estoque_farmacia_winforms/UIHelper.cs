using System.Drawing;
using System.Windows.Forms;

namespace estoque_farmacia_winforms;

public static class UIHelper
{
    // Paleta de cores em valores hexadecimais (#RRGGBB).
    public static readonly Color CorPrimaria   = ColorTranslator.FromHtml("#0078D7"); // azul forte
    public static readonly Color CorSecundaria = ColorTranslator.FromHtml("#00BFFF"); // azul claro destaque
    public static readonly Color CorFundo      = ColorTranslator.FromHtml("#F5F8FC"); // branco azulado
    public static readonly Color CorTexto      = ColorTranslator.FromHtml("#1A3A52"); // azul escuro para texto
    public static readonly Color CorErro       = ColorTranslator.FromHtml("#D32F2F"); // vermelho para mensagens de erro
    public static readonly Color CorSucesso    = ColorTranslator.FromHtml("#2E7D32"); // verde para confirmacoes

    public static void EstilizarBotaoPrimario(Button botao)
    {
        botao.BackColor = CorPrimaria;
        botao.ForeColor = Color.White;
        botao.FlatStyle = FlatStyle.Flat;
        botao.FlatAppearance.BorderSize = 0;
        botao.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        botao.Cursor = Cursors.Hand;
        botao.Height = 38;
    }

    public static void EstilizarBotaoSecundario(Button botao)
    {
        botao.BackColor = Color.White;
        botao.ForeColor = CorPrimaria;
        botao.FlatStyle = FlatStyle.Flat;
        botao.FlatAppearance.BorderSize = 1;
        botao.FlatAppearance.BorderColor = CorPrimaria;
        botao.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        botao.Cursor = Cursors.Hand;
        botao.Height = 38;
    }

    public static void EstilizarBotaoPerigo(Button botao)
    {
        botao.BackColor = CorErro;
        botao.ForeColor = Color.White;
        botao.FlatStyle = FlatStyle.Flat;
        botao.FlatAppearance.BorderSize = 0;
        botao.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        botao.Cursor = Cursors.Hand;
        botao.Height = 38;
    }

    public static void EstilizarCampo(TextBox campo)
    {
        campo.BorderStyle = BorderStyle.FixedSingle;
        campo.Font = new Font("Segoe UI", 10F);
        campo.Height = 28;
    }

    public static void EstilizarLabel(Label label)
    {
        label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        label.ForeColor = CorTexto;
        label.AutoSize = true;
    }
}
