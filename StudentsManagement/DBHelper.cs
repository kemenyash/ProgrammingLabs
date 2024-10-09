using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace StudentsManagement
{
    public class DBHelper
    {
        private readonly string connectionString;
        private SqlConnection connection;
        private bool isMockNeeded;

        public DBHelper(bool isMockNeeded)
        {
            this.isMockNeeded = isMockNeeded;
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=students;Integrated Security=True;";
            connection = new SqlConnection(connectionString);
            Initialization();
        }


        public List<Dictionary<string, object>> ExecuteSelectQuery(string query, Dictionary<string, object> parameters = null)
        {
            var result = new List<Dictionary<string, object>>();

            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.GetValue(i);
                        }
                        result.Add(row);
                    }
                }
            }

            return result;
        }

        #region Mocks and initialization
        private void Initialization()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();

            string checkDbQuery = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'students') BEGIN CREATE DATABASE students; END";
            using (SqlCommand command = new SqlCommand(checkDbQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            CreateTables();
            if (isMockNeeded) InsertMockData();
        }

        private void CreateTables()
        {
            string useDbQuery = "USE students;";
            using (SqlCommand command = new SqlCommand(useDbQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string createTablesQuery = @"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Faculties' AND xtype='U')
        BEGIN
            CREATE TABLE Faculties (
                FacultyId INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(100) NOT NULL
            );
        END;

        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Courses' AND xtype='U')
        BEGIN
            CREATE TABLE Courses (
                CourseId INT PRIMARY KEY IDENTITY(1,1),
                CourseNumber INT NOT NULL
            );
        END;

        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Students' AND xtype='U')
        BEGIN
            CREATE TABLE Students (
                StudentId INT PRIMARY KEY IDENTITY(1,1),
                FirstName NVARCHAR(50),
                LastName NVARCHAR(50),
                Gender NVARCHAR(10),
                Scholarship DECIMAL(18, 2),
                FacultyId INT FOREIGN KEY REFERENCES Faculties(FacultyId),
                CourseId INT FOREIGN KEY REFERENCES Courses(CourseId)
            );
        END;

        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Ratings' AND xtype='U')
        BEGIN
            CREATE TABLE Ratings (
                RatingId INT PRIMARY KEY IDENTITY(1,1),
                StudentId INT FOREIGN KEY REFERENCES Students(StudentId),
                SubjectId INT,
                Grade DECIMAL(18, 2)
            );
        END;
    ";

            using (SqlCommand command = new SqlCommand(createTablesQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void InsertMockData()
        {
            string insertFacultiesQuery = "INSERT INTO Faculties (Name) VALUES ";
            insertFacultiesQuery += "('Biologic'), ('Medical'), ('Economical'), ('MathAndDigitalTech'), ('Geographic');";

            using (SqlCommand command = new SqlCommand(insertFacultiesQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string insertCoursesQuery = "INSERT INTO Courses (CourseNumber) VALUES ";
            insertCoursesQuery += "(1), (2), (3), (4);";

            using (SqlCommand command = new SqlCommand(insertCoursesQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string insertStudentsQuery = @"
            INSERT INTO Students (FirstName, LastName, Gender, Scholarship, FacultyId, CourseId) VALUES
            ('Olena', 'Shevchenko', 'Female', 1400, 1, 1),
            ('Iryna', 'Kovalenko', 'Female', 1890, 1, 3),
            ('Andriy', 'Lysenko', 'Male', 0, 2, 4),
            ('Kateryna', 'Melnyk', 'Female', 1000, 3, 2),
            ('Svitlana', 'Hrytsenko', 'Female', 0, 4, 1),
            ('Dmytro', 'Moroz', 'Male', 1500, 4, 2),
            ('Oleksandr', 'Tkachenko', 'Male', 100, 5, 4),
            ('Viktoria', 'Bondarenko', 'Female', 500, 4, 3),
            ('Mariia', 'Krutyi', 'Female', 700, 1, 1),
            ('Petro', 'Voronin', 'Male', 1200, 4, 1),
            ('Mykola', 'Vasylenko', 'Male', 0, 2, 1);";

            using (SqlCommand command = new SqlCommand(insertStudentsQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string insertRatingsQuery = @"
            INSERT INTO Ratings (StudentId, SubjectId, Grade) VALUES
            (1, 1, 4.5), (1, 2, 4.0),
            (2, 1, 4.5), (2, 2, 4.0),
            (3, 1, 2.0), (3, 2, 4.0),
            (4, 1, 4.5), (4, 2, 4.0),
            (5, 1, 2.0), (5, 2, 1.5),
            (6, 1, 4.5), (6, 2, 4.5),
            (7, 1, 3.5), (7, 2, 3.5),
            (8, 1, 2.5), (8, 2, 4.0),
            (9, 1, 2.5), (9, 2, 4.0),
            (10, 1, 4.0), (10, 2, 4.0),
            (11, 1, 1.5), (11, 2, 2.0);";

            using (SqlCommand command = new SqlCommand(insertRatingsQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Дані успішно додані до таблиць.");
        }
        #endregion
    }
}
