using Yarışma.Models;

namespace Yarışma.Areas.Management.Models
{
	public class ContestantTableVM
	{
        public List<Contestant> Contestants { get; set; }
        public int TotalContestants { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
    }
}
