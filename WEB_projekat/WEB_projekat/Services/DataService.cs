using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WEB_projekat.Services
{
    public static class DataService
    {
        private static readonly string DataFolder = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "data");

        public static List<T> LoadData<T>(string fileName)
        {
            try
            {
                string path = Path.Combine(DataFolder, fileName);

                if (!File.Exists(path))
                {
                    return new List<T>();
                }

                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }
        public static void SaveData<T>(string fileName, List<T> data)
        {
            try
            {
                string path = Path.Combine(DataFolder, fileName);

                if (!Directory.Exists(DataFolder))
                {
                    Directory.CreateDirectory(DataFolder);
                }

                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Greška prilikom čuvanja podataka: " + ex.Message);
            }
        }
    }
}