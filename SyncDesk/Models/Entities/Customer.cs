namespace SyncDesk.Models.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TelefoneWhatsApp { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }

    // Relacionamentos EF Core
    public Tenant? Tenant { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public Customer() { }

    public Customer(Guid tenantId, string nome, string email, string telefoneWhatsApp)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Nome = nome;
        Email = email;
        TelefoneWhatsApp = telefoneWhatsApp;
        DataCadastro = DateTime.UtcNow;
    }
}