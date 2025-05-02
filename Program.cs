using CadastroApi.Data;
using CadastroApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configura o banco SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=cadastro.db"));

// Configurações de serialização JSON para ignorar ciclos de referência
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.WriteIndented = true;
    });

// Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilita o uso de controladores (necessário para APIs RESTful)
builder.Services.AddControllers();

var app = builder.Build();

// Ativar Swagger se em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona todas as requisições HTTP para HTTPS
app.UseHttpsRedirection();

// Endpoints - Pessoas (CRUD)

// Endpoint para listar todas as pessoas com informações de suas cidades
app.MapGet("/pessoas", async (AppDbContext db) =>
    await db.Pessoas.Include(p => p.Cidade).ToListAsync());

// Endpoint para criar uma nova pessoa, com validações básicas
app.MapPost("/pessoas", async (Pessoa pessoa, AppDbContext db) =>
{
    // Validação de campos obrigatórios
    if (string.IsNullOrEmpty(pessoa.Nome) || pessoa.Idade <= 0 || string.IsNullOrEmpty(pessoa.Email) || pessoa.CidadeId <= 0)
    {
        return Results.BadRequest("Todos os campos são obrigatórios.");
    }

    // Validação de formato de email (método simples para validar)
    if (!pessoa.Email.Contains("@"))
    {
        return Results.BadRequest("O email fornecido é inválido.");
    }

    // Verificar se a cidade fornecida existe no banco de dados
    var cidade = await db.Cidades.FindAsync(pessoa.CidadeId);
    if (cidade == null)
    {
        return Results.BadRequest("A cidade fornecida não existe.");
    }

    // Adiciona a nova pessoa ao banco de dados e salva as alterações
    db.Pessoas.Add(pessoa);
    await db.SaveChangesAsync();
    return Results.Created($"/pessoas/{pessoa.Id}", pessoa);
});

// Endpoint para buscar uma pessoa específica pelo ID
app.MapGet("/pessoas/{id}", async (int id, AppDbContext db) =>
    await db.Pessoas.Include(p => p.Cidade).FirstOrDefaultAsync(p => p.Id == id)
        is Pessoa p ? Results.Ok(p) : Results.NotFound());

// Endpoint para atualizar os dados de uma pessoa existente
app.MapPut("/pessoas/{id}", async (int id, Pessoa entrada, AppDbContext db) =>
{
    var pessoa = await db.Pessoas.FindAsync(id);
    if (pessoa is null) return Results.NotFound();

    pessoa.Nome = entrada.Nome;
    pessoa.Idade = entrada.Idade;
    pessoa.Email = entrada.Email;
    pessoa.CidadeId = entrada.CidadeId;

    // Salva as alterações no banco de dados
    await db.SaveChangesAsync();
    return Results.Ok(pessoa);
});

// Endpoint para deletar uma pessoa existente
app.MapDelete("/pessoas/{id}", async (int id, AppDbContext db) =>
{
    var pessoa = await db.Pessoas.FindAsync(id);
    if (pessoa is null) return Results.NotFound();

    db.Pessoas.Remove(pessoa);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Endpoints - Cidades (CRUD)

// Endpoint para listar todas as cidades e as pessoas relacionadas
app.MapGet("/cidades", async (AppDbContext db) =>
    await db.Cidades.Include(c => c.Pessoas).ToListAsync());

// Endpoint para criar uma nova cidade
app.MapPost("/cidades", async (Cidade cidade, AppDbContext db) =>
{
    db.Cidades.Add(cidade);
    await db.SaveChangesAsync();
    return Results.Created($"/cidades/{cidade.Id}", cidade);
});

// Inicializa a aplicação
app.Run();
