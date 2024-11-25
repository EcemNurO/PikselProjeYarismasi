namespace Yarışma.Models
{
    public class ContestantDetailsVM
    {
        public int ContestantId { get; set; }
        public string ContestantName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCategoryName { get; set; }
        public List<JudgeCommentViewModel> JudgeComments { get; set; } = new List<JudgeCommentViewModel>();
    }

    public class JudgeCommentViewModel
    {
        public string JudgeName { get; set; }
        public string JudgeCategory { get; set; } 
        public string Category { get; set; }
        public int? Score { get; set; }
        public string Comments { get; set; }
    }
}
