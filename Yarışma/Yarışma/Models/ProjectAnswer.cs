namespace Yarışma.Models
{
    public class ProjectAnswer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ProjectQuestionId { get; set; }
        public virtual ProjectQuestion Question { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
