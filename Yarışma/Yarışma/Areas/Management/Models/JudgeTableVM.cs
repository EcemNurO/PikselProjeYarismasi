namespace Yarışma.Areas.Management.Models
{
    public class JudgeTableVM
    {
        public List<JudgeTableViewModel> Judges { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}
