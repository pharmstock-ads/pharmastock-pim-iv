using System.Drawing;
using System.Windows.Forms;
using estoque_farmacia.Models;
using estoque_farmacia.Services;

namespace estoque_farmacia_winforms.Forms;

public class FornecedorForm : Form
{
    private readonly FornecedorService _service;

    private readonly TextBox _txtNome = new();
    private readonly TextBox _txtCnpj = new();
    private readonly TextBox _txtTelefone = new();
    private readonly DataGridView _grid = new();

    public FornecedorForm(FornecedorService service)
    {
        _service = service;
        ConfigurarJanela();
        ConstruirTela();
        CarregarDados();
    }

    private void ConfigurarJanela()
    {
        Text = "Cadastro de Fornecedores";
        Size = new Size(950, 540);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UIHelper.CorFundo;
        Font = new Font("Segoe UI", 9F);
    }

    private void ConstruirTela()
    {
        // Cabecalho azul.
        var topo = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = UIHelper.CorPrimaria };
        var lbl = new Label
        {
            Text = "Fornecedores",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 15)
        };
        topo.Controls.Add(lbl);
        Controls.Add(topo);

        // Painel do formulario.
        var painelForm = new Panel
        {
            Location = new Point(20, 80),
            Size = new Size(320, 420),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        int y = 20;
        AdicionarCampo(painelForm, "Nome da empresa", _txtNome, ref y);
        AdicionarCampo(painelForm, "CNPJ", _txtCnpj, ref y);
        AdicionarCampo(painelForm, "Telefone", _txtTelefone, ref y);

        var btnSalvar = new Button { Text = "Salvar", Location = new Point(20, y), Size = new Size(140, 38) };
        UIHelper.EstilizarBotaoPrimario(btnSalvar);
        btnSalvar.Click += BotaoSalvar_Click;

        var btnLimpar = new Button { Text = "Limpar", Location = new Point(170, y), Size = new Size(130, 38) };
        UIHelper.EstilizarBotaoSecundario(btnLimpar);
        btnLimpar.Click += (_, _) => LimparCampos();

        painelForm.Controls.Add(btnSalvar);
        painelForm.Controls.Add(btnLimpar);
        Controls.Add(painelForm);

        // Painel da grade.
        var painelGrid = new Panel
        {
            Location = new Point(360, 80),
            Size = new Size(560, 420),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        _grid.Location = new Point(10, 10);
        _grid.Size = new Size(540, 360);
        _grid.AllowUserToAddRows = false;
        _grid.ReadOnly = true;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.BackgroundColor = Color.White;
        _grid.RowHeadersVisible = false;

        var btnRemover = new Button { Text = "Remover selecionado", Location = new Point(10, 375), Size = new Size(200, 35) };
        UIHelper.EstilizarBotaoPerigo(btnRemover);
        btnRemover.Click += BotaoRemover_Click;

        var btnAtualizar = new Button { Text = "Atualizar lista", Location = new Point(220, 375), Size = new Size(150, 35) };
        UIHelper.EstilizarBotaoSecundario(btnAtualizar);
        btnAtualizar.Click += (_, _) => CarregarDados();

        painelGrid.Controls.Add(_grid);
        painelGrid.Controls.Add(btnRemover);
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
        _grid.Columns.Add("Nome", "Empresa");
        _grid.Columns.Add("Cnpj", "CNPJ");
        _grid.Columns.Add("Telefone", "Telefone");

        foreach (var f in lista)
        {
            _grid.Rows.Add(f.Id, f.NomeEmpresa, f.Cnpj, f.Telefone);
        }
    }

    private void BotaoSalvar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNome.Text))
        {
            MessageBox.Show("Informe o nome da empresa.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var novo = new Fornecedor
        {
            NomeEmpresa = _txtNome.Text.Trim(),
            Cnpj = _txtCnpj.Text.Trim(),
            Telefone = _txtTelefone.Text.Trim()
        };

        if (_service.Salvar(novo))
        {
            MessageBox.Show("Fornecedor salvo.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparCampos();
            CarregarDados();
        }
        else
        {
            var motivo = string.IsNullOrEmpty(_service.UltimoErro) ? "motivo desconhecido" : _service.UltimoErro;
            MessageBox.Show("Nao foi possivel salvar.\n\n" + motivo, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BotaoRemover_Click(object? sender, EventArgs e)
    {
        if (_grid.SelectedRows.Count == 0)
        {
            MessageBox.Show("Selecione um fornecedor.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var id = Convert.ToInt32(_grid.SelectedRows[0].Cells["Id"].Value);
        if (MessageBox.Show($"Remover fornecedor ID {id}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        if (_service.Remover(id))
        {
            CarregarDados();
        }
        else
        {
            var motivo2 = string.IsNullOrEmpty(_service.UltimoErro) ? "motivo desconhecido" : _service.UltimoErro;
            MessageBox.Show("Nao foi possivel remover.\n\n" + motivo2, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LimparCampos()
    {
        _txtNome.Clear();
        _txtCnpj.Clear();
        _txtTelefone.Clear();
        _txtNome.Focus();
    }
}
