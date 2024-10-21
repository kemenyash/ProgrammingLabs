using DataStore;
using Microsoft.EntityFrameworkCore;
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

        public bool IsInitializated { get; private set; }

        public IEnumerable<Scholarship> Scholarships
        {
            get
            {
                using (var dataContext = new DataContext())
                {
                    return dataContext.Scholarships
                        .Include(s => s.Student)
                        .ToList();
                }
            }
        }

        public IEnumerable<Student> Students
        {
            get
            {
                using(var dataContext = new DataContext())
                {
                    return dataContext.Students.ToList();
                }
            }
        }

        public IEnumerable<StudentsOnCourse> StudentsOnCourses
        {
            get
            {
                using (var dataContext = new DataContext())
                {
                    return dataContext.StudentsOnCourses
                                           .Include(s => s.Student) 
                                           .Include(s => s.Course)   
                                           .ThenInclude(c => c.Faculty) 
                                           .ToList();
                }
            }
        }

        public IEnumerable<DataStore.Assesment> Assesments
        {
            get
            {
                using(var dataContext = new DataContext())
                {
                    return dataContext.Assesments
                        .Include(s => s.Student)
                        .ToList();
                }
            }
        }

        #endregion

        public DBHelper(bool isMockNeeded)
        {
            this.isMockNeeded = isMockNeeded;
            
            if (isMockNeeded)
            {
                Initialization().ConfigureAwait(false);
            }
            else
            {
                IsInitializated = true;
            }
        }

        #region Initialization

        private async Task Initialization()
        {
            await FacultiesInit();
            await CoursesInit();
            await StudentsInit();
            await AssesmentInit();
            await ScholarshipInit();
            IsInitializated = true;
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

            List<Student> studentsMaleData;
            List<Student> studentsFemaleData;
            var studentsData = new List<Student>();

            // Контекст 1: Додаємо студентів
            using (var dataContext = new DataContext())
            {
                studentsMaleData = studentsMaleMock.Select(x => new Student
                {
                    FirstName = x.Key,
                    LastName = x.Value,
                    Gender = Schemas.Gender.Male
                }).ToList();

                studentsFemaleData = studentsFemaleMock.Select(x => new Student
                {
                    Gender = Schemas.Gender.Female,
                    FirstName = x.Key,
                    LastName = x.Value,
                }).ToList();


                studentsData.AddRange(studentsMaleData);
                studentsData.AddRange(studentsFemaleData);

                await dataContext.Students.AddRangeAsync(studentsData);
                await dataContext.SaveChangesAsync();
            }

            // Контекст 2: Отримуємо ідентифікатори збережених студентів і додаємо їх до курсів

            var courcesIds = new List<int>();

            using (var dataContext = new DataContext())
            {
                courcesIds = dataContext.Courses.Select(x => x.Id).ToList();
            }

            foreach (var courseId in courcesIds)
            {
                using (var dataContext = new DataContext())
                {
                    var studentsIds = studentsData.Select(x => new StudentsOnCourse
                    {
                        CourseId = courseId,
                        StudentId = x.Id
                    }).ToList();

                    await dataContext.StudentsOnCourses.AddRangeAsync(studentsIds);
                    await dataContext.SaveChangesAsync();
                }
            }
        }




        private async Task AssesmentInit()
        {
            var students = new List<Student>();
            var assesmentValues = Enum.GetValues(typeof(Schemas.Assesment));

            using (var dataContext = new DataContext())
            {
                students = dataContext.Students.ToList();
            }

            bool isOneGoodStudentAdded = false; //флажок для генерації одного гарантованого відмінника, так як в рандомі є вірогідність, що їх взагалі не буде

            foreach (var student in students)
            {
                var assesments = new List<Schemas.Assesment>();
                if (isOneGoodStudentAdded is false)
                {
                    isOneGoodStudentAdded = true;
                    assesments.AddRange(new Schemas.Assesment[] { Schemas.Assesment.Good, Schemas.Assesment.Good, Schemas.Assesment.Good, Schemas.Assesment.Excelent, Schemas.Assesment.Good, });
                }
                else
                {
                    //вибираємо 5 рандомних оцінок в список
                    for (int i = 0; i < 6; i++)
                    {
                        Random random = new Random();
                        var assesment = (Schemas.Assesment)assesmentValues.GetValue(random.Next(assesmentValues.Length));
                        assesments.Add(assesment);
                    }
                }


                //пачкою зберігаємо оцінки студенту
                var assesmentsData = assesments.Select(x => new DataStore.Assesment
                {
                    StudentId = student.Id,
                    Value = x
                });

                using (var dataContext = new DataContext())
                {
                    await dataContext.Assesments.AddRangeAsync(assesmentsData);
                    await dataContext.SaveChangesAsync();
                }

            }
        }

        private async Task ScholarshipInit()
        {
            var scholarships = new List<Scholarship>();
            //всім студентам, які мають середній бал 4 і вище - даємо стипендію і відповідно додаємо в таблицю
            var students = new List<Student>();
            using (var dataContext = new DataContext())
            {
                students = dataContext.Students.ToList();
            }


            foreach (var student in students)
            {
                using (var dataContext = new DataContext())
                {
                    var assesments = dataContext.Assesments.Where(x => x.StudentId == student.Id).Select(a => a.Value);
                    var average = assesments.Average(a => (int)a);

                    if (average > 4)
                    {

                        scholarships.Add(new Scholarship
                        {
                            StudentId = student.Id,
                            Value = 1500
                        });
                    }
                }
            }
            using (var dataContext = new DataContext())
            {
                await dataContext.AddRangeAsync(scholarships);
                await dataContext.SaveChangesAsync();
            }
        }

        #endregion
    }
}
