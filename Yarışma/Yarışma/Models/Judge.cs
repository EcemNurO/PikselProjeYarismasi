using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class Judge
	{
		[Key]
		public int Id { get; set; }
		public int JudgeProfilId { get; set; }
		public ContestantProfil contestantProfil { get; set; }
		public int JudgeCategoryId { get; set; }
		public JudgeCategory JudgeCategory { get; set; }
		public int ProjectEvaluationId { get; set; }
		public virtual ProjectEvaluation? ProjectEvaluation { get; set; }
		public int ProjectCategoryId { get; set; }
		public virtual ProjectCategory ProjectCategory { get; set; }
		
		
	}
}

