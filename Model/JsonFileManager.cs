using System.IO;
using System.Linq;

namespace Model.FileManager
{
    public static class JsonFileManager
    {
        public static readonly string LevelsPath = @"levels\";
        public static readonly string GameSavePath = @"saves\last_game.json";

        public static bool GetJsonStingsFromFile(string path, out string[] jsonStrings)
        {
            try
            {
                jsonStrings = File.ReadAllLines(path);
            }
            catch
            {
                jsonStrings = null;
                return false;
            }
            return true;
        }

        public static bool CreateJsonSaveFile(string[] jsonStrings)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(GameSavePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(GameSavePath));
                File.WriteAllLines(GameSavePath, jsonStrings);
            }
            catch { return false; }
            return true;
        }

        public static bool GetFileNamesFromDirectory(string directory, out string[] fileNames)
        {
            try
            {
                fileNames = Directory.GetFiles(directory, "*.json").OrderBy(f => f).ToArray();
            }
            catch {
                fileNames= null;
                return false; 
            }
            return true;
        }
    }
}
