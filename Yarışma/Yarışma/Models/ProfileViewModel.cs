namespace Yarışma.Models
{
    public class ProfileViewModel
    {
             public ContestantProfil Profile { get; set; }
            public IEnumerable<ContestantCategory> ContestantCategories { get; set; }
            public IEnumerable<ProjectCategory> ProjectCategories { get; set; }
            public int SelectedContestantCategoryId { get; set; }
            public int SelectedProjectCategoryId { get; set; }
                public IFormFile imageFile { get; set; }
    }
}
