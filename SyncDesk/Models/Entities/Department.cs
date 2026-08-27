namespace SyncDesk.Models.Entities;

public class Department
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    // Relacionamentos EF Core
    public Tenant? Tenant { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public Department() { }

    public Department(Guid tenantId, string nome, string descricao, bool ativo = true)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Nome = nome;
        Descricao = descricao;
        Ativo = ativo;
    }
}
