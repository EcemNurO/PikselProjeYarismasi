using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Yarışma.Models
{
	public class Project:BaseEntity
	{
    
        public int Id { get; set; }
		public string Name { get; set; }
		public int ContestantId { get; set; }
		public int ProjectCategoryId { get; set; }  
        public ProjectCategory ProjectCategory { get; set; }
		public Contestant Contestant { get; set; }
	
        public string? FilePath { get; set; }
        public byte[]? FileData { get; set; }
        public virtual ICollection<ProjectQuestion> ProjectQuestions { get; set; }
      
        public virtual ICollection<ProjectAnswer> ProjectAnswers { get; set; }
        public virtual ICollection<ProjectEvaluation>? ProjectEvaluations { get; set; }
        public string FileName => !string.IsNullOrEmpty(FilePath) ? Path.GetFileName(FilePath) : string.Empty;

    }
}
