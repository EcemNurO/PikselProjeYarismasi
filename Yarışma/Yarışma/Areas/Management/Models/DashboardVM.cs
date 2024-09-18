using Yarışma.Models;

namespace Yarışma.Areas.Management.Models
{
	public class DashboardVM
	{
		public int ContestantCount { get; set; }
		public int JudgeCount { get; set; }
		public int CategoryCount { get; set; }
		public DateTime RemainingTime { get; set; }
		public List<ContestantProfil> contestantProfils { get; set; }
		public List<JudgeProfil> judgeProfils { get; set; }
		public List<ProjectCategory> projectCategories { get; set; }
		public List<JudgeCategory> JudgeCategories { get; set; }

		public List<ContestantCategory> ContestantCategories { get; set; }


	}
	public class ContestantCategoryCount
	{
		public string CategoryName { get; set; }
		public int Count { get; set; }
	}
}
