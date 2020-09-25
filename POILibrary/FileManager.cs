using Newtonsoft.Json;
using System;
using System.IO;

namespace POILibrary
{
    /// <summary>
    /// Static class for storing objects to a file
    /// </summary>
    public static class FileManager<T>
    {
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                + "\\.poi\\library\\";

        private static string filePath = path + "books_data.json";

        /// <summary>
        /// Returns a objects of type <see cref="T"/> from the file which are stored
        /// </summary>
        public static T LoadBooks()
        {
            CheckDirectory();

            using (StreamReader temp = new StreamReader(filePath))
            {
                return JsonConvert.DeserializeObject<T>(temp.ReadToEnd());
            }
        }

        /// <summary>
        /// Saves a objects of type <see cref="T"/> in a file
        /// </summary>
        public static void SaveBooks(T items)
        {
            CheckDirectory();

            File.WriteAllText(filePath, JsonConvert.SerializeObject(items));
        }

        private static void CheckDirectory()
        {
            Directory.CreateDirectory(path);

            if (!File.Exists(filePath))
            {
                using (StreamWriter temp = new StreamWriter(filePath, true)) { temp.WriteLine("[]"); }
            }
        }
    }
}
