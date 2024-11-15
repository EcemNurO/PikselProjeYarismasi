using DocumentFormat.OpenXml.Presentation;
using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class JudgeProfil:BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public int UsedContestantJudgeId { get; set; }
		public virtual UsedContestantJudge UsedContestantJudges { get; set; }
		public string FullName { get; set; }
		public string? Phone { get; set; }
		public string Email { get; set; }
	
		public string? Univercity { get; set; }
		public string? Address { get; set; }
		public string? image { get; set; }
		public string? Biografy { get; set; }

        public ICollection<Judge>? Judge { get; set; }
    }
}
