using SyncDesk.Models.Entities;
using SyncDesk.Models.Enums;

namespace SyncDesk.Services.Interfaces;

public interface ITenantService
{
    Task<Tenant> CriarTenantAsync(string nomeRazaoSocial, string cnpj, PlanoEnum plano = PlanoEnum.Gratis);
    Task<Tenant?> ObterPorIdAsync(Guid id);
    Task<List<Tenant>> ListarTodosAsync();
    Task<bool> AtualizarStatusAsync(Guid id, bool ativo);
}