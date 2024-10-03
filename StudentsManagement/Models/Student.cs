using Newtonsoft.Json;
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
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Gender")]
        public Gender Gender { get; set; }

        [JsonProperty("Scholarship")]
        public decimal Scholarship { get; set; }

        [JsonProperty("Faculty")]
        public Faculty Faculty { get; set; }

        [JsonProperty("Course")]
        public Course Course { get; set; }
        [JsonProperty("Rating")]
        public List<Rating> Rating { get; set; }
    }
}
