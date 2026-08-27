using Microsoft.EntityFrameworkCore;
using SyncDesk.Models;
using SyncDesk.Models.Entities; // Importa as entidades Tenant, Department, Customer e User
using SyncDesk.Models.Enums;

namespace SyncDesk.Data;

public static class DbInitializer
{
    private static readonly Guid TenantIdFixo = Guid.Parse("E33D55AC-A362-4E6B-93EC-8031DF11E3F0");

    public static async Task SeedAsync(SyncDeskDbContext context)
    {
        // 1. Tenant principal
        var tenant = await context.Tenants.FirstOrDefaultAsync(t => t.Id == TenantIdFixo);
        if (tenant == null)
        {
            tenant = new Tenant
            {
                Id = TenantIdFixo,
                RazaoSocial = "Empresa Principal",
                CNPJ = "00.000.000/0001-00",
                Plano = (PlanoEnum)1,
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            };
            context.Tenants.Add(tenant);
            await context.SaveChangesAsync();
        }

        // 2. Departamento
        var departamento = await context.Departments
        .FirstOrDefaultAsync(d => d.Nome == "Suporte Técnico" && d.TenantId == tenant.Id);

        if (departamento == null)
        {
            departamento = new Department
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                Nome = "Suporte Técnico",
                Descricao = "Atendimento Geral",
                Ativo = true
            };
            context.Departments.Add(departamento);
            await context.SaveChangesAsync();
        }

        // 3. Cliente
        var cliente = await context.Customers.FirstOrDefaultAsync(c => c.TenantId == tenant.Id);
        if (cliente == null)
        {
            cliente = new Customer
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                Nome = "Cliente Teste",
                Email = "cliente@teste.com",
                TelefoneWhatsApp = "11999999999",
                DataCadastro = DateTime.UtcNow
            };
            context.Customers.Add(cliente);
            await context.SaveChangesAsync();
        }

        // 4. Usuário Agente
        var agente = await context.Users.FirstOrDefaultAsync(u => u.TenantId == tenant.Id);
        if (agente == null)
        {
            agente = new User
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                DepartmentId = departamento.Id,
                Nome = "Agente Suporte",
                Email = "suporte@syncdesk.com",
                SenhaHash = "123456",
                Perfil = PerfilEnum.Agente,
                StatusPresenca = (StatusPresencaEnum)1,
                LimiteAtendimentosSimultaneos = 5
            };
            context.Users.Add(agente);
            await context.SaveChangesAsync();
        }
    }
}