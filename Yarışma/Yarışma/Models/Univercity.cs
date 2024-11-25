using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
    public class Univercity
    {
        [Key]
        public int Id { get; set; }
        public string UniversityName { get; set; }
    }
}