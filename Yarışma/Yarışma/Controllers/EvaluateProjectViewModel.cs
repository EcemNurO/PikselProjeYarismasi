namespace Yarışma.Controllers
{
    internal class ProjectEvaluateViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCategory { get; set; }
        public string FilePath { get; set; }
        public int? Score { get; set; }
        public string Comments { get; set; }
    }
}