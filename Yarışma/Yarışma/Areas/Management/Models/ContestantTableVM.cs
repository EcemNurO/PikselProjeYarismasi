using Yarışma.Models;

namespace Yarışma.Areas.Management.Models
{
	    public class ContestantTableVM
	    {
        public List<ContestantViewModel> Contestants { get; set; } = new List<ContestantViewModel>();
        public int TotalContestants { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string SearchQuery { get; set; }
       
    }
}
