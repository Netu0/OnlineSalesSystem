using OnlineSalesSystem.Core.Entities;

namespace OnlineSalesSystem.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}