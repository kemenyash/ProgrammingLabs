using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    public class DataContext : DbContext
    {
        public DbSet<Assesment> Assesments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Scholarship> Scholarships { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentsOnCourse> StudentsOnCourses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=students;Username=postgres;Password=postgres");
        }
    }
}
