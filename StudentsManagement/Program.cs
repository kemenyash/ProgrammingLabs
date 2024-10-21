using DataStore;
using Microsoft.EntityFrameworkCore;
using StudentsManagement;
using System.Reflection;

using(var dbContext = new DataContext())
{
    dbContext.Database.Migrate(); //виконуємо міграцію (якщо є нові - вони виконаються)
}

#region Mock init
/*
 * В цьому регіоні є функціонал ініціалізації бази даних мок-даними
 * для того, щоб не заводити їх вручну
 * Є два варіанти вибору режиму, які залежать від вводу цифри з клавіатури:
 * 
 * 0 - продовжити без ініціалізації
 * 1 - ініціалізація моку
 * 
 * Перед першим запуском краще його ініціалізувати, так як без ініціалізації даних у нас
 * не буде що відображати
 */
Console.WriteLine("Is mock init for DB needed?");
Console.WriteLine("Type 1 if you need mock initialization, or 0 for continue");
var typedData = Console.ReadLine();
Console.Clear();

bool isMockNeeded = typedData == "1";
#endregion

var dbHelper = new DBHelper(isMockNeeded);

while(!dbHelper.IsInitializated) { } // чекаємо поки виконаються таски ініціалізації dbHelper

var studentsPresentationHelper = new StudentsPresentationHelper(dbHelper);

//показуємо всіх студентів
studentsPresentationHelper.ShowAll();

//показуємо оцінки за прізвищем (не залежно від регістру)
studentsPresentationHelper.ShowAssesmentAndScholarship("Marchuk");

//Виводимо хлопців відмінників, які вчаться на першому курсі факультету математики та цифрових технологій
studentsPresentationHelper.ShowGoodStudents("Biologic", 1, Schemas.Gender.Male);

Console.ReadKey();
