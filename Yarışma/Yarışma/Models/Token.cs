namespace Yarışma.Models
{
    public class Token
    {
        public int Id { get; set; } // Benzersiz kimlik
        public string Value { get; set; } // Token değeri (örneğin, GUID)
        public DateTime CreatedAt { get; set; } // Oluşturulma tarihi
        public DateTime? ExpirationDate { get; set; } // Token'in geçerlilik süresi
        public bool IsUsed { get; set; } // Token kullanıldı mı?
        public int UsedContestantJudgeId { get; set; } // İlişkili kullanıcı
        public UsedContestantJudge UsedContestantJudge { get; set; } // Kullanıcı ile ilişki
    }
}
