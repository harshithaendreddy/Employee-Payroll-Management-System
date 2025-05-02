# 👨‍💼 Employee Management System - ASP.NET Core MVC

A simple web application built with **ASP.NET Core MVC** that allows admins to manage employees, track their salary and absence details, and generate/view employee payslips. Employees can log in to view their own dashboard and payslip.

---

## 🚀 Features

* Admin can:

  * Add new employees
  * Edit employee details
  * Automatically create/update payslips based on salary and absent days

* Employee can:

  * Log in using their credentials
  * View a personalized dashboard
  * View their payslip with net salary calculation

* Validations:

  * Unique username check
  * Strong password enforcement (min 8 chars, upper, lower, number, symbol)
  * Model state validation for all fields

---

## 💾 Technologies Used

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server / LocalDB
* Razor Views & Layouts
* Bootstrap (for styling)

---

## 📁 Project Structure

```
/Controllers
  - HomeController.cs
  - AdminController.cs
  - EmployeeController.cs

/Models
  - Employee.cs
  - Payslip.cs

/Views
  - Home/
    - Index.cshtml
  - Admin/
    - AddEmployee.cshtml
    - EditEmployee.cshtml
    - ManageEmployees.cshtml
  - Employee/
    - Dashboard.cshtml
    - ViewPayslip.cshtml

wwwroot/
  - css/
  - images/ (place background image here)

appsettings.json
Startup.cs / Program.cs
```

---

## 🧮 Payslip Calculation

Net Salary =
`Basic Salary - (Absent Days × Per-Day Salary)`
Per-day salary is calculated assuming **30 working days per month**.

---

## ✅ Strong Password Validation

Passwords must:

* Be at least 8 characters long
* Contain:

  * At least one uppercase letter (A–Z)
  * At least one lowercase letter (a–z)
  * At least one digit (0–9)
  * At least one special character (!, @, #, etc.)

---

## 🧪 Running the App

1. Clone the repo
   `git clone https://github.com/your-repo-name.git`

2. Open in **Visual Studio 2022+** or **VS Code**

3. Restore NuGet packages
   `dotnet restore`

4. Update your connection string in `appsettings.json`

5. Apply migrations (if using EF Core Code First)

   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

6. Run the app
   `dotnet run`

7. Visit `https://localhost:xxxx/`



