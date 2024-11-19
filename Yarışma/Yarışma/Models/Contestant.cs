using DocumentFormat.OpenXml.Presentation;

namespace Yarışma.Models
{
	public class Contestant:BaseEntity
	{
        //public Contestant()
        //{
        //    this.Projects = new HashSet<Project>();
        //}
        public int Id { get; set; }
		public int ContestantProfilId { get; set; }
		public ContestantProfil contestantProfil { get; set; }
		public int ContestantCategoryId { get; set; }
		public ContestantCategory ContestantCategory { get; set; }	
        public virtual Project Projects { get; set; }
		
    
        //public ICollection<Project> Projects { get; set; }

    }
}

