using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortener.Data.EFCore;
using Shortener.Data;
using Shortener.Services;
using Shortener.Validators;
using Shortener.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IValidator<ShortenedLinkDto>, ShortenedLinkValidator>();
builder.Services.AddScoped<EfCoreShortenedLinkRepository>();
builder.Services.AddScoped<IManageLinksService, ManageLinksService>();
builder.Services.AddScoped<EfCoreShortenedLinkRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "redirectRoute",
    pattern: "{id?}",
    defaults: new { controller = "Home", action = "RedirectToOriginal" });

app.Run();
