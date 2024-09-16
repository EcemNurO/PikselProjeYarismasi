using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class ProjectQuestion:BaseEntity
	{
		[Key] 
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}
}
