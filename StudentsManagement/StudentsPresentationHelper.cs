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

        if(!studentsOnCourses.Any() || !scholarShips.Any())
        {
            Console.WriteLine("Database is empty, please init data");
            return;
        }

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
            if (scholarShip?.Value != null)
            {
                Console.WriteLine($"Scholarship: {scholarShip.Value}");
            }
            Console.WriteLine("\r\n\r\n");
        }
    }

    public void ShowAssesmentAndScholarship(string firstName)
    {
        //шукаємо студента по прізвищі
        var student = dbHelper.Students.FirstOrDefault(x => x.FirstName.ToLower() == firstName.ToLower());

        if(student is null)
        {
            //виводимо відповідне повідомлення, якщо не знайшли
            Console.WriteLine("Students not found");
            return;
        }

        //зберігаємо собі оцінки студента та перевіряємо чи є студент в таблиці зі стипедіантами
        var assesments = dbHelper.Assesments.Where(x => x.StudentId == student.Id).Select(a => (int)a.Value);
        var scholarship = dbHelper.Scholarships.FirstOrDefault(x => x.StudentId == student.Id);

        //виводимо оцінки через кому
        Console.WriteLine($"Assesments of student {student.FirstName} {student.LastName}: {string.Join(",", assesments)}");

        //перевіряємо наявність стипендії
        if (scholarship != null)
        {
            Console.WriteLine($"Scholarship: {scholarship.Value}\r\n\r\n");
        }
        else
        {
            Console.WriteLine($"Student without scholarship\r\n\r\n");
        }

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

        //якщо відмінників нема - повідомляємо про це
        if (!goodStudents.Any()) 
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
        Console.WriteLine($"Good students on faculty {faculty} and course {course}: {string.Join(", ", studentNames)}");
    }
}
