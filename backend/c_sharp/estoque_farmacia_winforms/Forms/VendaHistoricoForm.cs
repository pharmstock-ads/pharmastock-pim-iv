using System.Drawing;
using System.Windows.Forms;
using estoque_farmacia.Models;
using estoque_farmacia.Services;

namespace estoque_farmacia_winforms.Forms;

public class VendaHistoricoForm : Form
{
    private readonly VendaService _service;

    private readonly DataGridView _gridVendas = new();
    private readonly DataGridView _gridItens = new();

    public VendaHistoricoForm(VendaService service)
    {
        _service = service;
        ConfigurarJanela();
        ConstruirTela();
        CarregarDados();
    }

    private void ConfigurarJanela()
    {
        Text = "Historico de Vendas";
        Size = new Size(960, 580);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UIHelper.CorFundo;
        Font = new Font("Segoe UI", 9F);
    }

    private void ConstruirTela()
    {
        var topo = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = UIHelper.CorPrimaria };
        var lbl = new Label
        {
            Text = "Vendas realizadas",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 15)
        };
        topo.Controls.Add(lbl);
        Controls.Add(topo);

        var lblVendas = new Label
        {
            Text = "Vendas",
            Location = new Point(20, 75),
            AutoSize = true,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = UIHelper.CorTexto
        };
        Controls.Add(lblVendas);

        _gridVendas.Location = new Point(20, 100);
        _gridVendas.Size = new Size(900, 200);
        _gridVendas.AllowUserToAddRows = false;
        _gridVendas.ReadOnly = true;
        _gridVendas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _gridVendas.MultiSelect = false;
        _gridVendas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridVendas.BackgroundColor = Color.White;
        _gridVendas.RowHeadersVisible = false;
        _gridVendas.SelectionChanged += (_, _) => CarregarItensSelecionados();
        Controls.Add(_gridVendas);

        var lblItens = new Label
        {
            Text = "Itens da venda selecionada",
            Location = new Point(20, 315),
            AutoSize = true,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = UIHelper.CorTexto
        };
        Controls.Add(lblItens);

        _gridItens.Location = new Point(20, 340);
        _gridItens.Size = new Size(900, 160);
        _gridItens.AllowUserToAddRows = false;
        _gridItens.ReadOnly = true;
        _gridItens.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridItens.BackgroundColor = Color.White;
        _gridItens.RowHeadersVisible = false;
        Controls.Add(_gridItens);

        var btnAtualizar = new Button { Text = "Atualizar lista", Location = new Point(20, 515), Size = new Size(160, 35) };
        UIHelper.EstilizarBotaoSecundario(btnAtualizar);
        btnAtualizar.Click += (_, _) => CarregarDados();

        var btnFechar = new Button { Text = "Fechar", Location = new Point(190, 515), Size = new Size(120, 35) };
        UIHelper.EstilizarBotaoPrimario(btnFechar);
        btnFechar.Click += (_, _) => Close();

        Controls.Add(btnAtualizar);
        Controls.Add(btnFechar);
    }

    private void CarregarDados()
    {
        var lista = _service.ListarTodos();

        _gridVendas.Columns.Clear();
        _gridVendas.Rows.Clear();
        _gridVendas.Columns.Add("Id", "ID");
        _gridVendas.Columns.Add("Data", "Data");
        _gridVendas.Columns.Add("Itens", "Qtd. itens");
        _gridVendas.Columns.Add("Subtotal", "Subtotal");
        _gridVendas.Columns.Add("Desconto", "Desconto");
        _gridVendas.Columns.Add("Total", "Total");

        foreach (var v in lista.OrderByDescending(x => x.Id))
        {
            _gridVendas.Rows.Add(
                v.Id,
                v.DataVenda.ToString("dd/MM/yyyy HH:mm"),
                v.Itens.Count,
                v.Subtotal.ToString("C2"),
                v.Desconto.ToString("C2"),
                v.ValorTotal.ToString("C2"));
        }

        _gridItens.Columns.Clear();
        _gridItens.Rows.Clear();
        if (_gridVendas.Rows.Count > 0)
            _gridVendas.Rows[0].Selected = true;
        else
            CarregarItensSelecionados();
    }

    private void CarregarItensSelecionados()
    {
        _gridItens.Columns.Clear();
        _gridItens.Rows.Clear();

        _gridItens.Columns.Add("IdProduto", "ID prod.");
        _gridItens.Columns.Add("Nome", "Produto");
        _gridItens.Columns.Add("Qtd", "Qtd");
        _gridItens.Columns.Add("Unit", "Preco unit.");
        _gridItens.Columns.Add("Sub", "Subtotal");

        if (_gridVendas.SelectedRows.Count == 0) return;

        var idCelula = _gridVendas.SelectedRows[0].Cells["Id"].Value;
        if (idCelula == null || !int.TryParse(idCelula.ToString(), out int idVenda)) return;

        var venda = _service.ListarTodos().FirstOrDefault(v => v.Id == idVenda);
        if (venda == null) return;

        foreach (var i in venda.Itens)
        {
            _gridItens.Rows.Add(
                i.IdProduto,
                i.NomeProduto,
                i.Quantidade,
                i.PrecoUnitario.ToString("C2"),
                i.Subtotal.ToString("C2"));
        }
    }
}
