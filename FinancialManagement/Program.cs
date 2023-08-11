using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
// add services into IoC container 
builder.Services.AddSingleton<ICurrenciesService, CurrenciesService>();
builder.Services.AddSingleton<ICountriesService, CountriesService>();
builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<IFinancialAccountsService, FinancialAccountsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseStaticFiles();
app.MapControllers();

app.Run();