namespace Yarışma.Models
{
    public class ProjectEvaluationViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCategoryName { get; set; }
        public int? Score { get; set; }
        public string Comments { get; set; }

        // Soru ve Cevaplar
        public List<ProjectQuestion> ProjectQuestions { get; set; }
        public List<ProjectAnswer> ProjectAnswers { get; set; }
    }
}
