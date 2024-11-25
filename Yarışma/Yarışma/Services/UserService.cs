using Yarışma.Models;
using Yarışma.Repositories;

namespace Yarışma.Services
{

    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly TokenRepository _tokenRepository;

        public UserService(UserRepository userRepository, TokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task<string> CreatePasswordResetTokenAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return null;

            var token = Guid.NewGuid().ToString();
            await _tokenRepository.SaveTokenAsync(user.Id, token, DateTime.UtcNow.AddHours(1));
            return token;
        }

        public async Task<UsedContestantJudge> ValidateResetTokenAsync(string token)
        {
            return await _tokenRepository.GetUserByTokenAsync(token);
        }

        public async Task UpdatePasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.Password = HashPassword(newPassword);
                await _userRepository.UpdateAsync(user);
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}