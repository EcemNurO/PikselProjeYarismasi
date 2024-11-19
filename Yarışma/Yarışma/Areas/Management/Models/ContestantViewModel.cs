namespace Yarışma.Areas.Management.Models
{
    public class ContestantViewModel
    {
        public int ContestantId { get; set; }
        public string ContestantName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCategoryName { get; set; }
        public string AssignedAcademicJudgeName { get; set; }
        public string AssignedIndustrialJudgeName { get; set; }
        public bool IsAcademicJudgeAssigned { get; set; }
        public bool IsIndustrialJudgeAssigned { get; set; }

        public double? AcademicJudgeScore { get; set; } 
        public double? IndustrialJudgeScore { get; set; } 

  
        public double? AverageScore { get; set; }
       
        public string MissingJudgeType
        {
            get
            {
                if (!IsAcademicJudgeAssigned && !IsIndustrialJudgeAssigned)
                    return "Akademisyen ve Sanayici Hakem Eksik";
                if (!IsAcademicJudgeAssigned)
                    return "Akademisyen Hakem Eksik";
                if (!IsIndustrialJudgeAssigned)
                    return "Sanayici Hakem Eksik";
                return "Tamamlandı"; 
            }
        }
    }
}
