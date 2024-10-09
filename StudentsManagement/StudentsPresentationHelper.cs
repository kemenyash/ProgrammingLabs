using System;
using System.Collections.Generic;
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
        string query = @"
            SELECT s.FirstName, s.LastName, s.Gender, s.Scholarship, c.CourseNumber, f.Name as Faculty
            FROM Students s
            JOIN Courses c ON s.CourseId = c.CourseId
            JOIN Faculties f ON s.FacultyId = f.FacultyId";

        var students = dbHelper.ExecuteSelectQuery(query);

        foreach (var student in students)
        {
            Console.WriteLine($"Full name: {student["FirstName"]} {student["LastName"]}");
            Console.WriteLine($"Gender: {student["Gender"]}");
            Console.WriteLine($"Course: {student["CourseNumber"]}");
            Console.WriteLine($"Faculty: {student["Faculty"]}");
            Console.WriteLine($"Scholarship: {student["Scholarship"]}\r\n\r\n");
        }
    }

    public void ShowRatingAndScholarship(string lastName)
    {
        string query = @"
            SELECT s.FirstName, s.LastName, s.Scholarship, r.Grade
            FROM Students s
            LEFT JOIN Ratings r ON s.StudentId = r.StudentId
            WHERE s.LastName = @LastName";

        var parameters = new Dictionary<string, object>
        {
            { "@LastName", lastName }
        };

        var students = dbHelper.ExecuteSelectQuery(query, parameters);

        if (students.Count == 0)
        {
            Console.WriteLine("Student not found");
        }
        else
        {
            foreach (var student in students)
            {
                Console.WriteLine($"Rating of student {student["FirstName"]} {student["LastName"]}: {student["Grade"]}");
                Console.WriteLine($"Scholarship: {student["Scholarship"]}\r\n\r\n");
            }
        }
    }

    public void ShowStudents(string faculty, int course, decimal rating, string gender)
    {
        string query = @"
            SELECT s.FirstName, s.LastName
            FROM Students s
            JOIN Faculties f ON s.FacultyId = f.FacultyId
            JOIN Courses c ON s.CourseId = c.CourseId
            LEFT JOIN Ratings r ON s.StudentId = r.StudentId
            WHERE f.Name = @Faculty AND c.CourseNumber = @Course AND r.Grade = @Rating AND s.Gender = @Gender";

        var parameters = new Dictionary<string, object>
        {
            { "@Faculty", faculty },
            { "@Course", course },
            { "@Rating", rating },
            { "@Gender", gender }
        };

        var students = dbHelper.ExecuteSelectQuery(query, parameters);

        if (students.Count == 0)
        {
            Console.WriteLine("Students not found on faculty and course");
        }
        else
        {
            var studentNames = new List<string>();
            foreach (var student in students)
            {
                studentNames.Add($"{student["FirstName"]} {student["LastName"]}");
            }
            Console.WriteLine($"Students on faculty {faculty} and {course} course with rating {rating}: {string.Join(", ", studentNames)}");
        }
    }
}
