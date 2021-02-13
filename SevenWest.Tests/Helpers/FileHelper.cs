namespace SevenWest.Tests.Helpers
{
    public class FileHelper
    {
        public static string GetAsString(string filePath)
        {
            var data = System.IO.File.ReadAllText(filePath);
            return data;
        }

        public static T GetAsJson<T>(string filePath)
        {
            var json = System.IO.File.ReadAllText(filePath);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return data;
        }
    }
}
