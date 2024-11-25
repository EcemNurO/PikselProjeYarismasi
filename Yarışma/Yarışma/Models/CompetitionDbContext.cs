using Microsoft.EntityFrameworkCore;

namespace Yarışma.Models
{
    public class CompetitionDbContext : DbContext
    {
        public CompetitionDbContext()
        {
        }

        public CompetitionDbContext(DbContextOptions<CompetitionDbContext> options) : base(options)
        {
        }

        public DbSet<ContestantProfil> ContestantProfils { get; set; }
        public DbSet<JudgeProfil> JudgeProfils { get; set; }
        public DbSet<Judge> Judges { get; set; }
        public DbSet<Contestant> Contestants { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectEvaluation> ProjectEvaluations { get; set; }
        public DbSet<ContestantCategory> ContestantCategories { get; set; }
        public DbSet<JudgeCategory> JudgeCategories { get; set; }
        public DbSet<ProjectCategory> ProjectCategories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
       public DbSet<UsedContestantJudge> usedContestantJudges { get; set; }
        public DbSet<ProjectQuestion> ProjectQuestions { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<User> Users { get; set; }
       public DbSet<ScoreProject>ScoreProjects { get; set; }
        public DbSet<Token>Tokens { get; set; }

        public DbSet<ProjectAnswer> ProjectAnswers { get; set; }
        public DbSet<Univercity> univercities { get; set; }

        // Connection string configuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=ECEM;" +
                "Database=ProjeYarismaDb;Trusted_Connection=true;Encrypt=false;"
            );
            base.OnConfiguring(optionsBuilder);
        }

        // Configuring relationships and keys
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1:1 ilişki için yabancı anahtar belirtme
           


            // Diğer ilişki yapılandırmaları
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectCategory)
                .WithMany(pc => pc.Projects)
                .HasForeignKey(p => p.ProjectCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

                    modelBuilder.Entity<JudgeProfil>()
               .HasOne(j => j.UsedContestantJudges)
               .WithMany(u => u.JudgeProfils)
               .HasForeignKey(j => j.UsedContestantJudgeId)
               .OnDelete(DeleteBehavior.Restrict);


                    modelBuilder.Entity<ProjectAnswer>()
                .HasOne(pa => pa.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(pa => pa.ProjectQuestionId)
                .OnDelete(DeleteBehavior.NoAction);

                        modelBuilder.Entity<ProjectQuestion>()
            .HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.ProjectQuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        }


    }

}

