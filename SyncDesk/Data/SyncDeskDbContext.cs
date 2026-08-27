using Microsoft.EntityFrameworkCore;
using SyncDesk.Models.Entities;

namespace SyncDesk.Data;

public class SyncDeskDbContext : DbContext
{
    public SyncDeskDbContext(DbContextOptions<SyncDeskDbContext> options) : base(options) { }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Mapeamento Tenant
        modelBuilder.Entity<Tenant>(builder =>
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.RazaoSocial)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(t => t.CNPJ)
                .IsRequired()
                .HasMaxLength(18);
        });

        // 2. Mapeamento Department
        modelBuilder.Entity<Department>(builder =>
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Descricao)
                .HasMaxLength(250);

            builder.HasOne(d => d.Tenant)
                .WithMany(t => t.Departments)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 3. Mapeamento User
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(u => u.Email);

            builder.HasOne(u => u.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // 4. Mapeamento Customer
        modelBuilder.Entity<Customer>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Email)
                .HasMaxLength(150);

            builder.Property(c => c.TelefoneWhatsApp)
                .HasMaxLength(20);

            builder.HasOne(c => c.Tenant)
                .WithMany()
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 5. Mapeamento Ticket
        modelBuilder.Entity<Ticket>(builder =>
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Protocolo)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(t => t.Protocolo)
                .IsUnique();

            builder.Property(t => t.Assunto)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasOne(t => t.Tenant)
                .WithMany()
                .HasForeignKey(t => t.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Customer)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Department)
                .WithMany(d => d.Tickets)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Agent)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.AgentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 6. Mapeamento Message
        modelBuilder.Entity<Message>(builder =>
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Conteudo)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(m => m.UrlAnexo)
                .HasMaxLength(500);

            builder.HasOne(m => m.Ticket)
                .WithMany(t => t.Messages)
                .HasForeignKey(m => m.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}