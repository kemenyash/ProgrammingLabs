using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsManagement
{
    public class JsonHelper
    {
        public void CreateJsonFile<T>(string filePath, T itemsForSerialize)
        {
            var serializedJson = JsonConvert.SerializeObject(itemsForSerialize);
            File.WriteAllText(filePath, serializedJson);
        }

        public T DeserializeJsonFile<T>(string filePath)
        {
            var serializedJson = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(serializedJson);
        }
    }
}
