using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Yarışma.Models
{
	public class Project:BaseEntity
	{
    
        public int Id { get; set; }
		public string Name { get; set; }
	  public int JudgeId { get; set; }
        public virtual Judge Judge { get; set; }

		public int ContestantId { get; set; }
		public int ProjectCategoryId { get; set; }  
        public ProjectCategory ProjectCategory { get; set; }
		public Contestant Contestant { get; set; }
		public bool Status { get; set; }
        public string? FilePath { get; set; }
        public virtual ICollection<ProjectQuestion> ProjectQuestions { get; set; }
      
        public virtual ICollection<ProjectAnswer> ProjectAnswers { get; set; }

    }
}
