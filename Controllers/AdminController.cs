using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class AdminController : Controller
{
    private readonly FileDataService _fileDataService;
    private const string AdminPassword = "admin123";

    public AdminController(FileDataService fileDataService)
    {
        _fileDataService = fileDataService;
    }

    // Akcja wyświetlająca formularz logowania admina
    public IActionResult Login_admin()
    {
        return View();
    }

    // Akcja obsługująca przesłanie formularza logowania admina
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login_admin(string password)
    {
        if (password == AdminPassword)
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            return RedirectToAction("Panel_admin");
        }
        else
        {
            ViewBag.ErrorMessage = "Niepoprawne hasło!";
            return View();
        }
    }

    // Akcja wyświetlająca panel administracyjny
    public async Task<IActionResult> Panel_admin()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToAction("Login_admin");
        }

        var users = await _fileDataService.GetUsersAsync();
        var events = await _fileDataService.GetEventsAsync();
        var sortedEvents = events.OrderBy(e => e.Start).ToList();

        var model = new AdminViewModel
        {
            Users = users,
            Events = sortedEvents
        };

        return View(model);
    }

    // Akcja usuwająca użytkownika
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToAction("Login_admin");
        }

        var events = await _fileDataService.GetEventsAsync();
        var userEvents = events.Where(e => e.UserId == userId).ToList();

        if (userEvents.Any())
        {
            TempData["ErrorMessage"] = "Nie można usunąć użytkownika gdyż posiada aktywne wydarzenia!";
            TempData["UserId"] = userId;
            return RedirectToAction("Panel_admin");
        }

        var users = await _fileDataService.GetUsersAsync();
        var userToRemove = users.FirstOrDefault(u => u.Id == userId);
        if (userToRemove != null)
        {
            users.Remove(userToRemove);
            await _fileDataService.SaveUsersAsync(users);
        }
        return RedirectToAction("Panel_admin");
    }

    // Akcja wyświetlająca formularz edycji wydarzenia
    public async Task<IActionResult> EditEvent(int Id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToAction("Login_admin");
        }

        var events = await _fileDataService.GetEventsAsync();
        var workEvent = events.FirstOrDefault(e => e.Id == Id);
        if (workEvent == null)
        {
            return NotFound();
        }

        return View(workEvent);
    }

    // Akcja aktualizująca wydarzenie
    [HttpPost]
    public async Task<IActionResult> EditEvent(WorkEvent workEvent)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToAction("Login_admin");
        }

        var events = await _fileDataService.GetEventsAsync();
        var eventToUpdate = events.FirstOrDefault(e => e.Id == workEvent.Id);
        if (eventToUpdate != null)
        {
            eventToUpdate.Title = workEvent.Title;
            eventToUpdate.Start = workEvent.Start;
            eventToUpdate.End = workEvent.End;
            await _fileDataService.SaveEventsAsync(events);
        }
        return RedirectToAction("Panel_admin");
    }

    // Akcja usuwająca wydarzenie
    [HttpPost]
    public async Task<IActionResult> DeleteEvent(int eventId)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToAction("Login_admin");
        }

        await _fileDataService.DeleteEventAsync(eventId);
        return RedirectToAction("Panel_admin");
    }

    // Akcja wyświetlająca formularz zmiany hasła użytkownika
    public async Task<IActionResult> ChangePassword(string userId)
    {
        var user = (await _fileDataService.GetUsersAsync()).FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return NotFound();
        }

        var model = new ChangePasswordViewModel
        {
            UserId = userId
        };

        return View(model);
    }

    // Akcja zmieniająca hasło wybranego użytkownika
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var users = await _fileDataService.GetUsersAsync();
        var user = users.FirstOrDefault(u => u.Id == model.UserId);
        if (user != null)
        {
            user.Password = model.NewPassword;
            await _fileDataService.SaveUsersAsync(users);
            TempData["SuccessMessage"] = "Pomyślnie zmieniono hasło użytkownika.";
        }
        else
        {
            TempData["ErrorMessage"] = "Nie znaleziono użytkownika!";
        }

        return RedirectToAction("Panel_admin");
    }

    // Wylogowanie admina
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login_admin", "Admin");
    }
}
