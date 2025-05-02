using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Data;
using PayrollSystem.Models;

namespace PayrollSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // 1. View All Employees
        // GET: Admin/ManageEmployees
        public IActionResult ManageEmployees()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }


        // 2. Edit Employee Info
        public IActionResult EditEmployee(int id)
        {
            var emp = _context.Employees.Find(id);
            return View(emp);
        }
        [HttpPost]
        public IActionResult EditEmployee(Employee emp)
        {
            if (ModelState.IsValid)
            {
                // Check if the username is taken by another employee
                var usernameExists = _context.Employees
                    .Any(e => e.Username == emp.Username && e.Id != emp.Id);

                if (usernameExists)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(emp);
                }

                // Validate password strength only if the user provided a new one
                if (!string.IsNullOrEmpty(emp.Password) && !IsStrongPassword(emp.Password))
                {
                    ModelState.AddModelError("Password", "Password must be at least 8 characters long, contain an uppercase letter, a lowercase letter, a digit, and a special character.");
                    return View(emp);
                }

                var existingEmployee = _context.Employees.FirstOrDefault(e => e.Id == emp.Id);
                if (existingEmployee != null)
                {
                    existingEmployee.Name = emp.Name;
                    existingEmployee.Username = emp.Username;
                    existingEmployee.Password = string.IsNullOrEmpty(emp.Password)
                        ? existingEmployee.Password
                        : emp.Password; // Save only if changed

                    existingEmployee.Role = emp.Role;
                    existingEmployee.Salary = emp.Salary;
                    existingEmployee.AbsentDays = emp.AbsentDays;

                    _context.Employees.Update(existingEmployee);
                    _context.SaveChanges();
                }

                // Update or create payslip
                var payslip = _context.Payslips.FirstOrDefault(p => p.EmployeeId == emp.Id);

                int totalWorkingDays = 30;
                decimal perDaySalary = emp.Salary / totalWorkingDays;
                decimal netSalary = emp.Salary - (emp.AbsentDays * perDaySalary);

                if (payslip != null)
                {
                    payslip.BasicSalary = emp.Salary;
                    payslip.DaysAbsent = emp.AbsentDays;
                    payslip.NetSalary = netSalary;
                    payslip.Salary = emp.Salary;
                }
                else
                {
                    _context.Payslips.Add(new Payslip
                    {
                        EmployeeId = emp.Id,
                        BasicSalary = emp.Salary,
                        DaysAbsent = emp.AbsentDays,
                        NetSalary = netSalary,
                        Salary = emp.Salary
                    });
                }

                _context.SaveChanges();
                return RedirectToAction("ManageEmployees");
            }

            return View(emp); // Show validation errors
        }


        // 3. Manage Absentees (Same employee list but focuses on AbsentDays)
        public IActionResult ManageAbsentees()
        {
            var employees = _context.Employees.Where(e => e.Role == "Employee").ToList();
            return View(employees);
        }

        public IActionResult UpdateAbsent(int id)
        {
            var emp = _context.Employees.Find(id);
            return View(emp);
        }

        [HttpPost]
        public IActionResult UpdateAbsent(Employee emp)
        {
            var existing = _context.Employees.Find(emp.Id);
            existing.AbsentDays = emp.AbsentDays;
            _context.SaveChanges();
            return RedirectToAction("ManageAbsentees");
        }
        [HttpGet]
        // GET: Admin/AddEmployee
        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        // POST: Admin/AddEmployee
        [HttpPost]
        public IActionResult AddEmployee(Employee emp)
        {
            if (!ModelState.IsValid)
                return View(emp);

            // 1. Check for unique username
            var existingUser = _context.Employees.FirstOrDefault(e => e.Username == emp.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(emp);
            }

            // 2. Validate strong password
            if (!IsStrongPassword(emp.Password))
            {
                ModelState.AddModelError("Password", "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.");
                return View(emp);
            }

            // 3. Set default role
            emp.Role = "Employee";
            _context.Employees.Add(emp);
            _context.SaveChanges(); // Get emp.Id

            // 4. Generate payslip
            int totalWorkingDays = 30;
            decimal perDaySalary = emp.Salary / totalWorkingDays;
            decimal netSalary = emp.Salary - (emp.AbsentDays * perDaySalary);

            var payslip = new Payslip
            {
                EmployeeId = emp.Id,
                BasicSalary = emp.Salary,
                DaysAbsent = emp.AbsentDays,
                NetSalary = netSalary,
                Salary = emp.Salary
            };

            _context.Payslips.Add(payslip);
            _context.SaveChanges();

            return RedirectToAction("ManageEmployees");
        }

        // Helper method to validate password
        private bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            return
                password.Any(char.IsUpper) &&
                password.Any(char.IsLower) &&
                password.Any(char.IsDigit) &&
                password.Any(ch => !char.IsLetterOrDigit(ch));
        }





    }

}
