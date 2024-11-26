using Yarışma.Models;

namespace Yarışma.Areas.Management.Models
{
	public class ReportVM
	{
        public int RemainingTime { get; set; }
        public int TotalContestants { get; set; }
        public int TotalJudges { get; set; }
        public int TotalCategories { get; set; }
        public int EvaluatedProjects { get; set; }
        public int NotEvaluatedProjects { get; set; }
        public int NonEvaluatedProjects { get; set; }
       

        public List<CategoryReport> ContestantCategories { get; set; }
        public List<CategoryReport> JudgeCategories { get; set; }
        public List<CategoryReport> EducationLevels { get; set; }
        public List<CategoryReport> AcademicLevels { get; set; } = new List<CategoryReport>();
        public List<CategoryReport> ProjectCategories { get; set; } = new List<CategoryReport>();
    }

    public class CategoryReport
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

}

