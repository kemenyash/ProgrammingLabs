using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsManagement.Models
{
    public class Student
    {
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("Gender")]
        public Gender Gender { get; set; }

        [XmlElement("Scholarship")]
        public decimal Scholarship { get; set; }

        [XmlElement("Faculty")]
        public Faculty Faculty { get; set; }

        [XmlElement("Course")]
        public Course Course { get; set; }
        [XmlElement("Rating")]
        public List<Rating> Rating { get; set; }
    }
}
