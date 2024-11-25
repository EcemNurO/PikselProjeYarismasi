using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Yarışma.Models;

namespace Yarışma.Repositories
{
    public class UserRepository
    {
        CompetitionDbContext db = new CompetitionDbContext();
       
        public async Task<UsedContestantJudge> GetUserByEmailAsync(string email)
        {
            return await db.usedContestantJudges.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<UsedContestantJudge> GetByIdAsync(int userId)
        {
            return await db.usedContestantJudges.FindAsync(userId);
        }

        // Kullanıcı bilgilerini günceller
        public async Task UpdateAsync(UsedContestantJudge user)
        {
           db.usedContestantJudges.Update(user);
            await db.SaveChangesAsync();
        }

    }
}