using System;
using System.Collections.Generic;
using DataStore;
using Schemas;
using StudentsManagement;

public class StudentsPresentationHelper
{
    private readonly DBHelper dbHelper;

    public StudentsPresentationHelper(DBHelper dbHelper)
    {
        this.dbHelper = dbHelper;
    }

    public void ShowAll()
    {
       
        var studentsOnCourses = dbHelper.StudentsOnCourses;
        var scholarShips = dbHelper.Scholarships;

        foreach (var studentOnCourse in studentsOnCourses)
        {
            var student = studentOnCourse.Student;
            var course = studentOnCourse.Course;
            var faculty = course.Faculty;
            var scholarShip = scholarShips.Where(x => x.StudentId == student.Id).FirstOrDefault();

            Console.WriteLine($"Full name: {student.FirstName} {student.LastName}");
            Console.WriteLine($"Gender: {student.Gender.ToString()}");
            Console.WriteLine($"Course: {course.Value}");
            Console.WriteLine($"Faculty: {faculty.Name}");
            Console.WriteLine($"Scholarship: {scholarShip.Value}\r\n\r\n");
        }
    }

    public void ShowAssesmentAndScholarship(string lastName)
    {
        var studentScholarship = dbHelper.Scholarships.FirstOrDefault(x => x.Student.FirstName.ToLower() == lastName.ToLower());
        var assesments = dbHelper.Assesments.Where(x => x.StudentId == studentScholarship.StudentId).Select(x => x.Value);
        
        if(studentScholarship is null)
        {
            Console.WriteLine("Student not found");
            return;
        }
        var student = studentScholarship.Student;

        Console.WriteLine($"Assesments of student {student.FirstName} {student.LastName}: {string.Join(",", assesments)}");
        Console.WriteLine($"Scholarship: {studentScholarship.Value}\r\n\r\n");

    }

    public void ShowGoodStudents(string faculty, int course, Gender gender)
    {

        var studentsOnCourse = dbHelper.StudentsOnCourses.Where(x => x.Student.Gender == gender &&
                                                                x.Course.Faculty.Name.ToLower() == faculty.ToLower()
                                                                && x.Course.Value == course);

        if (studentsOnCourse is null || !studentsOnCourse.Any())
        {
            Console.WriteLine("Students not found on faculty and course");
            return;
        }

        //критерієм відмінника є наявність стипендії, тому ми можемо пошукати співпадіння в таблиці з стипендіями
        var goodStudents = new List<Student>();
        foreach(var studentOnCourse in studentsOnCourse)
        {
            if(dbHelper.Scholarships.Any(x => x.StudentId == studentOnCourse.StudentId))
            {
                goodStudents.Add(studentOnCourse.Student);
            }

        }

        if (goodStudents.Any()) 
        {
            Console.WriteLine("Good students not found on faculty and course");
            return;
        }

        //виводимо відмінників
        var studentNames = new List<string>();
        foreach (var student in goodStudents)
        {
            studentNames.Add($"{student.FirstName} {student.LastName}");
        }
        Console.WriteLine($"Good students on faculty {faculty} and {course}: {string.Join(", ", studentNames)}");
    }
}
