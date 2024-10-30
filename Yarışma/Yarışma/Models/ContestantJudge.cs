using NuGet.DependencyResolver;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yarışma.Models
{
    public class ContestantJudge
    {
        [Required]
        public int ContestantId{ get; set; }
        [ForeignKey("ContestantId")]
        public virtual Contestant Contestant { get; set; }
        [Required]
        public int JudgeId { get; set; }
        [ForeignKey("JudgeId")]
        public virtual Judge Judge { get; set; }
    }
}
