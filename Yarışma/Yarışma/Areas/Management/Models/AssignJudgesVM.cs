using Yarışma.Models;

namespace Yarışma.Areas.Management.Models
{
    public class AssignJudgesVM
    {
        public int ContestantId { get; set; }
        public string ContestantName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<Judge> AcademicJudges { get; set; }
        public List<Judge> IndustrialJudges { get; set; }
        public bool IsAssignmentComplete { get; set; }
    }
}
