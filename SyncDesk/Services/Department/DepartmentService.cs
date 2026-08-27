using Microsoft.EntityFrameworkCore;
using SyncDesk.Data;
using SyncDesk.Models.Entities;
using SyncDesk.Services.Interfaces;

namespace SyncDesk.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly SyncDeskDbContext _context;

    public DepartmentService(SyncDeskDbContext context)
    {
        _context = context;
    }

    public async Task<Department> CriarDepartmentAsync(Guid tenantId, string nome, string descricao)
    {
        var department = new Department(tenantId, nome, descricao);
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return department;
    }

    public async Task<Department?> ObterPorIdAsync(Guid id)
    {
        return await _context.Departments
            .Include(d => d.Users)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<List<Department>> ListarPorTenantAsync(Guid tenantId, bool apenasAtivos = true)
    {
        var query = _context.Departments.Where(d => d.TenantId == tenantId);

        if (apenasAtivos)
            query = query.Where(d => d.Ativo);

        return await query.ToListAsync();
    }

    public async Task<bool> AtualizarStatusAsync(Guid id, bool ativo)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department is null) return false;

        department.Ativo = ativo;
        await _context.SaveChangesAsync();
        return true;
    }
}
