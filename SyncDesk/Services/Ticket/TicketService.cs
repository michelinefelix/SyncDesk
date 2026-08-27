using Microsoft.EntityFrameworkCore;
using SyncDesk.Data;
using SyncDesk.Models.Entities;
using SyncDesk.Models.Enums;
using SyncDesk.Services.Interfaces;

namespace SyncDesk.Services.Implementations;

public class TicketService : ITicketService
{
    private readonly SyncDeskDbContext _context;

    public TicketService(SyncDeskDbContext context)
    {
        _context = context;
    }

    public async Task<Ticket> CriarTicketAsync(Guid tenantId, Guid customerId, Guid departmentId, string assunto, CanalOrigemEnum canal = CanalOrigemEnum.WebChat, PrioridadeEnum prioridade = PrioridadeEnum.Media)
    {
        var ticket = new Ticket(tenantId, customerId, departmentId, assunto, canal, prioridade);
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return ticket;
    }

    public async Task<Ticket?> ObterPorIdAsync(Guid id)
    {
        return await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Department)
            .Include(t => t.Agent)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Ticket?> ObterPorProtocoloAsync(string protocolo)
    {
        return await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Department)
            .FirstOrDefaultAsync(t => t.Protocolo == protocolo);
    }

    public async Task<List<Ticket>> ListarPorTenantAsync(Guid tenantId, TicketStatusEnum? status = null)
    {
        var query = _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Department)
            .Include(t => t.Agent)
            .Where(t => t.TenantId == tenantId);

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        return await query.OrderByDescending(t => t.DataAbertura).ToListAsync();
    }

    public async Task<List<Ticket>> ListarFilaAguardandoAsync(Guid tenantId, Guid? departmentId = null)
    {
        var query = _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Department)
            .Where(t => t.TenantId == tenantId && t.Status == TicketStatusEnum.AguardandoFila);

        if (departmentId.HasValue)
            query = query.Where(t => t.DepartmentId == departmentId.Value);

        return await query.OrderBy(t => t.DataAbertura).ToListAsync();
    }

    public async Task<bool> AtribuirAgenteAsync(Guid ticketId, Guid agentId)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket is null) return false;

        ticket.AgentId = agentId;
        ticket.Status = TicketStatusEnum.EmAtendimento;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EncerrarTicketAsync(Guid ticketId)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket is null) return false;

        ticket.Status = TicketStatusEnum.Encerrado;
        ticket.DataFechamento = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}