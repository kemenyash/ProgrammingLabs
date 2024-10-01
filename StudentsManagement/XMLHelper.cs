using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace StudentsManagement
{
    public class XMLHelper
    {
        public void CreateXmlFile<T>(string filePath, T root)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new XmlTextWriter(filePath, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 3
            })
            {
                serializer.Serialize(writer, root);
            }
        }

        public T GetRoot<T>(string filePath) 
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new XmlTextReader(filePath)) 
            {
                var root = (T)serializer.Deserialize(reader);
                return root;
            }
        }
    }
}
