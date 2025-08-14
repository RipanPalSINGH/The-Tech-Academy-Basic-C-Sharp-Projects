using System;

namespace EFCodeFirstStudentApp
{
    public class Student
    {
        public int StudentId { get; set; }  // Primary key by convention
        public string Name { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}

