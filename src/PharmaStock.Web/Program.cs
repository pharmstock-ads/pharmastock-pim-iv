using PharmaStock.Negocio;
using PharmaStock.Repositorio;
using PharmaStock.Repositorio.Abstracoes;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Padrao")
    ?? throw new InvalidOperationException("Connection string 'Padrao' não encontrada no appsettings.json.");

// DI identico ao da Api (molde do professor).
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

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
