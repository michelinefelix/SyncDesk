using SyncDesk.Models.Enums;

namespace SyncDesk.Models.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public PlanoEnum Plano { get; set; } = PlanoEnum.Gratis;
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; }

    // Relacionamentos EF Core
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Department> Departments { get; set; } = new List<Department>();

    public Tenant() { }

    public Tenant(string razaoSocial, string cnpj, PlanoEnum plano = PlanoEnum.Gratis, bool ativo = true)
    {
        Id = Guid.NewGuid();
        RazaoSocial = razaoSocial;
        CNPJ = cnpj;
        Plano = plano;
        Ativo = ativo;
        DataCriacao = DateTime.UtcNow;
    }
}