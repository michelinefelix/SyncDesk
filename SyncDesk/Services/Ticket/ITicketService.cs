using SyncDesk.Models.Entities;
using SyncDesk.Models.Enums;

namespace SyncDesk.Services.Interfaces;

public interface ITicketService
{
    Task<Ticket> CriarTicketAsync(Guid tenantId, Guid customerId, Guid departmentId, string assunto, CanalOrigemEnum canal = CanalOrigemEnum.WebChat, PrioridadeEnum prioridade = PrioridadeEnum.Media);
    Task<Ticket?> ObterPorIdAsync(Guid id);
    Task<Ticket?> ObterPorProtocoloAsync(string protocolo);
    Task<List<Ticket>> ListarPorTenantAsync(Guid tenantId, TicketStatusEnum? status = null);
    Task<List<Ticket>> ListarFilaAguardandoAsync(Guid tenantId, Guid? departmentId = null);
    Task<bool> AtribuirAgenteAsync(Guid ticketId, Guid agentId);
    Task<bool> EncerrarTicketAsync(Guid ticketId);
}