using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SyncDesk.Data;
using SyncDesk.Models.Enums;
using SyncDesk.Services.Interfaces;

namespace SyncDesk.Controllers;

[Route("[controller]")]
public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly ITicketService _ticketService;
    private readonly IDepartmentService _departmentService;
    private readonly IMessageService _messageService;
    private readonly SyncDeskDbContext _context; // Injeção do DbContext

    // Atualize o GUID fixo para o mesmo GUID que está gravado no seu banco:
    private readonly Guid _tenantIdAtual = Guid.Parse("E33D55AC-A362-4E6B-93EC-8031DF11E3F0");

    public CustomerController(
        ICustomerService customerService,
        ITicketService ticketService,
        IDepartmentService departmentService,
        IMessageService messageService,
        SyncDeskDbContext context)
    {
        _customerService = customerService;
        _ticketService = ticketService;
        _departmentService = departmentService;
        _messageService = messageService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var departamentos = await _context.Departments
            .IgnoreQueryFilters()
            .Where(d => d.Ativo)
            .ToListAsync();

        ViewBag.Departments = departamentos.Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = d.Nome
        }).ToList();

        return View();
    }

    [HttpGet("Detalhes/{id}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var customer = await _customerService.ObterPorIdAsync(id);
        if (customer is null) return NotFound();

        return View(customer);
    }

    [HttpPost("CriarOuObter")]
    public async Task<IActionResult> CriarOuObter(string nome, string email, string telefoneWhatsApp)
    {
        var customer = await _customerService.ObterOuCriarAsync(_tenantIdAtual, nome, email, telefoneWhatsApp);
        return Json(customer);
    }

    [HttpPost("AbrirTicket")]
    public async Task<IActionResult> AbrirTicket(string nome, string email, string whatsapp, Guid departmentId, string assunto, string primeiraMensagem)
    {
        var customer = await _customerService.ObterOuCriarAsync(_tenantIdAtual, nome, email, whatsapp);
        var ticket = await _ticketService.CriarTicketAsync(_tenantIdAtual, customer.Id, departmentId, assunto);

        if (!string.IsNullOrWhiteSpace(primeiraMensagem))
        {
            await _messageService.EnviarMensagemAsync(ticket.Id, customer.Id, TipoRemetenteEnum.Cliente, primeiraMensagem);
        }

        return RedirectToAction(nameof(Chat), new { id = ticket.Id, customerId = customer.Id });
    }

    [HttpGet("Chat/{id}")]
    public async Task<IActionResult> Chat(Guid id, Guid customerId)
    {
        var ticket = await _ticketService.ObterPorIdAsync(id);
        if (ticket == null) return NotFound();

        ViewBag.CustomerId = customerId;
        ViewBag.Mensagens = await _messageService.ListarPorTicketIdAsync(id);

        return View(ticket);
    }
}