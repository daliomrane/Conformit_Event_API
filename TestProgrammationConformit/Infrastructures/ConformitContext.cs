using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestProgrammationConformit.Entities;

namespace TestProgrammationConformit.Infrastructures
{
    public class ConformitContext : DbContext
    {
        public ConformitContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Evenement> Evenements { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evenement>()
            .HasMany(c => c.Commentaires)
            .WithOne(e => e.ParentEvent).OnDelete(DeleteBehavior.Cascade);
        }

    }
}
