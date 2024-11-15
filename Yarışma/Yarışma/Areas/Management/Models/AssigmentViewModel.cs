using Yarışma.Models;

namespace Yarışma.Areas.Management.Models
{
    public class AssigmentViewModel
    {
        public int ContestantId { get; set; }
        public int ProjectId { get; set; }
        public List<Judge> Judges { get; set; }
    }
}
