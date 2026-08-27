using Microsoft.EntityFrameworkCore;
using SyncDesk.Data;
using SyncDesk.Models.Entities;
using SyncDesk.Models.Enums;
using SyncDesk.Services.Interfaces;

namespace SyncDesk.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly SyncDeskDbContext _context;

    public MessageService(SyncDeskDbContext context)
    {
        _context = context;
    }

    public async Task<Message> EnviarMensagemAsync(Guid ticketId, Guid? remetenteId, TipoRemetenteEnum tipoRemetente, string conteudo, TipoMensagemEnum tipoMensagem = TipoMensagemEnum.Texto, string? urlAnexo = null)
    {
        var mensagem = new Message(ticketId, remetenteId, tipoRemetente, conteudo, tipoMensagem, urlAnexo);
        _context.Messages.Add(mensagem);

        await _context.SaveChangesAsync();
        return mensagem;
    }

    public async Task<List<Message>> ListarPorTicketIdAsync(Guid ticketId)
    {
        return await _context.Messages
            .Where(m => m.TicketId == ticketId)
            .OrderBy(m => m.DataEnvio)
            .ToListAsync();
    }

    public async Task MarcarComoLidasAsync(Guid ticketId)
    {
        var mensagensNaoLidas = await _context.Messages
            .Where(m => m.TicketId == ticketId && !m.Lida)
            .ToListAsync();

        foreach (var msg in mensagensNaoLidas)
        {
            msg.Lida = true;
        }

        if (mensagensNaoLidas.Any())
            await _context.SaveChangesAsync();
    }
}