namespace Yarışma.Models
{
	public class JudgeCategory:BaseEntity
	{
		public int Id { get; set; }            
		public string? Name { get; set; }

		public ICollection<Judge>? Judges { get; set; }
	}
}