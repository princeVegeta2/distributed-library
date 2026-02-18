using DistributedLibrary.Main.Domain;
using DistributedLibrary.Main.Features.Authors._Endpoints;
using DistributedLibrary.Main.Features.Books._Endpoints;
using DistributedLibrary.Main.Features.Users._Endpoints;
using DistributedLibrary.Main.Infrastructure.DB;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure the DATABASE
// We are using SQLITE, the data source should be memory for testing
if (!builder.Environment.IsEnvironment("Testing"))
{
    // This gets the folder where the .exe is running
    string baseDir = AppDomain.CurrentDomain.BaseDirectory;

    // Combine it with your filename (no dots needed)
    string dbPath = Path.Combine(baseDir, "database.db");

    var connection = new SqliteConnection($"Data Source={dbPath}");
    connection.Open();

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlite(connection);
    });
}

// MediatR and FluentValidation
// We are using MediatR instead of controller base
// We need to include internal types for the validator, since all classes are internal
// We need a FluentValidation.DependencyInjectionExtensions package to inject FluentValidation
builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

// Password hasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    await DBSeeder.SeedAsync(db, CancellationToken.None);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints
app.MapUserEndpoints();
app.MapAuthorEndpoints();
app.MapBookEndpoints();

app.Run();

// For test observability
public partial class Program { }
