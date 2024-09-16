﻿using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class JudgeProfil:BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public string FullName { get; set; }
		public int Phone { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Univercity { get; set; }
		public string Address { get; set; }
		public string image { get; set; }
		public string Biografy { get; set; }
		public ICollection<Judge>? Judges { get; set; }
	}
}
