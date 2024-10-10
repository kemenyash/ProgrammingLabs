using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    [Table("assesments")]
    public class Assesment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("value")]
        public Schemas.Assesment Value { get; set; }
        [Column("student_id")]
        public int StudentId { get; set; }

        public Student Student { get; set; }
    }
}
