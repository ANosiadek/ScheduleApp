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

    // Akcja wyœwietlaj¹ca formularz logowania u¿ytkownika
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // Akcja obs³uguj¹ca przes³anie formularza logowania u¿ytkownika
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

    // Akcja wyœwietlaj¹ca b³¹d
    public IActionResult Error()
    {
        return View();
    }

    // Akcja wyœwietlaj¹ca twórców
    public IActionResult Creators()
    {
        return View();
    }
}
