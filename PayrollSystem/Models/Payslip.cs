public class Payslip
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal NetSalary { get; set; }
    public decimal Salary { get; set; }
    public int DaysAbsent { get; set; }  // Add this property
    public Employee Employee { get; set; }
}
