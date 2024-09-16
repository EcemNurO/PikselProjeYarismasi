using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class Period:BaseEntity
	{
		[Key]
		public int Id { get; set; }
		
		public string? PeriodName { get; set; }
		public DateTime? ContestanStartDate { get; set; }
		public DateTime? ContestantEndDate { get; set; }
		public DateTime? ProjectStartDate { get; set; }
		public DateTime? ProjectEndDate { get; set; }
		public DateTime? JudgeStartDate { get; set; }
		public DateTime? JudgeEndDate { get; set; }

	}
}
