﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TermsAndDefinitions.WebUI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GlossaryProjectDatabaseEntities : DbContext
    {
        public GlossaryProjectDatabaseEntities()
            : base("name=GlossaryProjectDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<FundamentalArea> FundamentalAreas { get; set; }
        public virtual DbSet<InformationSystem> InformationSystems { get; set; }
        public virtual DbSet<LifeCycle> LifeCycles { get; set; }
        public virtual DbSet<Reference> References { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<webpages_Roles> webpages_Roles { get; set; }
        public virtual DbSet<BucketHash> BucketHashes { get; set; }
        public virtual DbSet<MinHash> MinHashes { get; set; }
        public virtual DbSet<Definition> Definitions { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Term> Terms { get; set; }
    }
}
