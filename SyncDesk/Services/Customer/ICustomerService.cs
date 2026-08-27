using SyncDesk.Models.Entities;

namespace SyncDesk.Services.Interfaces;

public interface ICustomerService
{
    Task<Customer> ObterOuCriarAsync(Guid tenantId, string nome, string email, string telefoneWhatsApp);
    Task<Customer?> ObterPorIdAsync(Guid id);
}