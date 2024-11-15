﻿using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class ProjectEvaluation:BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public string? Comments { get; set; }
		public int? Score { get; set; }
		public int? JudgeId { get; set; }
		public virtual Judge? Judge { get; set; }
		public int ProjectId {  get; set; }
		public virtual Project Project { get; set; }
		
		
		
	}
}

