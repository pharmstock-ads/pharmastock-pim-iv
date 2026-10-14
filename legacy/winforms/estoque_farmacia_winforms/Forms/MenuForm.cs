using System.Drawing;
using System.Windows.Forms;
using estoque_farmacia.Services;

namespace estoque_farmacia_winforms.Forms;

public class MenuForm : Form
{
    private readonly FornecedorService _fornecedorService = new();
    private readonly ProdutoService _produtoService;
    private readonly LoteService _loteService;
    private readonly Estoque _estoque;
    private readonly VendaService _vendaService;

    public MenuForm()
    {
        _produtoService = new ProdutoService(_fornecedorService);
        _loteService = new LoteService(_produtoService);
        _estoque = new Estoque(_loteService);
        _vendaService = new VendaService(_produtoService, _estoque);

        ConfigurarJanela();
        ConstruirTela();
    }

    private void ConfigurarJanela()
    {
        Text = "Menu Principal - Pharmastock";
        Size = new Size(780, 580);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UIHelper.CorFundo;
        Font = new Font("Segoe UI", 9F);
    }

    private void ConstruirTela()
    {
        // Cabecalho azul.
        var topo = new Panel
        {
            Dock = DockStyle.Top,
            Height = 70,
            BackColor = UIHelper.CorPrimaria
        };
        var lblTitulo = new Label
        {
            Text = "Menu Principal",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(30, 20)
        };
        topo.Controls.Add(lblTitulo);

        // Painel central: grade 2x2 + linha extra para Lotes.
        var painelBotoes = new TableLayoutPanel
        {
            Location = new Point(40, 100),
            Size = new Size(700, 400),
            ColumnCount = 2,
            RowCount = 3,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.None
        };
        painelBotoes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        painelBotoes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        painelBotoes.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
        painelBotoes.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
        painelBotoes.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34f));

        painelBotoes.Controls.Add(CriarBotaoMenu("Produtos", AbrirProdutos), 0, 0);
        painelBotoes.Controls.Add(CriarBotaoMenu("Funcionarios", AbrirFuncionarios), 1, 0);
        painelBotoes.Controls.Add(CriarBotaoMenu("Fornecedores", AbrirFornecedores), 0, 1);
        painelBotoes.Controls.Add(CriarBotaoMenu("Vendas", AbrirVendas), 1, 1);
        var btnLotes = CriarBotaoMenu("Lotes", AbrirLotes);
        painelBotoes.Controls.Add(btnLotes, 0, 2);
        painelBotoes.SetColumnSpan(btnLotes, 2);

        // Botao "Sair" no rodape.
        var btnSair = new Button
        {
            Text = "Sair",
            Location = new Point(640, 520),
            Size = new Size(100, 32)
        };
        UIHelper.EstilizarBotaoSecundario(btnSair);
        btnSair.Click += (_, _) => Close();

        Controls.Add(topo);
        Controls.Add(painelBotoes);
        Controls.Add(btnSair);
    }

    private Button CriarBotaoMenu(string texto, EventHandler aoClicar)
    {
        var btn = new Button
        {
            Text = texto,
            Dock = DockStyle.Fill,
            Margin = new Padding(15),
            BackColor = Color.White,
            ForeColor = UIHelper.CorPrimaria,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderColor = UIHelper.CorPrimaria;
        btn.FlatAppearance.BorderSize = 2;
        btn.Click += aoClicar;
        return btn;
    }

    // Cada handler abre o respectivo Form como dialogo (modal).
    // ShowDialog() bloqueia o menu ate o usuario fechar a tela aberta,
    // o que evita que multiplas janelas iguais sejam abertas ao mesmo tempo.

    private void AbrirProdutos(object? s, EventArgs e)
    {
        using var f = new ProdutoForm(_produtoService);
        f.ShowDialog(this);
    }

    private void AbrirFuncionarios(object? s, EventArgs e)
    {
        using var f = new FuncionarioForm(new FuncionarioService());
        f.ShowDialog(this);
    }

    private void AbrirFornecedores(object? s, EventArgs e)
    {
        using var f = new FornecedorForm(_fornecedorService);
        f.ShowDialog(this);
    }

    private void AbrirVendas(object? s, EventArgs e)
    {
        using var f = new VendaForm(_produtoService, _vendaService, _estoque);
        f.ShowDialog(this);
    }

    private void AbrirLotes(object? s, EventArgs e)
    {
        using var f = new LoteForm(_loteService);
        f.ShowDialog(this);
    }
}
