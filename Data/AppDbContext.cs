using CadastroApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroApi.Data
{
    // Contexto do banco de dados que gerencia as tabelas Pessoas e Cidades
    public class AppDbContext : DbContext
    {
        // Construtor que recebe as configurações do DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Representa a tabela Pessoas no banco de dados
        public DbSet<Pessoa> Pessoas => Set<Pessoa>();

        // Representa a tabela Cidades no banco de dados
        public DbSet<Cidade> Cidades => Set<Cidade>();
    }
}
