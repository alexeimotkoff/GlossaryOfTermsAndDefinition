namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataContext : DbContextIndexed
    {
        public virtual DbSet<Definitions> Definitions { get; set; }
        public virtual DbSet<FundamentalAreas> FundamentalAreas { get; set; }
        public virtual DbSet<Glossaries> Glossaries { get; set; }
        public virtual DbSet<InformationSystems> InformationSystems { get; set; }
        public virtual DbSet<LifeСycle> LifeСycle { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Signatures> Signatures { get; set; }
        public virtual DbSet<Terms> Terms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FundamentalAreas>()
                .HasMany(e => e.Terms)
                .WithRequired(e => e.FundamentalAreas)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InformationSystems>()
                .HasMany(e => e.Terms)
                .WithRequired(e => e.InformationSystems)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LifeСycle>()
                .HasMany(e => e.Projects)
                .WithRequired(e => e.LifeСycle)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Projects>()
                .HasMany(e => e.Glossaries)
                .WithRequired(e => e.Projects)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Projects>()
                .HasMany(e => e.Signatures)
                .WithRequired(e => e.Projects)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Terms>()
                .HasMany(e => e.Definitions)
                .WithRequired(e => e.Terms)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Terms>()
                .HasMany(e => e.Glossaries)
                .WithRequired(e => e.Terms)
                .WillCascadeOnDelete(false);
        }
        
    }
}
