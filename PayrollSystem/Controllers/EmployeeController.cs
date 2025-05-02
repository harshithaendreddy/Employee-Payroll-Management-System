using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace PayrollSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        public EmployeeController(AppDbContext context) => _context = context;

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
        public IActionResult ViewPayslip()
        {
            int employeeId = HttpContext.Session.GetInt32("UserId").Value;  // Get employee ID from session

            // Retrieve the employee along with the corresponding payslip
            var employeeWithPayslip = (from emp in _context.Employees
                                       join pay in _context.Payslips on emp.Id equals pay.EmployeeId
                                       where emp.Id == employeeId
                                       select new
                                       {
                                           EmployeeId = emp.Id,
                                           Name = emp.Name,
                                           Username = emp.Username,
                                           Role = emp.Role,
                                           Salary = emp.Salary,
                                           NetSalary = pay.NetSalary
                                       }).FirstOrDefault();

            if (employeeWithPayslip == null)
            {
                return NotFound("Employee or Payslip not found");
            }

            // Pass the result to the view
            var model = new
            {
                employeeWithPayslip.EmployeeId,
                employeeWithPayslip.Name,
                employeeWithPayslip.Username,
                employeeWithPayslip.Role,
                employeeWithPayslip.Salary,
                employeeWithPayslip.NetSalary
            };

            return View(model);
        }

        public FileResult DownloadPayslip()
        {
            int employeeId = HttpContext.Session.GetInt32("UserId").Value;

            var payslip = _context.Payslips.FirstOrDefault(p => p.EmployeeId == employeeId);
            if (payslip == null)
            {
                throw new Exception("Payslip not found for the logged-in employee.");
            }

            using var stream = new MemoryStream();
            var doc = new Document();
            PdfWriter.GetInstance(doc, stream).CloseStream = false;
            doc.Open();
            doc.Add(new Paragraph("Payslip for Employee ID: " + employeeId));
            doc.Add(new Paragraph("Basic Salary: " + payslip.BasicSalary));
            doc.Add(new Paragraph("Days Absent: " + payslip.DaysAbsent));
            doc.Add(new Paragraph("Net Salary: " + payslip.NetSalary));
            doc.Close();
            stream.Position = 0;
            return File(stream, "application/pdf", "Payslip.pdf");
        }

    }

}
