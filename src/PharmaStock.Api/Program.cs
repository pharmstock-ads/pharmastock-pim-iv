using System.Collections.Generic;
using Microsoft.OpenApi;
using PharmaStock.Negocio;
using PharmaStock.Repositorio;
using PharmaStock.Repositorio.Abstracoes;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Padrao")
    ?? throw new InvalidOperationException("Connection string 'Padrao' não encontrada no appsettings.json.");

builder.Services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
builder.Services.AddScoped<IFuncionarioRepositorio, FuncionarioRepositorio>();
builder.Services.AddScoped<IFornecedorRepositorio, FornecedorRepositorio>();
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<ILoteRepositorio, LoteRepositorio>();
builder.Services.AddScoped<IEntradaRepositorio, EntradaRepositorio>();
builder.Services.AddScoped<IVendaRepositorio, VendaRepositorio>();
builder.Services.AddScoped<IAlertaRepositorio, AlertaRepositorio>();

builder.Services.AddScoped<AuthNegocio>();
builder.Services.AddScoped<CadastroNegocio>();
builder.Services.AddScoped<EntradaNegocio>();
builder.Services.AddScoped<VendaNegocio>();
builder.Services.AddScoped<AlertaNegocio>();

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    // Ordem das tags no OpenAPI = fluxo operacional (como na Web).
    // Scalar usa essa ordem quando não força ordenação alfabética.
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Tags = new HashSet<OpenApiTag>
        {
            new() { Name = "Auth" },
            new() { Name = "Fornecedores" },
            new() { Name = "Produtos" },
            new() { Name = "Entradas" },
            new() { Name = "Lotes" },
            new() { Name = "Vendas" },
            new() { Name = "Alertas" },
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
