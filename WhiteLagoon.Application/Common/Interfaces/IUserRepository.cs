using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User entity);
    }
}
