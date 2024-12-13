﻿namespace Yarışma.Areas.Management.Models
{
    public class JudgeItemViewModel
    {
        public int JudgeId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UniversityOrWorkplace { get; set; } // Üniversite veya işyeri bilgisi
        public string JudgeCategoryName { get; set; }
        public string ProjectCategoryName { get; set; }
        public bool IsApproved { get; set; }
    }
}
