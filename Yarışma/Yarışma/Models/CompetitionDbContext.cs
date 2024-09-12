using Microsoft.EntityFrameworkCore;

namespace Yarışma.Models
{
	public class CompetitionDbContext:DbContext

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
		
		public DbSet<ProjectQuestion>ProjectQuestions { get; set; }
		public DbSet<Period> Periods { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(
				"Server=ECEM;"
				+ "Database=YarismaDb;Trusted_Connection=true;Encrypt=false;"
				);
			base.OnConfiguring(optionsBuilder);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

            modelBuilder.Entity<Judge>()
       .HasOne(j => j.ProjectEvaluation)
       .WithOne(pe => pe.Judge)
       .HasForeignKey<ProjectEvaluation>(pe => pe.JudgeId);

            modelBuilder.Entity<Project>()
           .HasOne(p => p.ProjectCategory)
           .WithMany(pc => pc.Projects) 
           .HasForeignKey(p => p.ProjectCategoryId);

            modelBuilder.Entity<Project>()
               .HasOne(p => p.ProjectCategory)
               .WithMany(pc => pc.Projects)
               .HasForeignKey(p => p.ProjectCategoryId)
               .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Question)
                .WithMany() 
                .HasForeignKey(p => p.ProjectQuestionId)
                .OnDelete(DeleteBehavior.NoAction);

			
			base.OnModelCreating(modelBuilder);
        }
	}
}
