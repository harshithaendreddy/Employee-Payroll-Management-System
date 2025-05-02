using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Data;

namespace PayrollSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "Admin@123")
            {
                // Admin login (hardcoded)
                HttpContext.Session.SetString("Username", "Admin");
                HttpContext.Session.SetString("Role", "Admin");
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                var user = _context.Employees.FirstOrDefault(e => e.Username == username && e.Password == password);

                if (user != null && user.Role == "Employee")
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    return RedirectToAction("Dashboard", "Employee");
                }
                else
                {
                    ViewBag.Error = "Invalid credentials";
                    return View();
                }
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

}
