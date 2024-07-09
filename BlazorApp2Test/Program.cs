using BlazorApp2Test.Components;
using BlazorApp2Test.Data;
using BlazorApp2Test.FileAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddSingleton<TextData>();
builder.Services.AddScoped<ReplyData>();
builder.Services.AddScoped<MemoData>();
builder.Services.AddSingleton<ErrorService>();
builder.Services.AddSingleton<RenderService>();
builder.Services.AddScoped<FileAccesses>(sp => new FileAccesses(
    sp.GetRequiredService<UserService>(),
    builder.Configuration["CloudflareR2:AccessKey"],
    builder.Configuration["CloudflareR2:SecretKey"],
    builder.Configuration["CloudflareR2:ServiceUrl"],
    builder.Configuration["CloudflareR2:BucketName"]
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();