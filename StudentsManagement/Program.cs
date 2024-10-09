using StudentsManagement;
using System.Reflection;


Console.WriteLine("Is mock init for DB needed?");
Console.WriteLine("Type 1 if you need mock initialization, or 0 for continue");
var typedData = Console.ReadLine();

bool isMockNeeded = typedData == "1";

var dbHelper = new DBHelper(isMockNeeded);
var studentsPresentationHelper = new StudentsPresentationHelper(dbHelper);

//показуємо всіх студентів
studentsPresentationHelper.ShowAll();

//показуємо оцінки за прізвищем (не залежно від регістру)
studentsPresentationHelper.ShowRatingAndScholarship("shevchenko");

//Виводимо хлопців відмінників, які вчаться на першому курсі факультету математики та цифрових технологій
studentsPresentationHelper.ShowStudents("MathAndDigitalTech", 1, 4, "Male");

Console.ReadKey();
