using Microsoft.EntityFrameworkCore;
using OnlineSalesSystem.Core.Entities;
using OnlineSalesSystem.Core.Interfaces;
using OnlineSalesSystem.Infrastructure.Data;

namespace OnlineSalesSystem.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}