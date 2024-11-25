using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Yarışma.Models;

namespace Yarışma.Repositories
{
    public class TokenRepository
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public async Task SaveTokenAsync(int userId, string token, DateTime expiration)
        {
            var user = await db.usedContestantJudges.FindAsync(userId);
            if (user != null)
            {
                user.ResetToken = token;
                user.ResetTokenExpirationDate = expiration;
                await db.SaveChangesAsync();
            }
        }

        // Token'a göre kullanıcıyı bulur
        public async Task<UsedContestantJudge> GetUserByTokenAsync(string token)
        {
            return await db.usedContestantJudges.FirstOrDefaultAsync(u =>
                u.ResetToken == token && u.ResetTokenExpirationDate > DateTime.UtcNow);
        }
    }
}
