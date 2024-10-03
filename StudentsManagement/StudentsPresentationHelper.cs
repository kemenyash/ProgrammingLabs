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
        private readonly List<Student> students;

        public StudentsPresentationHelper(List<Student> students) => this.students = students;

        public void ShowAll()
        {
            if (students is null) return;
            foreach (var student in students) 
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
            if (students is null) return;
            if(string.IsNullOrEmpty(lastName)) return;

            var student = students.FirstOrDefault(x => 
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
            if (this.students is null) return;
            var students = this.students.FindAll(x => x.Faculty == faculty && x.Course == course);
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
