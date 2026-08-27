using SyncDesk.Models.Entities;

namespace SyncDesk.Models.ViewModels;

public class DashboardViewModel
{
    public Tenant Tenant { get; set; } = null!;
    public int TotalTickets { get; set; }
    public int TicketsAguardando { get; set; }
    public int TicketsEmAtendimento { get; set; }
    public int TotalDepartamentos { get; set; }
}