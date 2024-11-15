using DocumentFormat.OpenXml.Presentation;
using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
    public class UsedContestantJudge : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public RoleTypes Role { get; set; }
      public ICollection<JudgeProfil>  JudgeProfils { get; set; }
        public ICollection<ContestantProfil> ContestantProfils { get; set; }
    }
}   