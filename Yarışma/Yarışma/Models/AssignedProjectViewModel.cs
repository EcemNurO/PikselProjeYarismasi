namespace Yarışma.Models
{
    public class AssignedProjectViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCategory { get; set; }
        public List<QuestionWithAnswerViewModel> Questions { get; set; }
        public int? Score { get; set; }
        public string Comments { get; set; }
        public string? FilePath { get; set; }
    }

    public class QuestionWithAnswerViewModel
    {
        public string Question { get; set; } 
        //public string QuestionTitle { get; set; } 
        public string Answer { get; set; } 
        public int ProjectQuestionId { get; set; } 
    }
}
