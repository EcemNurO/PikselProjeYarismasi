using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Yarışma.Models
{
	public class Project
	{

		public int Id { get; set; }
		public string Name { get; set; }
		public int ProjectQuestionId { get; set; }
		public virtual ProjectQuestion Question { get; set; }
		public string Answer { get; set; }
		public int ContestantId { get; set; }
		public int ProjectCategoryId { get; set; }
		
		public ProjectCategory ProjectCategory { get; set; }
		public Contestant Contestant { get; set; }
		public bool Status { get; set; }
		
	}
}
