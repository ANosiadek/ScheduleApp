using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly FileDataService _fileDataService;

    public HomeController(FileDataService fileDataService)
    {
        _fileDataService = fileDataService;
    }

    // Akcja wy�wietlaj�ca formularz logowania u�ytkownika
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // Akcja obs�uguj�ca przes�anie formularza logowania u�ytkownika
    [HttpPost]
    public async Task<IActionResult> Index(string username, string password)
    {
        var users = await _fileDataService.GetUsersAsync();
        var user = users.FirstOrDefault(u => string.Equals(u.UserName, username, StringComparison.OrdinalIgnoreCase) &&
                                             u.Password == password);

        if (user != null)
        {
            HttpContext.Session.SetString("UserId", user.Id);
            return RedirectToAction("Schedule", "Account");
        }

        ViewBag.ErrorMessage = "Niepoprawne dane logowania!";
        return View();
    }

    // Akcja wy�wietlaj�ca b��d
    public IActionResult Error()
    {
        return View();
    }

    // Akcja wy�wietlaj�ca tw�rc�w
    public IActionResult Creators()
    {
        return View();
    }
}
