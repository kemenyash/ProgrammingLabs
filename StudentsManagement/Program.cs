using StudentsManagement;
using StudentsManagement.Models;

#region Initialization
var path = "Student.xml";
var xmlHelper = new XMLHelper();
var students = new StudentsRoot
{
    Students = new List<Student>
    {
        new Student
        {
            Course = Course.First,
            Faculty = Faculty.Biologic,
            Gender = Gender.Female,
            FirstName = "Olena",
            LastName = "Shevchenko",
            Scholarship = 1400,
            Rating = new List<Rating>
            {
                Rating.Excelent,
                Rating.Well
            }
        },

        new Student
        {
            Course = Course.Third,
            Faculty = Faculty.Biologic,
            Gender = Gender.Female,
            FirstName = "Iryna",
            LastName = "Kovalenko",
            Scholarship = 1890,
            Rating = new List<Rating>
            {
                Rating.Excelent,
                Rating.Well
            }
        },

        new Student
        {
            Course = Course.Four,
            Faculty = Faculty.Medical,
            Gender =    Gender.Male,
            FirstName = "Andriy",
            LastName = "Lysenko",
            Scholarship = 0,
            Rating = new List<Rating>
            {
                Rating.Bad,
                Rating.Well
            }
        },

        new Student
        {
            Course = Course.Second,
            Faculty = Faculty.Economical,
            Gender = Gender.Female,
            FirstName = "Kateryna",
            LastName = "Melnyk",
            Scholarship = 1000,
            Rating = new List<Rating>
            {
                Rating.Excelent,
                Rating.Well,
                Rating.NotVery
            }
        },

        new Student
        {
            Course = Course.First,
            Faculty =   Faculty.MathAndDigitalTech,
            Gender = Gender.Female,
            FirstName = "Svitlana",
            LastName = "Hrytsenko",
            Scholarship = 0,
            Rating = new List<Rating>
            {
                Rating.Bad,
                Rating.NotVery
            }
        },

        new Student
        {
            Course = Course.Second,
            Faculty = Faculty.MathAndDigitalTech,
            Gender = Gender.Male,
            FirstName = "Dmytro",
            LastName = "Moroz",
            Scholarship = 1500,
            Rating = new List<Rating>
            {
                Rating.Excelent,
                Rating.Excelent
            }
        },

        new Student
        {
            Course = Course.Four,
            Faculty = Faculty.Geographic,
            Gender = Gender.Male,
            FirstName = "Oleksandr",
            LastName = "Tkachenko",
            Scholarship = 100,
            Rating = new List<Rating>
            {
                Rating.Well,
                Rating.Well
            }
        },

        new Student
        {
            Course = Course.Third,
            Faculty = Faculty.MathAndDigitalTech,
            Gender = Gender.Female,
            FirstName = "Viktoria",
            LastName = "Bondarenko",
            Scholarship = 500,
            Rating = new List<Rating>
            {
                Rating.NotVery,
                Rating.Well
            }
        },

        new Student
        {
            Course = Course.First,
            Faculty = Faculty.Biologic,
            Gender = Gender.Female,
            FirstName = "Mariia",
            LastName = "Krutyi",
            Scholarship = 700,
            Rating = new List<Rating>
            {
                Rating.NotVery, 
                Rating.Well
            }
        },

        new Student
        {
            Course = Course.First,
            Faculty = Faculty.MathAndDigitalTech,
            Gender = Gender.Male,
            FirstName = "Petro",
            LastName = "Voronin",
            Scholarship = 1200,
            Rating = new List<Rating>
            {
                Rating.Excelent,
                Rating.Well
            }
        },

        new Student
        {
            Course = Course.First,
            Faculty = Faculty.Medical,
            Gender =  Gender.Male,
            FirstName = "Mykola",
            LastName = "Vasylenko",
            Scholarship = 0,
            Rating = new List<Rating>
            {
                Rating.Bad, 
                Rating.NotVery
            }
        },

    }
};
#endregion

//створюємо файл на базі students-списку
xmlHelper.CreateXmlFile<StudentsRoot>(path, students);

//створюємо рутовий об'єкт, в який десеріалізуємо файл
var studentsRoot = xmlHelper.GetRoot<StudentsRoot>(path);

//ініціалізуємо хелпер з допомогою рутового об'єкту, який десеріалізували
var studentsPresentationHelper = new StudentsPresentationHelper(studentsRoot);

//показуємо всіх студентів
studentsPresentationHelper.ShowAll();

//показуємо оцінки за прізвищем (не залежно від регістру)
studentsPresentationHelper.ShowRatingAndScholarship("shevchenko");

//Виводимо хлопців відмінників, які вчаться на першому курсі факультету математики та цифрових технологій
studentsPresentationHelper.ShowStudents(Faculty.MathAndDigitalTech, Course.First, Rating.Excelent, Gender.Male);
Console.ReadKey();
