using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    [Table("students_on_courses")]
    public class StudentsOnCourse
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("student_id")]
        public int StudentId { get; set; }

        [Column("course_id")]
        public int CourseId { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }

    }
}
