using Microsoft.EntityFrameworkCore;
using SyncDesk.Data;
using SyncDesk.Models.Entities;
using SyncDesk.Services.Interfaces;

namespace SyncDesk.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly SyncDeskDbContext _context;

    public CustomerService(SyncDeskDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> ObterOuCriarAsync(Guid tenantId, string nome, string email, string telefoneWhatsApp)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && (c.Email == email || c.TelefoneWhatsApp == telefoneWhatsApp));

        if (customer is not null) return customer;

        customer = new Customer(tenantId, nome, email, telefoneWhatsApp);
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<Customer?> ObterPorIdAsync(Guid id)
    {
        return await _context.Customers.FindAsync(id);
    }
}