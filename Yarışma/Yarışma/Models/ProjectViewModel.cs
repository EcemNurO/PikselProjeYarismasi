namespace Yarışma.Models
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; } // Proje Kimliği
        public string ProjectCategory { get; set; }
        public string Name { get; set; } // Projenin adı

        public List<QuestionViewModel> Questions { get; set; } // Sorular ve cevaplar listesi
        public string FilePath { get; set; }
    }
    public class QuestionViewModel
    {
        public int QuestionId { get; set; } // Soru Kimliği
        public string Text { get; set; } // Soru metni
        public string Description { get; set; }
        public string Answer { get; set; } // Kullanıcının verdiği cevap
    }
}
