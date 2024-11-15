using Microsoft.AspNetCore.Mvc.Rendering;

namespace Yarışma.Models
{
    public class JudgeProfilViewModel
    {

        public JudgeProfil Profile { get; set; }
        //public List<SelectListItem> JudgeCategories { get; set; }
        public IEnumerable<JudgeCategory>JudgeCategories { get; set; }
        public IEnumerable<ProjectCategory> ProjectCategories { get; set; }
        public int SelectedProjectCategoryId { get; set; }
        public int SelectedJudgeCategoryId { get; set; }
        
    }
}
