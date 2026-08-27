using SyncDesk.Models.Entities;
using SyncDesk.Models.Enums;

namespace SyncDesk.Services.Interfaces;

public interface IMessageService
{
    Task<Message> EnviarMensagemAsync(Guid ticketId, Guid? remetenteId, TipoRemetenteEnum tipoRemetente, string conteudo, TipoMensagemEnum tipoMensagem = TipoMensagemEnum.Texto, string? urlAnexo = null);
    Task<List<Message>> ListarPorTicketIdAsync(Guid ticketId);
    Task MarcarComoLidasAsync(Guid ticketId);
}