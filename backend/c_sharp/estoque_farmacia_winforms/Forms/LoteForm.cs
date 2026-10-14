using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using estoque_farmacia.Models;
using estoque_farmacia.Services;

namespace estoque_farmacia_winforms.Forms;

public class LoteForm : Form
{
    private readonly LoteService _service;

    private readonly TextBox _txtIdProduto = new();
    private readonly TextBox _txtNumeroLote = new();
    private readonly DateTimePicker _dtpValidade = new();
    private readonly TextBox _txtQuantidade = new();
    private readonly DataGridView _grid = new();

    public LoteForm(LoteService service)
    {
        _service = service;
        ConfigurarJanela();
        ConstruirTela();
        CarregarDados();
    }

    private void ConfigurarJanela()
    {
        Text = "Cadastro de Lotes";
        Size = new Size(950, 560);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UIHelper.CorFundo;
        Font = new Font("Segoe UI", 9F);
    }

    private void ConstruirTela()
    {
        var topo = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = UIHelper.CorPrimaria };
        var lbl = new Label
        {
            Text = "Lotes",
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
            Size = new Size(320, 440),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        int y = 20;
        AdicionarCampo(painelForm, "ID do produto", _txtIdProduto, ref y);
        AdicionarCampo(painelForm, "Numero do lote (inteiro)", _txtNumeroLote, ref y);

        var lblVal = new Label
        {
            Text = "Validade",
            Location = new Point(20, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = UIHelper.CorTexto
        };
        painelForm.Controls.Add(lblVal);
        y += 22;

        _dtpValidade.Location = new Point(20, y);
        _dtpValidade.Size = new Size(280, 26);
        _dtpValidade.Format = DateTimePickerFormat.Short;
        painelForm.Controls.Add(_dtpValidade);
        y += 40;

        AdicionarCampo(painelForm, "Quantidade (ex.: 10 ou 10,5)", _txtQuantidade, ref y);

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
            Size = new Size(560, 440),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        _grid.Location = new Point(10, 10);
        _grid.Size = new Size(540, 375);
        _grid.AllowUserToAddRows = false;
        _grid.ReadOnly = true;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.BackgroundColor = Color.White;
        _grid.RowHeadersVisible = false;

        var btnRemover = new Button { Text = "Remover selecionado", Location = new Point(10, 390), Size = new Size(200, 35) };
        UIHelper.EstilizarBotaoPerigo(btnRemover);
        btnRemover.Click += BotaoRemover_Click;

        var btnAtualizar = new Button { Text = "Atualizar lista", Location = new Point(220, 390), Size = new Size(150, 35) };
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
        _grid.Columns.Add("IdProduto", "ID Produto");
        _grid.Columns.Add("NumeroLote", "N. lote");
        _grid.Columns.Add("Validade", "Validade");
        _grid.Columns.Add("Quantidade", "Qtd");

        foreach (var l in lista)
        {
            _grid.Rows.Add(
                l.Id,
                l.IdProduto,
                l.NumeroLote,
                l.Validade.ToString("dd/MM/yyyy"),
                l.Quantidade.ToString("F2", CultureInfo.GetCultureInfo("pt-BR")));
        }
    }

    private void BotaoSalvar_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(_txtIdProduto.Text.Trim(), out int idProduto) || idProduto <= 0)
        {
            MessageBox.Show("Informe um ID de produto valido (inteiro maior que zero).", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!int.TryParse(_txtNumeroLote.Text.Trim(), out int numeroLote) || numeroLote <= 0)
        {
            MessageBox.Show("Informe o numero do lote (inteiro maior que zero).", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cultura = CultureInfo.GetCultureInfo("pt-BR");
        if (!decimal.TryParse(_txtQuantidade.Text.Trim(), cultura, out decimal quantidade) || quantidade <= 0)
        {
            MessageBox.Show("Informe a quantidade (maior que zero; use virgula para decimais).", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var novo = new Lote
        {
            IdProduto = idProduto,
            NumeroLote = numeroLote,
            Validade = _dtpValidade.Value.Date,
            Quantidade = decimal.Round(quantidade, 2)
        };

        if (_service.Salvar(novo))
        {
            MessageBox.Show("Lote salvo.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparCampos();
            CarregarDados();
        }
        else
        {
            var motivo = string.IsNullOrEmpty(_service.UltimoErro) ? "motivo desconhecido" : _service.UltimoErro;
            MessageBox.Show("Nao foi possivel salvar.\n\n" + motivo, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private DataGridViewRow? ObterLinhaSelecionada()
    {
        if (_grid.SelectedRows.Count > 0)
            return _grid.SelectedRows[0];

        var atual = _grid.CurrentRow;
        if (atual != null && !atual.IsNewRow)
            return atual;

        return null;
    }

    private void BotaoRemover_Click(object? sender, EventArgs e)
    {
        var linha = ObterLinhaSelecionada();
        if (linha == null)
        {
            MessageBox.Show("Selecione um lote (clique na linha na tabela).", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var idCelula = linha.Cells["Id"].Value;
        if (idCelula == null || !int.TryParse(idCelula.ToString(), out int id) || id <= 0)
        {
            MessageBox.Show("Nao foi possivel identificar o ID do lote.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"Remover lote ID {id}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        if (_service.Remover(id))
        {
            CarregarDados();
        }
        else
        {
            var motivo = string.IsNullOrEmpty(_service.UltimoErro) ? "Lote nao encontrado ou erro ao remover." : _service.UltimoErro;
            MessageBox.Show("Nao foi possivel remover.\n\n" + motivo, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LimparCampos()
    {
        _txtIdProduto.Clear();
        _txtNumeroLote.Clear();
        _dtpValidade.Value = DateTime.Today;
        _txtQuantidade.Clear();
        _txtIdProduto.Focus();
    }
}
