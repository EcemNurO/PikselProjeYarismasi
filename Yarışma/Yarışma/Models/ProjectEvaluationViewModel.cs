using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
    public class ProjectEvaluationViewModel
    {
        public int ProjectId { get; set; } 

        public int JudgeId { get; set; }


        [Range(0, 100, ErrorMessage = "Puan 0 ile 100 arasında olmalıdır.")]
        public int? Score { get; set; } 
        public string? Comments { get; set; }
        public string? ProjectName { get; set; }
        public int? JudgeCategoryId { get; set; }
        public string? ProjectCategory { get; set; }
        public List<QuestionWithAnswerViewModel>? ProjectAnswers { get; set; }
        public string? FilePath { get; set; }
    }
}
