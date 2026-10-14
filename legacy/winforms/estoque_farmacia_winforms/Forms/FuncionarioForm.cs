using System.Drawing;
using System.Windows.Forms;
using estoque_farmacia.Models;
using estoque_farmacia.Services;

namespace estoque_farmacia_winforms.Forms;

public class FuncionarioForm : Form
{
    private readonly FuncionarioService _service;

    private readonly TextBox _txtNome = new();
    private readonly TextBox _txtCpf = new();
    private readonly TextBox _txtCargo = new();
    private readonly TextBox _txtSenha = new();
    private readonly DataGridView _grid = new();

    public FuncionarioForm(FuncionarioService service)
    {
        _service = service;
        ConfigurarJanela();
        ConstruirTela();
        CarregarDados();
    }

    private void ConfigurarJanela()
    {
        Text = "Cadastro de Funcionarios";
        Size = new Size(950, 580);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UIHelper.CorFundo;
        Font = new Font("Segoe UI", 9F);
    }

    private void ConstruirTela()
    {
        var topo = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = UIHelper.CorPrimaria };
        var lbl = new Label
        {
            Text = "Funcionarios",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 15)
        };
        topo.Controls.Add(lbl);
        Controls.Add(topo);

        var painelForm = new Panel
        {
            Location = new Point(20, 80),
            Size = new Size(320, 460),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        int y = 20;
        AdicionarCampo(painelForm, "Nome completo", _txtNome, ref y);
        AdicionarCampo(painelForm, "CPF (formato 000.000.000-00)", _txtCpf, ref y);
        AdicionarCampo(painelForm, "Cargo", _txtCargo, ref y);

        var lblSenha = new Label
        {
            Text = "Senha",
            Location = new Point(20, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = UIHelper.CorTexto
        };
        painelForm.Controls.Add(lblSenha);
        y += 22;

        _txtSenha.Location = new Point(20, y);
        _txtSenha.Size = new Size(280, 26);
        _txtSenha.UseSystemPasswordChar = true; // esconde os caracteres
        UIHelper.EstilizarCampo(_txtSenha);
        painelForm.Controls.Add(_txtSenha);
        y += 50;

        var btnSalvar = new Button { Text = "Salvar", Location = new Point(20, y), Size = new Size(140, 38) };
        UIHelper.EstilizarBotaoPrimario(btnSalvar);
        btnSalvar.Click += BotaoSalvar_Click;

        var btnLimpar = new Button { Text = "Limpar", Location = new Point(170, y), Size = new Size(130, 38) };
        UIHelper.EstilizarBotaoSecundario(btnLimpar);
        btnLimpar.Click += (_, _) => LimparCampos();

        painelForm.Controls.Add(btnSalvar);
        painelForm.Controls.Add(btnLimpar);
        Controls.Add(painelForm);

        var painelGrid = new Panel
        {
            Location = new Point(360, 80),
            Size = new Size(560, 460),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        _grid.Location = new Point(10, 10);
        _grid.Size = new Size(540, 400);
        _grid.AllowUserToAddRows = false;
        _grid.ReadOnly = true;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.BackgroundColor = Color.White;
        _grid.RowHeadersVisible = false;

        var btnInativar = new Button { Text = "Inativar selecionado", Location = new Point(10, 415), Size = new Size(200, 35) };
        UIHelper.EstilizarBotaoPerigo(btnInativar);
        btnInativar.Click += BotaoInativar_Click;

        var btnAtualizar = new Button { Text = "Atualizar lista", Location = new Point(220, 415), Size = new Size(150, 35) };
        UIHelper.EstilizarBotaoSecundario(btnAtualizar);
        btnAtualizar.Click += (_, _) => CarregarDados();

        painelGrid.Controls.Add(_grid);
        painelGrid.Controls.Add(btnInativar);
        painelGrid.Controls.Add(btnAtualizar);
        Controls.Add(painelGrid);
    }

    private static void AdicionarCampo(Control painel, string textoLabel, TextBox campo, ref int y)
    {
        var label = new Label
        {
            Text = textoLabel,
            Location = new Point(20, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = UIHelper.CorTexto
        };
        painel.Controls.Add(label);
        y += 22;

        campo.Location = new Point(20, y);
        campo.Size = new Size(280, 26);
        UIHelper.EstilizarCampo(campo);
        painel.Controls.Add(campo);
        y += 40;
    }

    private void CarregarDados()
    {
        var lista = _service.ListarTodos();

        _grid.Columns.Clear();
        _grid.Rows.Clear();
        _grid.Columns.Add("Id", "ID");
        _grid.Columns.Add("Nome", "Nome");
        _grid.Columns.Add("Cpf", "CPF");
        _grid.Columns.Add("Cargo", "Cargo");
        _grid.Columns.Add("Ativo", "Ativo");
        _grid.Columns.Add("Admissao", "Admissao");

        foreach (var f in lista)
        {
            _grid.Rows.Add(
                f.Id,
                f.Nome,
                f.CPF,
                f.Cargo,
                f.Ativo ? "Sim" : "Nao",
                f.DataAdmissao.ToString("dd/MM/yyyy"));
        }
    }

    private void BotaoSalvar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNome.Text))
        {
            MessageBox.Show("Informe o nome.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(_txtCpf.Text))
        {
            MessageBox.Show("Informe o CPF.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(_txtCargo.Text))
        {
            MessageBox.Show("Informe o cargo.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(_txtSenha.Text) || _txtSenha.Text.Length < 4)
        {
            MessageBox.Show("Senha deve ter ao menos 4 caracteres.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var novo = new Funcionario
        {
            Nome = _txtNome.Text.Trim(),
            CPF = _txtCpf.Text.Trim(),
            Cargo = _txtCargo.Text.Trim(),
            SenhaHash = SenhaHasher.HashSha256(_txtSenha.Text),
            DataAdmissao = DateTime.Now,
            Ativo = true
        };

        if (_service.Salvar(novo))
        {
            MessageBox.Show("Funcionario salvo.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparCampos();
            CarregarDados();
        }
        else
        {
            // Mostra o motivo real exposto pelo service.
            var motivo = string.IsNullOrEmpty(_service.UltimoErro) ? "motivo desconhecido" : _service.UltimoErro;
            MessageBox.Show("Nao foi possivel salvar.\n\n" + motivo, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // SelectedRows pode ficar vazio apos recarregar; CurrentRow costuma indicar a linha em foco.
    private DataGridViewRow? ObterLinhaFuncionarioParaAcao()
    {
        if (_grid.SelectedRows.Count > 0)
            return _grid.SelectedRows[0];

        var atual = _grid.CurrentRow;
        if (atual != null && !atual.IsNewRow)
            return atual;

        return null;
    }

    private void BotaoInativar_Click(object? sender, EventArgs e)
    {
        var linha = ObterLinhaFuncionarioParaAcao();
        if (linha == null)
        {
            MessageBox.Show("Selecione um funcionario (clique na linha na tabela).", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var idCelula = linha.Cells["Id"].Value;
        if (idCelula == null || !int.TryParse(idCelula.ToString(), out int id) || id <= 0)
        {
            MessageBox.Show("Nao foi possivel identificar o ID do funcionario.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (MessageBox.Show($"Inativar funcionario ID {id}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        if (_service.Inativar(id))
        {
            CarregarDados();
        }
        else
        {
            var motivo = string.IsNullOrEmpty(_service.UltimoErro) ? "motivo desconhecido" : _service.UltimoErro;
            MessageBox.Show("Nao foi possivel inativar.\n\n" + motivo, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LimparCampos()
    {
        _txtNome.Clear();
        _txtCpf.Clear();
        _txtCargo.Clear();
        _txtSenha.Clear();
        _txtNome.Focus();
    }
}
