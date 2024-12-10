using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public User RegisterUser(string name, string email, string password)
        {
            var hashedPassword = _passwordHasher.Hash(password);

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = hashedPassword
            };

            if (!ValidateUser(user))
            {
                throw new ArgumentException("Invalid user data");
            }

            _unitOfWork.User.Add(user);
            _unitOfWork.Save();

            return user;
        }

        private bool ValidateUser(User user)
        {
            return !string.IsNullOrEmpty(user.Name) && 
                   !string.IsNullOrEmpty(user.Email) && 
                   !string.IsNullOrEmpty(user.PasswordHash);
        }

        public UserSession AuthenticateUser(string email, string password)
        {
            var user = _unitOfWork.User.Get(u => u.Email == email);

            if (user is null || !user.VerifyPassword(password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            return new UserSession
            {
                Token = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}
