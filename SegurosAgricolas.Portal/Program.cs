using Microsoft.EntityFrameworkCore;
using SegurosAgricolas.Context;
using SegurosAgricolas.Domain.Services.Interfaces;
using SegurosAgricolas.Domain.Services;
using System;
using SegurosAgricolas.Domain.Validator;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BeneficiarioDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBeneficiarioService, BeneficiarioService>();

builder.Services.AddScoped<BeneficiarioValidator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
