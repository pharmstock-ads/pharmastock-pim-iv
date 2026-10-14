using estoque_farmacia.UI;
using estoque_farmacia.Services;

// var identifica o tipo de variavel automaticamente
var funcionarioService = new FuncionarioService();
var fornecedorService = new FornecedorService();
var produtoService = new ProdutoService(fornecedorService); //produto depende de fornecedor
var loteService = new LoteService(produtoService); //lote depende de produto
var estoque = new Estoque(loteService); //estoque depende de lote
var vendaService = new VendaService(produtoService, estoque); //venda depende de produto e estoque

// cria a instancia do menu passando as uis prontas
var menu = new Menu(
    new FuncionarioUI(funcionarioService),
    new ProdutoUI(produtoService),
    new FornecedorUI(fornecedorService),
    new LoteUI(loteService),
    new VendaUI(produtoService, vendaService, estoque));

try
{
    if (menu.ValidarLogin())
        menu.ProcessarMenu();
    else
        Console.WriteLine("Sistema encerrado por não autorização.");
}
catch (Exception ex)
{
    Console.WriteLine($"ERRO ao inicializar aplicação: {ex.Message}");
}

// exceptionex é a classe mais generica de erro