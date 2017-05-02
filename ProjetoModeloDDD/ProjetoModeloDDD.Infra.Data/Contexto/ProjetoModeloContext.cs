using ProjetoModeloDDD.Domain.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace ProjetoModeloDDD.Infra.Data.Contexto
{
    public class ProjetoModeloContext : DbContext 
    {
        public ProjetoModeloContext() : base("ProjetoModeloDDD")
        {

        }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Não Plularizar tabelas
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Não deletar relaciomanto um para muitos
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // // Não deletar relaciomanto muitos para muitos
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Forçar que todo o nome de campo que conter Id é uma chave promária
            modelBuilder.Properties()
                .Where(p => p.Name == p.ReflectedType.Name + "Id")
                .Configure(p => p.IsKey());

            // Alterar os campos string nvarchar para varchar
            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnType("varchar"));

            // Padronizar tamanho do campo string
            modelBuilder.Properties<string>()
                .Configure(p => p.HasMaxLength(100));
        }
        public override int SaveChanges()
        {
            foreach(var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("Data Cadastro") != null))
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Property("Data Cadastro").CurrentValue = DateTime.Now;
                }

                if(entry.State == EntityState.Modified)
                {
                    entry.Property("Data Cadastro").IsModified = false;
                }
            }
            return base.SaveChanges();
        }
    }
}
 