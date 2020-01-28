using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    class CandidateContext : DbContext
    {
        static CandidateContext()
        {
            Database.SetInitializer<CandidateContext>(new DbInitializer());
        }
        public CandidateContext() : base ("DBConnection") {}
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<Framework> Frameworks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.ProgrammingLanguages)
                .WithMany(p => p.Candidates);
            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.Frameworks)
                .WithMany(f => f.Candidates);
            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.Languages)
                .WithMany(l => l.Candidates);
        }
    }
}
