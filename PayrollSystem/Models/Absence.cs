namespace PayrollSystem.Models
{
    public class Absence
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DaysAbsent { get; set; }
        public User User { get; set; }
    }

}
