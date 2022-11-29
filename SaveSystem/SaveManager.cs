using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace SaveSystem
{
    /// <summary>
    /// The save manager saved the data as json to the same folder as the project.
    /// Also made it autosave when data is updated.
    /// </summary>
    public static class SaveManager
    {
        public static T? Load<T>(string identifier) => GetDataFromFile<T>(identifier);
        public static void Save<TValue>(string identifier, TValue data) => SaveDataToFile(identifier, data);

        private static T? GetDataFromFile<T>(string identifier)
        {
            string json = Loadfile(identifier);
            if (json == string.Empty) return default;

            return JsonSerializer.Deserialize<T>(json);
        }

        private static void SaveDataToFile<TValue>(string identifier, TValue data) => SaveFile(identifier, JsonSerializer.Serialize(data));

        public static void SaveFile(string identifier, string json)
        {
            using StreamWriter file = new($"{identifier}.txt");
            file.Write(json);
        }
        public static string Loadfile(string identifier)
        {
            if (!File.Exists($"{identifier}.txt")) return string.Empty;

            using StreamReader file = new($"{identifier}.txt");
            return file.ReadToEnd();
        }

    }
}
