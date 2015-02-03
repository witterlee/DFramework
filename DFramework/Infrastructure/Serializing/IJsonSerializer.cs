using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DFramework
{
    public class JsonSerializer : IJsonSerializer
    {
        JsonSerializerSettings settings;

        public JsonSerializer()
        {
            settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
        public string Serialize(object obj)
        {
            string jsonData = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);

            return jsonData;
        }

        public object Deserialize(string value, Type type)
        {
            object obj = JsonConvert.DeserializeObject(value, type, settings);

            return obj;
        }

        public T Deserialize<T>(string value) where T : class
        {
            T obj = null;

            try
            {
                obj = (T)this.Deserialize(value, typeof(T));
                return obj;
            }
            catch
            {
                return null;
            }

        }
    }

    public interface IJsonSerializer
    {
        string Serialize(object obj);
        object Deserialize(string value, Type type);
        T Deserialize<T>(string value) where T : class;
    }
}
