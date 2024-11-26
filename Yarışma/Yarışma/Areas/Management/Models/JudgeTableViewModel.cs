namespace Yarışma.Areas.Management.Models
{
    public class JudgeTableViewModel
    {

        public int JudgeId { get; set; }    
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string University { get; set; }
        public string JudgeCategoryName { get; set; }
        public string ProjectCategories { get; set; }
        public string UniversityOrWorkplace { get; set; }
        public bool HasAssignedProject { get; set; }
    }
}
