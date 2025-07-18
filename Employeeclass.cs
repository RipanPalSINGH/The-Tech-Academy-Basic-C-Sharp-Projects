 using System;

namespace EmployeeEqualityApp
{
    // Employee class represents an employee with Id, FirstName, and LastName.
    public class Employee
    {
        // Unique identifier for the employee
        public int Id { get; set; }

        // First name of the employee
        public string FirstName { get; set; }

        // Last name of the employee
        public string LastName { get; set; }

        // Overload '==' operator to compare two Employee objects by Id
        public static bool operator ==(Employee emp1, Employee emp2)
        {
            // If both are null, they are equal
            if (ReferenceEquals(emp1, emp2))
                return true;

            // If either is null, they are not equal
            if (emp1 is null || emp2 is null)
                return false;

            // Compare employee IDs
            return emp1.Id == emp2.Id;
        }

        // Overload '!=' operator to be the opposite of '=='
        public static bool operator !=(Employee emp1, Employee emp2)
        {
            return !(emp1 == emp2);
        }

        // Override Equals to keep consistent behavior with '=='
        public override bool Equals(object obj)
        {
            if (obj is Employee other)
                return this == other;

            return false;
        }

        // Override GetHashCode whenever Equals is overridden
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    // Program class contains the Main method - entry point of the application
    class Program
    {
        static void Main(string[] args)
        {
            // Create two Employee objects with the same Id but different names
            Employee emp1 = new Employee { Id = 1, FirstName = "Alice", LastName = "Smith" };
            Employee emp2 = new Employee { Id = 1, FirstName = "Alicia", LastName = "Smythe" };

            // Create another Employee object with a different Id
            Employee emp3 = new Employee { Id = 2, FirstName = "Bob", LastName = "Johnson" };

            // Compare emp1 and emp2 using the overloaded '==' operator
            Console.WriteLine($"emp1 == emp2: {emp1 == emp2}"); // Expected: True

            // Compare emp1 and emp3 using the overloaded '==' operator
            Console.WriteLine($"emp1 == emp3: {emp1 == emp3}"); // Expected: False

            // Keep console window open until a key is pressed
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

           
