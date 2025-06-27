using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly FileDataService _fileDataService;

    public AccountController(FileDataService fileDataService)
    {
        _fileDataService = fileDataService;
    }

    // Akcja wyświetlająca formularz rejestracji
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // Akcja obsługująca przesłanie formularza rejestracji
    [HttpPost]
    public async Task<IActionResult> Register(string username, string password)
    {
        if (username.Equals("administrator", StringComparison.OrdinalIgnoreCase))
        {
            ViewBag.ErrorMessage = "Ta nazwa użytkownika jest zarezerwowana!";
            return View();
        }

        var users = await _fileDataService.GetUsersAsync();
        if (users.Any(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            ViewBag.ErrorMessage = "Nazwa użytkownika już istnieje!";
            return View();
        }

        var newUser = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = username,
            Password = password,
            IsAdmin = false
        };

        users.Add(newUser);
        await _fileDataService.SaveUsersAsync(users);

        HttpContext.Session.SetString("UserId", newUser.Id);
        return RedirectToAction("Schedule");
    }

    public IActionResult Schedule()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (userId == null)
        {
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Index", "WorkEvents");
    }
    
    // Wylogowanie użytkownika
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
