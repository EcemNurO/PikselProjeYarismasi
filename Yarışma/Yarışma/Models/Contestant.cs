namespace Yarışma.Models
{
	public class Contestant
	{
	   
		public int Id { get; set; }
		public int ContestantProfilId { get; set; }
		public ContestantProfil contestantProfil { get; set; }
		public int ContestantCategoryId { get; set; }
		public ContestantCategory ContestantCategory { get; set; }		
		public ICollection<Project> Projects { get; set; }
	}
}

