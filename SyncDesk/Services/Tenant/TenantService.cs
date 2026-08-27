using Microsoft.EntityFrameworkCore;
using SyncDesk.Data;
using SyncDesk.Models.Entities;
using SyncDesk.Models.Enums;
using SyncDesk.Services.Interfaces;

namespace SyncDesk.Services.Implementations;

public class TenantService : ITenantService
{
    private readonly SyncDeskDbContext _context;

    public TenantService(SyncDeskDbContext context)
    {
        _context = context;
    }

    public async Task<Tenant> CriarTenantAsync(string nomeRazaoSocial, string cnpj, PlanoEnum plano = PlanoEnum.Gratis)
    {
        var tenant = new Tenant(nomeRazaoSocial, cnpj, plano);
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        return tenant;
    }

    public async Task<Tenant?> ObterPorIdAsync(Guid id)
    {
        return await _context.Tenants
            .Include(t => t.Departments)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Tenant>> ListarTodosAsync()
    {
        return await _context.Tenants.ToListAsync();
    }

    public async Task<bool> AtualizarStatusAsync(Guid id, bool ativo)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant is null) return false;

        tenant.Ativo = ativo;
        await _context.SaveChangesAsync();
        return true;
    }
}