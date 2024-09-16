using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
	public class BaseEntity
	{
		[ DefaultValue(true)]
		public bool Status { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? UpdatedBy { get; set; }
		public bool? Deleted { get; set; }
	}
}
