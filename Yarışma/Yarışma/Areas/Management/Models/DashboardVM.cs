using System.Collections.Generic;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Models
{
    public class DashboardVM
    {
        public int ContestantCount { get; set; }
        public int JudgeCount { get; set; }
        public int CategoryCount { get; set; }
        public int EvaluatedCount { get; set; }
        public int NotEvaluatedCount { get; set; }
        public List<ProjectCategory> ProjectCategories { get; set; } = new List<ProjectCategory>();
        public List<JudgeCategory> JudgeCategories { get; set; } = new List<JudgeCategory>();

        // Yeni özellikler
        public int GetTotalProjectsForCategory(ProjectCategory category)
        {
            return category.Projects?.Count ?? 0; // Proje sayısını döndür
        }

        public int GetTotalJudgesForCategory(JudgeCategory category)
        {
            return category.Judges?.Count ?? 0; // Hakem sayısını döndür
        }
    }
}
