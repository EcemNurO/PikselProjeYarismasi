using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class ProjectQuestion
	{
		[Key] 
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}
}
