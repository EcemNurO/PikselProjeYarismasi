using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class ContestantCategory:BaseEntity
	{
		[Key]
		public int Id { get; set; }
		[Required]	
		public string Name { get; set; }
		public ICollection<Contestant>? Contestants { get; set; }
		
	}
}
