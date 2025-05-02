namespace PayrollSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using PayrollSystem.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between Payslip and Employee
            modelBuilder.Entity<Payslip>()
                .HasOne(p => p.Employee)
                .WithMany()  // If you want to allow navigation from Employee to Payslips, you can use .WithMany(p => p.Payslips)
                .HasForeignKey(p => p.EmployeeId);  // Specify that EmployeeId is the foreign key
        }
    }


}
