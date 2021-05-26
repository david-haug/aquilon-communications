using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aes.Communication.Infrastructure.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aes.Communication.Infrastructure
{
    public class JsonRepository<T>
    {

        public List<T> Create(string file,IDomainObjectParser<T> parser)
        {
            var json = string.Empty;
            var objects = new List<T>();

            using (StreamReader sr = File.OpenText(file))
            {
                json = sr.ReadToEnd();
                dynamic data = JsonConvert.DeserializeObject(json);
                foreach (var d in data)
                {
                    var obj = parser.Create(d);
                    objects.Add(obj);
                }
            }

            return objects;
        }

        public void SaveJsonToFile(object obj, string file)
        {
            using (FileStream fs = File.Open(file, FileMode.Truncate))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    TypeNameHandling = TypeNameHandling.Auto,

                };

                serializer.Serialize(jw, obj);
            }
        }
    }
}
