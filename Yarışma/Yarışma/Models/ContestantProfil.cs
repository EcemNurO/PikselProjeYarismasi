using Microsoft.AspNetCore.Http;

namespace Yarışma.Models
{
	public class ContestantProfil:BaseEntity
	{
		public int Id { get; set; }
        public int usedContestantJudgeId { get; set; }
		public virtual UsedContestantJudge UsedContestantJudges { get; set; }
        public string FullName { get; set; }
		public string? Age { get; set; }
		public string? Phone { get; set; }
		public string Email { get; set; }
	
		public string? Univercity { get; set; }
		public string? Address { get; set; }
		public string? image { get; set; }
		public string? Biografy { get; set; }
		
		public ICollection<Contestant>? Contestants { get; set; }

    }
}
