using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Fishbot
{
    public class JsonFileLoader<T>
    {
        public JsonFileLoader(string fileName)
        {
            var folder = Application.persistentDataPath;
            _databasePath = Path.Combine(folder, fileName + ".json");
        }

        private readonly string _databasePath;

        public T Read()
        {
            if (!File.Exists(_databasePath))
            {
                return default;
            }

            var json = File.ReadAllText(_databasePath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Write(T data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_databasePath, json);
        }
    }
}