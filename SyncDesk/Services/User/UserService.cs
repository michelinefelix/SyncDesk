using Microsoft.EntityFrameworkCore;
using SyncDesk.Data;
using SyncDesk.Models.Entities;
using SyncDesk.Models.Enums;
using SyncDesk.Services.Interfaces;

namespace SyncDesk.Services.Implementations;

public class UserService : IUserService
{
    private readonly SyncDeskDbContext _context;

    public UserService(SyncDeskDbContext context)
    {
        _context = context;
    }

    public async Task<User?> ObterPorIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AtualizarStatusPresencaAsync(Guid userId, StatusPresencaEnum status)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user is not null)
        {
            user.StatusPresenca = status;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<User>> ObterAgentesDisponiveisAsync(Guid tenantId, Guid departmentId)
    {
        return await _context.Users
            .Where(u => u.TenantId == tenantId
                     && u.DepartmentId == departmentId
                     && u.StatusPresenca == StatusPresencaEnum.Online)
            .ToListAsync();
    }
}