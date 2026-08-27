using SyncDesk.Models.Enums;

namespace SyncDesk.Models.Entities;

public class User
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public PerfilEnum Perfil { get; set; } = PerfilEnum.Agente;
    public StatusPresencaEnum StatusPresenca { get; set; } = StatusPresencaEnum.Offline;
    public int LimiteAtendimentosSimultaneos { get; set; } = 5;

    // Relacionamentos EF Core
    public Tenant? Tenant { get; set; }
    public Department? Department { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public User() { }

    public User(Guid tenantId, string nome, string email, string senhaHash, PerfilEnum perfil = PerfilEnum.Agente, Guid? departmentId = null, int limiteAtendimentosSimultaneos = 5)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        DepartmentId = departmentId;
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Perfil = perfil;

    }
}