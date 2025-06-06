using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace CromisDev.SaveSystem
{
    public class SaveManager<T> where T : class
    {
        private static string GetPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, $"{fileName}.json");
        }

        public static async Task SaveAsync(T data, string fileName)
        {
            if (data == null || string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("Cannot save: data or filename is null.");
                return;
            }

            string json = JsonUtility.ToJson(data, true);
            string path = GetPath(fileName);

            await File.WriteAllTextAsync(path, json);
        }

        public static async Task<T> LoadAsync(string fileName)
        {
            string path = GetPath(fileName);
            if (!File.Exists(path))
                return null;

            string json = await File.ReadAllTextAsync(path);
            return JsonUtility.FromJson<T>(json);
        }

        public static void Delete(string fileName)
        {
            string path = GetPath(fileName);
            if (File.Exists(path))
                File.Delete(path);
        }

        public static bool Exists(string fileName) => File.Exists(GetPath(fileName));
    }
}
