using Yarışma.Models;

namespace Yarışma.Areas.Management.Models
{
	public class ContestantTableVM
	{
		public List<ContestantProfil> ContestantProfils { get; set; }
		public List<Contestant> Contestant { get; set; }
		public List<ContestantCategory> contestantCategories { get; set; }
		public List<JudgeProfil> JudgeProfils { get; set; }
		public List<Judge> Judge { get; set; }
		public List<JudgeCategory> JudgeCategories { get; set; }
		public List<Project> projects { get; set; }
		public List<ContestantJudge> ContestantJudges { get; set; }


        public int TotalCount { get; set; }   
        public int PageSize { get; set; }     
        public int CurrentPage { get; set; }
    }
}
