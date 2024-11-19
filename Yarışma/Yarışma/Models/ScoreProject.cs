using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
    public class ScoreProject
    {
        [Key]
        public int Id { get; set; }
        public int ProjectEvaluationId { get; set; }
        public virtual ProjectEvaluation ProjectEvaluation { get; set; }
        public int? Score { get; set; }  
        public string ?Comments { get; set; }
    }
}
