using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class BaseEntity
	{
		[ DefaultValue(true)]
		public bool Status { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string? UpdatedBy { get; set; }
		public bool? Deleted { get; set; }
	}
}
