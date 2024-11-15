﻿using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class Judge:BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public int JudgeProfilId { get; set; }
		public JudgeProfil? JudgeProfil { get; set; }
		public int JudgeCategoryId { get; set; }
		public JudgeCategory JudgeCategory { get; set; }
		public int? ProjectEvaluationId { get; set; }
		public virtual ProjectEvaluation? ProjectEvaluation { get; set; }
	
        public bool IsApproved { get; set; } = false;
		//  public int? ProjectId { get; set; }
		//public virtual Project Project { get; set; }
        public int? ProjectCategoryId { get; set; } 
        public ProjectCategory? ProjectCategory { get; set; }
		public ICollection<Project>Projects { get; set; }


    }
}

