﻿using Yarışma.Models;

namespace Yarışma.Areas.Management.Models
{
    public class JudgeViewModel
    {
        public List<Judge> Judges { get; set; }
        public List<JudgeProfil> JudgeProfils { get; set; }
        public List<JudgeCategory> JudgeCategories { get; set; }
        public List<ProjectCategory> ProjectCategories { get; set; }
        public List<JudgeItemViewModel> JudgeItemViewModels { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string UniversityOrWorkplace { get; set; }
    }
}
