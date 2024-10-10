using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    [Table("courses")]
    public class Course
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("value")]
        public int Value { get; set; }

        [Column("faculty_id")]
        public int FacultyId { get; set; }

        public Faculty Faculty { get; set; }

    }
}
