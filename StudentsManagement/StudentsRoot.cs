using StudentsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsManagement
{
    public class StudentsRoot
    {
        [XmlElement("Student")]
        public List<Student> Students { get; set; }
    }
}
