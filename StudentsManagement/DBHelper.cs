using DataStore;
using Schemas;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace StudentsManagement
{
    public class DBHelper
    {

        private readonly bool isMockNeeded;

        #region Properties

        public IEnumerable<Scholarship> Scholarships
        {
            get
            {
                using (var dataContext = new DataContext())
                {
                    return dataContext.Scholarships;
                }
            }
        }

        public IEnumerable<StudentsOnCourse> StudentsOnCourses
        {
            get
            {
                using (var dataContext = new DataContext())
                {
                    return dataContext.StudentsOnCourses;
                }
            }
        }

        public IEnumerable<DataStore.Assesment> Assesments
        {
            get
            {
                using(var dataContext = new DataContext())
                {
                    return dataContext.Assesments;
                }
            }
        }

        #endregion

        public DBHelper(bool isMockNeeded)
        {
            this.isMockNeeded = isMockNeeded;
            if (isMockNeeded) Initialization().ConfigureAwait(false);
        }

        #region Initialization

        private async Task Initialization()
        {
            await FacultiesInit();
            await CoursesInit();
            await StudentsInit();
            await AssesmentInit();
        }

        private async Task FacultiesInit()
        {
            var facultiesNames = new[]
            {
                "Biologic",
                "Geographic",
                "Economical",
                "Medical",
                "MathAndDigitalTech"
            };

            using (var dataContext = new DataContext())
            {
                //Ініціалізуємо список факультетів
                for (int i = 0; i < facultiesNames.Length; i++) 
                {
                    await dataContext.Faculties.AddAsync(new Faculty { Name = facultiesNames[i] });
                }
                await dataContext.SaveChangesAsync();
            }
        }

        private async Task CoursesInit()
        {
            using(var dataContext = new DataContext())
            {

                foreach (var faculty in dataContext.Faculties)
                {
                    //Додаємо по 4 курси на кожен факультет почиинаючи з 1-го і закінчуючи 4-им
                    for(int i = 1; i < 5; i++)
                    {
                        await dataContext.Courses.AddAsync(new Course
                        {
                            FacultyId = faculty.Id,
                            Value = i
                        });
                    }
                }
                await dataContext.SaveChangesAsync();
            }
        }

        private async Task StudentsInit()
        {
            var studentsMaleMock = new Dictionary<string, string>
            {
                { "Koval", "Ivan" },
                { "Marchuk", "Stepan" },
                { "Kasian", "Vasyl" },
                { "Savoliuk", "Mykhailo" },
                { "Hromovych", "Andriy" },
            };

            var studentsFemaleMock = new Dictionary<string, string>
            {
                { "Koval", "Ivanna" },
                { "Marchuk", "Stepanida" },
                { "Kasian", "Vasylyna" },
                { "Savoliuk", "Mykhailyna" },
                { "Hromovych", "Andriana" },
            };

            using (var dataContext = new DataContext())
            {

                //Додаємо студентів на курси
                foreach(var course in dataContext.Courses)
                {
                    //Ініціалізуємо студентів з допомогою списків, щоб після збережння мати їх id
                    var studentsMaleData = studentsMaleMock.Select(x => new Student
                    {
                        FirstName = x.Key,
                        LastName = x.Value,
                        Gender = Schemas.Gender.Male
                    });


                    var studentsFemaleData = studentsFemaleMock.Select(x => new Student
                    {
                        Gender = Schemas.Gender.Female,
                        FirstName = x.Key,
                        LastName = x.Value,
                    });

                    await dataContext.Students.AddRangeAsync(studentsMaleData);
                    await dataContext.Students.AddRangeAsync (studentsFemaleData);

                    await dataContext.SaveChangesAsync();
                    
                    //Після збереження вибираємо id і робимо модель контексту для збереження в суміжній таблиці
                    
                    var femaleStudentsIds = studentsFemaleData.Select(x => new StudentsOnCourse
                    {
                        CourseId = course.Id,
                        StudentId = x.Id
                    });
                    var maleStudentsIds = studentsMaleData.Select(x => new StudentsOnCourse
                    {
                        CourseId = course.Id,
                        StudentId = x.Id
                    });

                    await dataContext.StudentsOnCourses.AddRangeAsync(femaleStudentsIds);
                    await dataContext.StudentsOnCourses.AddRangeAsync(maleStudentsIds);

                    await dataContext.SaveChangesAsync();
                }                

                
            }
        }

        private async Task AssesmentInit()
        {
            using (var dataContext = new DataContext())
            {
                //отримуємо всі значення енумератора, щоб потім з масиву рандомно призначити студенту
                var values = Enum.GetValues(typeof(Schemas.Assesment));

                foreach (var student in dataContext.Students)
                {
                    //вибираємо 5 рандомних оцінок в список
                    var assesments = new List<Schemas.Assesment>();
                    for (int i = 0; i < 6; i++)
                    {
                        Random random = new Random();
                        var assesment = (Schemas.Assesment)values.GetValue(random.Next(values.Length));
                    }

                    //пачкою зберігаємо оцінки студенту
                    var assesmentsData = assesments.Select(x => new DataStore.Assesment
                    {
                        StudentId = student.Id,
                        Value = x
                    });

                    await dataContext.Assesments.AddRangeAsync(assesmentsData);
                    await dataContext.SaveChangesAsync();

                }
            }
        }

        private async Task ScholarshipInit()
        {

            using (var dataContext = new DataContext())
            {
                //всім студентам, які мають середній бал 4 і вище - даємо стипендію і відповідно додаємо в таблицю
                foreach(var student in dataContext.Students)
                {
                    var assesments = dataContext.Assesments.Where(x => x.StudentId == student.Id).Select(a => a.Value);
                    var average =  assesments.Average(a => (int)a);

                    if (average > 4)
                    {

                        await dataContext.Scholarships.AddAsync(new Scholarship
                        {
                            StudentId = student.Id,
                            Value = 1500
                        });

                        await dataContext.SaveChangesAsync();
                    }
                }
            }
        }

        #endregion
    }
}
