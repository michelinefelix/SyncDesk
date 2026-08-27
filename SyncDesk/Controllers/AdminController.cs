using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyncDesk.Data;
using SyncDesk.Models.Enums;
using SyncDesk.Models.ViewModels;
using SyncDesk.Services.Interfaces;

namespace SyncDesk.Controllers;

[Route("[controller]")]
public class AdminController : Controller
{
    private readonly ITenantService _tenantService;
    private readonly IDepartmentService _departmentService;
    private readonly IUserService _userService;
    private readonly ITicketService _ticketService;
    private readonly SyncDeskDbContext _context;

    private readonly Guid _tenantIdAtual = Guid.Parse("E33D55AC-A362-4E6B-93EC-8031DF11E3F0");

    public AdminController(
        ITenantService tenantService,
        IDepartmentService departmentService,
        IUserService userService,
        ITicketService ticketService,
        SyncDeskDbContext context)
    {
        _tenantService = tenantService;
        _departmentService = departmentService;
        _userService = userService;
        _ticketService = ticketService;
        _context = context;
    }

    // GET: /Admin/Dashboard
    [HttpGet("Dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var tenant = await _tenantService.ObterPorIdAsync(_tenantIdAtual);
        var tickets = await _ticketService.ListarPorTenantAsync(_tenantIdAtual);
        var departamentos = await _departmentService.ListarPorTenantAsync(_tenantIdAtual);

        var viewModel = new DashboardViewModel
        {
            Tenant = tenant!,
            TotalTickets = tickets.Count,
            TicketsAguardando = tickets.Count(t => t.Status == TicketStatusEnum.AguardandoFila),
            TicketsEmAtendimento = tickets.Count(t => t.Status == TicketStatusEnum.EmAtendimento),
            TotalDepartamentos = departamentos.Count
        };

        return View(viewModel);
    }

    // GET: /Admin/Departamentos
    [HttpGet("Departamentos")]
    public async Task<IActionResult> Departamentos()
    {
        var departamentos = await _context.Departments
            .IgnoreQueryFilters()
            .ToListAsync();

        return View(departamentos);
    }

    // POST: /Admin/CriarDepartamento
    [HttpPost("CriarDepartamento")]
    public async Task<IActionResult> CriarDepartamento(string nome, string descricao)
    {
        if (string.IsNullOrWhiteSpace(nome)) return BadRequest("O nome do departamento é obrigatório.");

        await _departmentService.CriarDepartmentAsync(_tenantIdAtual, nome, descricao);
        return RedirectToAction(nameof(Departamentos));
    }

    // POST: /Admin/AlternarStatusDepartamento
    [HttpPost("AlternarStatusDepartamento")]
    public async Task<IActionResult> AlternarStatusDepartamento(Guid id, bool ativo)
    {
        await _departmentService.AtualizarStatusAsync(id, ativo);
        return RedirectToAction(nameof(Departamentos));
    }
}