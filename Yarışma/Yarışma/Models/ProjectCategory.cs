namespace Yarışma.Models
{
	public class ProjectCategory
	{
		
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Project> Projects { get; set; }
		public ICollection<Judge> Judges { get; set; }
	}
}
