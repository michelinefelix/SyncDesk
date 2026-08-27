using SyncDesk.Models.Enums;

namespace SyncDesk.Models.Entities;

public class Message
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid? RemetenteId { get; set; }
    public TipoRemetenteEnum TipoRemetente { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public TipoMensagemEnum TipoMensagem { get; set; } = TipoMensagemEnum.Texto;
    public string? UrlAnexo { get; set; }
    public DateTime DataEnvio { get; set; }
    public bool Lida { get; set; }

    // Relacionamentos EF Core
    public Ticket? Ticket { get; set; }

    public Message() { }

    public Message(Guid ticketId, Guid? remetenteId, TipoRemetenteEnum tipoRemetente, string conteudo, TipoMensagemEnum tipoMensagem = TipoMensagemEnum.Texto, string? urlAnexo = null)
    {
        Id = Guid.NewGuid();
        TicketId = ticketId;
        RemetenteId = remetenteId;
        TipoRemetente = tipoRemetente;
        Conteudo = conteudo;
        TipoMensagem = tipoMensagem;
        UrlAnexo = urlAnexo;
        DataEnvio = DateTime.UtcNow;
        Lida = false;
    }
}