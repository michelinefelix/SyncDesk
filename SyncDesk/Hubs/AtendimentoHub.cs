using Microsoft.AspNetCore.SignalR;
using SyncDesk.Models.Enums;
using SyncDesk.Services.Interfaces;

namespace SyncDesk.Hubs;

public class AtendimentoHub : Hub
{
    private readonly IMessageService _messageService;

    public AtendimentoHub(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task EntrarNaSalaTicket(string ticketId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, ticketId);
    }

    public async Task SairDaSalaTicket(string ticketId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, ticketId);
    }

    public async Task EnviarMensagemAoVivo(string ticketIdStr, string remetenteIdStr, int tipoRemetenteInt, string conteudo)
    {
        if (Guid.TryParse(ticketIdStr, out var ticketId))
        {
            var tipoRemetente = (TipoRemetenteEnum)tipoRemetenteInt;
            Guid? remetenteId = Guid.TryParse(remetenteIdStr, out var g) ? g : null;

            // 1. Grava no SQL Server
            await _messageService.EnviarMensagemAsync(ticketId, remetenteId, tipoRemetente, conteudo);

            // 2. Transmite para os demais participantes da sala
            await Clients.OthersInGroup(ticketIdStr).SendAsync("ReceberMensagem", remetenteIdStr, conteudo, DateTime.UtcNow.ToString("HH:mm"));
        }
    }
}