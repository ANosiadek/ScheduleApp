using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

public class WorkEventsController : Controller
{
    private readonly FileDataService _dataService;

    public WorkEventsController(FileDataService dataService)
    {
        _dataService = dataService;
    }

    // Wyświetlanie wydarzeń dla zalogowanego użytkownika
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var events = await _dataService.GetEventsAsync();
        var userEvents = events.Where(e => e.UserId == userId).OrderBy(e => e.Start).ToList();
        return View(userEvents);
    }

    // Wyświetlanie formularza tworzenia wydarzenia przez zalogowanego użytkownika
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Akcja obsługująca formularz tworzenia wydarzenia przez zalogowanego użytkownika
    [HttpPost]
    public async Task<IActionResult> Create(string title, DateTime start, DateTime? end)
    {
        var userId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Error", new { message = "Użytkownik nie jest zalogowany!" });
        }

        var events = await _dataService.GetEventsAsync() ?? new List<WorkEvent>(); ;
        var newEvent = new WorkEvent
        {
            Id = events.Any() ? events.Max(e => e.Id) + 1 : 1,
            Title = title,
            Start = start,
            End = end,
            UserId = userId
        };

        events.Add(newEvent);
        await _dataService.SaveEventsAsync(events);

        return RedirectToAction("Index");
    }

    // Wyświetlanie formularza edycji wydarzenia przez zalogowanego użytkownika
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        var events = await _dataService.GetEventsAsync();
        var workEvent = events.FirstOrDefault(e => e.Id == id && e.UserId == userId);

        if (workEvent == null)
        {
            return NotFound();
        }

        return View(workEvent);
    }

    // Akcja obsługująca formularz edycji wydarzenia przez zalogowanego użytkownika
    [HttpPost]
    public async Task<IActionResult> Edit(int id, string title, DateTime start, DateTime? end)
    {
        var userId = HttpContext.Session.GetString("UserId");
        var events = await _dataService.GetEventsAsync();
        var workEvent = events.FirstOrDefault(e => e.Id == id && e.UserId == userId);

        if (workEvent == null)
        {
            return NotFound();
        }

        workEvent.Title = title;
        workEvent.Start = start;
        workEvent.End = end;

        await _dataService.SaveEventsAsync(events);

        return RedirectToAction("Index");
    }

    // Usuwanie wydarzenia przez zalogowanego użytkownika
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        var events = await _dataService.GetEventsAsync();
        var workEvent = events.FirstOrDefault(e => e.Id == id && e.UserId == userId);

        if (workEvent == null)
        {
            return NotFound();
        }

        events.Remove(workEvent);
        await _dataService.SaveEventsAsync(events);

        return RedirectToAction("Index");
    }
}
