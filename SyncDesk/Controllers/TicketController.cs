using Microsoft.AspNetCore.Mvc;
using SyncDesk.Data;
using SyncDesk.Models.Enums;
using SyncDesk.Services.Interfaces;
using Ticket = SyncDesk.Models.Entities.Ticket;
using Microsoft.EntityFrameworkCore;

namespace SyncDesk.Controllers;

[Route("[controller]")]
public class TicketController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly IMessageService _messageService;
    private readonly IDepartmentService _departmentService;
    private readonly SyncDeskDbContext _context;

    private readonly Guid _tenantIdAtual = Guid.Parse("E33D55AC-A362-4E6B-93EC-8031DF11E3F0");

    public TicketController(ITicketService ticketService, IMessageService messageService, IDepartmentService departmentService, SyncDeskDbContext context)
    {
        _ticketService = ticketService;
        _messageService = messageService;
        _departmentService = departmentService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var chamados = await _context.Tickets
            .IgnoreQueryFilters() // Desativa o filtro de isolamento de tenant para listar tudo
            .Include(t => t.Customer)
            .Include(t => t.Department)
            .OrderByDescending(t => t.DataAbertura)
            .ToListAsync();

        return View(chamados);
    }

    // GET: /Ticket/Fila (Fila de Espera)
    [HttpGet("Fila")]
    public async Task<IActionResult> Fila()
    {
        var chamadosFila = await _context.Tickets
            .IgnoreQueryFilters()
            .Where(t => t.AgentId == null) // Sem agente atribuído
            .Include(t => t.Customer)
            .Include(t => t.Department)
            .OrderByDescending(t => t.DataAbertura)
            .ToListAsync();

        return View(chamadosFila);
    }

    // GET: /Ticket/Atender/{id}
    [HttpGet("Atender/{id}")]
    public async Task<IActionResult> Atender(Guid id)
    {
        var ticket = await _ticketService.ObterPorIdAsync(id);
        if (ticket is null) return NotFound();

        var mensagens = await _messageService.ListarPorTicketIdAsync(id);
        await _messageService.MarcarComoLidasAsync(id);

        ViewBag.Mensagens = mensagens;
        return View(ticket);
    }

    // POST: /Ticket/AtribuirAgente
    [HttpPost("AtribuirAgente")]
    public async Task<IActionResult> AtribuirAgente(Guid ticketId)
    {
        var agente = await _context.Users.FirstOrDefaultAsync();
        if (agente == null)
        {
            return BadRequest("Nenhum usuário/agente foi encontrado no banco de dados.");
        }

        await _ticketService.AtribuirAgenteAsync(ticketId, agente.Id);
        return RedirectToAction(nameof(Atender), new { id = ticketId });
    }

    // POST: /Ticket/Encerrar
    [HttpPost("Encerrar")]
    public async Task<IActionResult> Encerrar(Guid ticketId)
    {
        var sucesso = await _ticketService.EncerrarTicketAsync(ticketId);
        if (!sucesso) return BadRequest();

        return RedirectToAction(nameof(Index));
    }
}