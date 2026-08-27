using SyncDesk.Models.Enums;

namespace SyncDesk.Models.Entities;

public class Ticket
{
    public Guid Id { get; set; }
    public string Protocolo { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid? AgentId { get; set; }
    public string Assunto { get; set; } = string.Empty;
    public TicketStatusEnum Status { get; set; } = TicketStatusEnum.AguardandoFila;
    public PrioridadeEnum Prioridade { get; set; } = PrioridadeEnum.Media;
    public CanalOrigemEnum CanalOrigem { get; set; } = CanalOrigemEnum.WebChat;
    public DateTime DataAbertura { get; set; }
    public DateTime? DataFechamento { get; set; }

    // Relacionamentos EF Core
    public Tenant? Tenant { get; set; }
    public Customer? Customer { get; set; }
    public Department? Department { get; set; }
    public User? Agent { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public Ticket() { }

    public Ticket(Guid tenantId, Guid customerId, Guid departmentId, string assunto, CanalOrigemEnum canalOrigem = CanalOrigemEnum.WebChat, PrioridadeEnum prioridade = PrioridadeEnum.Media)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        CustomerId = customerId;
        DepartmentId = departmentId;
        Assunto = assunto;
        CanalOrigem = canalOrigem;
        Prioridade = prioridade;
        Protocolo = $"{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
        DataAbertura = DateTime.UtcNow;
    }
}