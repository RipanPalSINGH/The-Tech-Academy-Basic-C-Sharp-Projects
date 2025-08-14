using System.Data.Entity;

namespace EFCodeFirstStudentApp
{
    public class StudentContext : DbContext
    {
        public StudentContext() : base("StudentDbConnectionString") { }

        public DbSet<Student> Students { get; set; }
    }
}
