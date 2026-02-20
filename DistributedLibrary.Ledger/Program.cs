using DistributedLibrary.Ledger.Infrastructure.DB;
using FluentValidation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB
if (!builder.Environment.IsEnvironment("Testing"))
{
    // This gets the folder where the .exe is running
    string baseDir = AppDomain.CurrentDomain.BaseDirectory;

    // Combine it with your filename (no dots needed)
    string dbPath = Path.Combine(baseDir, "ledger_database.db");

    var connection = new SqliteConnection($"Data Source={dbPath}");
    connection.Open();

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlite(connection);
    });
}

// MediatR and FluentValidation
builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
