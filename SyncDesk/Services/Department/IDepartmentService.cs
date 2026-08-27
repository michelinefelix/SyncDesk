using SyncDesk.Models.Entities;

namespace SyncDesk.Services.Interfaces;

public interface IDepartmentService
{
    Task<Department> CriarDepartmentAsync(Guid tenantId, string nome, string descricao);
    Task<Department?> ObterPorIdAsync(Guid id);
    Task<List<Department>> ListarPorTenantAsync(Guid tenantId, bool apenasAtivos = true);
    Task<bool> AtualizarStatusAsync(Guid id, bool ativo);
}