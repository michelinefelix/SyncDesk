using SyncDesk.Models.Entities;
using SyncDesk.Models.Enums;

namespace SyncDesk.Services.Interfaces;

public interface IUserService
{
    Task<User?> ObterPorIdAsync(Guid id);
    Task AtualizarStatusPresencaAsync(Guid userId, StatusPresencaEnum status);
    Task<List<User>> ObterAgentesDisponiveisAsync(Guid tenantId, Guid departmentId);
}