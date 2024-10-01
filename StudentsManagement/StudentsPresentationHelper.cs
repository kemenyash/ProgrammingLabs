using StudentsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagement
{
    public class StudentsPresentationHelper
    {
        private readonly StudentsRoot root;

        public StudentsPresentationHelper(StudentsRoot root) => this.root = root;

        public void ShowAll()
        {
            if (root is null) return;
            foreach (var student in root.Students) 
            {
                Console.WriteLine($"Full name: {student.FirstName} {student.LastName}");
                Console.WriteLine($"Gender {student.Gender}");
                Console.WriteLine($"Rating: {string.Join(", ", student.Rating
                    .Select(x =>  ((int)x).ToString()))}");
                Console.WriteLine($"Course: {student.Course}");
                Console.WriteLine($"Faculty: {student.Faculty}");
                Console.WriteLine($"Scholarship: {student.Scholarship}\r\n\r\n");
            }
        }

        public void ShowRatingAndScholarship(string lastName)
        {
            if (root is null) return;
            if(string.IsNullOrEmpty(lastName)) return;

            var student = root.Students.FirstOrDefault(x => 
                                       x.LastName.ToLower() == lastName.ToLower());
            if(student is null)
            {
                Console.WriteLine("Student not found");
                return;
            }

            Console.WriteLine($"Rating of student {student.FirstName} {student.LastName}: {string.Join(", ", student.Rating
                    .Select(x => ((int)x).ToString()))}");
            Console.WriteLine($"Scholarship: {student.Scholarship}\r\n\r\n");
        }

        public void ShowStudents(Faculty faculty, Course course, Rating rating, Gender gender)
        {
            if (root is null) return;
            var students = root.Students.FindAll(x => x.Faculty == faculty && x.Course == course);
            if(students is null)
            {
                Console.WriteLine("Students not found on faculty and course");
                return;
            }

            var studentsWithTheSameRating = students.Where(x => x.Gender == gender && x.Rating.Contains(rating))
                .Select(x => $"{x.FirstName} {x.LastName}");

            Console.WriteLine($"Students on faculty {faculty} and " +
                $"{course} course with rating {(int)rating}: " +
                $"{string.Join(", ", studentsWithTheSameRating)}");

        }
    }
}
