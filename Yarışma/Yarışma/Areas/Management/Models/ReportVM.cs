using Yarışma.Models;

namespace Yarışma.Areas.Management.Models
{
	public class ReportVM
	{
		public List<ContestantProfil> contestantProfils { get; set; }
		public List<JudgeProfil> judgeProfils { get; set; }
		public List<ProjectCategory> projectCategories { get; set; }
		public List<JudgeCategory> JudgeCategories { get; set; }

		public List<ContestantCategory> ContestantCategories { get; set; }

	}
}
