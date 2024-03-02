using Learning.App.Data;
using Learning.Business.Interfaces;
using Learning.Data.Context;
using Learning.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "O valor fornecido é inválido.");
    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "O campo é obrigatório.");
    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "O campo é obrigatório.");
    options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "É necessário fornecer um corpo na solicitação HTTP.");
    options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => "O valor fornecido é inválido.");
    options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor fornecido é inválido.");
    options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O campo deve ser um número.");
    options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => "O valor fornecido é inválido.");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => "O valor fornecido é inválido para " + x);
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "O campo " + x + " deve ser um número.");
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "O campo " + x + " não pode ser nulo.");
});

//Configurando AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

//Injeção de dependencia do meu contexto do banco e repositories
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IFornecedorRepository, FornecedorRepository>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var defaultCulture = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
