using System;
using Curso.Data.Configurations;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole()); // Para escrever os Logs do app no Console

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger) //Para escrever o Log
                .EnableSensitiveDataLogging() //Exibe o valor de cada parametro no console
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=CursoEFCore;Integrated Security=true;",
                p => p.EnableRetryOnFailure(
                    maxRetryCount: 2, //Caso haja falhas de conexao com o banco ele fica tentando por x vezes
                    maxRetryDelay: TimeSpan.FromSeconds(5), //e a cada tentativa aguarda por x minutos
                    errorNumbersToAdd: null)
                    .MigrationsHistoryTable("curso_ef_core")); // Nome da tabela de migrações
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Faz o Mapeamento de todas as classes que esta implementando o IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }

    }
}