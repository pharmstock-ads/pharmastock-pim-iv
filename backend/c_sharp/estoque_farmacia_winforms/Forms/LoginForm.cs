using System.Drawing;
using System.Windows.Forms;

namespace estoque_farmacia_winforms.Forms;

public class LoginForm : Form
{
    private const int MaxTentativas = 3;

    private readonly TextBox _txtLogin = new();
    private readonly TextBox _txtSenha = new();
    private readonly Label _lblErro = new();
    private readonly Button _btnEntrar = new();
    private int _tentativasFalhas;

    public LoginForm()
    {
        ConfigurarJanela();
        ConstruirTela();
    }

    private void ConfigurarJanela()
    {
        Text = "Login - Pharmastock";
        Size = new Size(420, 460);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = UIHelper.CorFundo;
        Font = new Font("Segoe UI", 9F);
    }

    private void ConstruirTela()
    {
        var topo = new Panel
        {
            Dock = DockStyle.Top,
            Height = 90,
            BackColor = UIHelper.CorPrimaria
        };

        var lblTitulo = new Label
        {
            Text = "Pharmastock",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(30, 20)
        };
        var lblSubtitulo = new Label
        {
            Text = "Gestao de farmacia",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10F),
            AutoSize = true,
            Location = new Point(32, 55)
        };
        topo.Controls.Add(lblTitulo);
        topo.Controls.Add(lblSubtitulo);

        var lblLogin = new Label { Text = "Usuario", Location = new Point(40, 130) };
        UIHelper.EstilizarLabel(lblLogin);

        _txtLogin.Location = new Point(40, 155);
        _txtLogin.Size = new Size(330, 28);
        UIHelper.EstilizarCampo(_txtLogin);

        var lblSenha = new Label { Text = "Senha", Location = new Point(40, 200) };
        UIHelper.EstilizarLabel(lblSenha);

        _txtSenha.Location = new Point(40, 225);
        _txtSenha.Size = new Size(330, 28);
        _txtSenha.UseSystemPasswordChar = true;
        UIHelper.EstilizarCampo(_txtSenha);

        _lblErro.Text = "Usuario ou senha invalidos.";
        _lblErro.ForeColor = UIHelper.CorErro;
        _lblErro.Font = new Font("Segoe UI", 9F);
        _lblErro.Location = new Point(40, 265);
        _lblErro.AutoSize = true;
        _lblErro.Visible = false;

        _btnEntrar.Text = "Entrar";
        _btnEntrar.Location = new Point(40, 300);
        _btnEntrar.Size = new Size(330, 42);
        UIHelper.EstilizarBotaoPrimario(_btnEntrar);
        _btnEntrar.Click += BotaoEntrar_Click;

        AcceptButton = _btnEntrar;

        Controls.Add(topo);
        Controls.Add(lblLogin);
        Controls.Add(_txtLogin);
        Controls.Add(lblSenha);
        Controls.Add(_txtSenha);
        Controls.Add(_lblErro);
        Controls.Add(_btnEntrar);
    }

    private void BotaoEntrar_Click(object? sender, EventArgs e)
    {
        var login = _txtLogin.Text.Trim();
        var senha = _txtSenha.Text;

        if (login == "admin" && senha == "123")
        {
            var menu = new MenuForm();
            menu.Show();
            Hide();
            menu.FormClosed += (_, _) => Application.Exit();
            return;
        }

        _tentativasFalhas++;
        _lblErro.Visible = true;

        if (_tentativasFalhas >= MaxTentativas)
        {
            _lblErro.Text = "Limite de 3 tentativas excedido.";
            _btnEntrar.Enabled = false;
            _txtLogin.Enabled = false;
            _txtSenha.Enabled = false;
            AcceptButton = null;
            MessageBox.Show(
                "Acesso bloqueado apos 3 tentativas incorretas.",
                "Pharmastock",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            Application.Exit();
            return;
        }

        int restantes = MaxTentativas - _tentativasFalhas;
        _lblErro.Text = $"Usuario ou senha invalidos. Restam {restantes} tentativa(s).";
        _txtSenha.Clear();
        _txtLogin.Focus();
    }
}
