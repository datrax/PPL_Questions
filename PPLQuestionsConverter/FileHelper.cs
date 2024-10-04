using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

public static class FileHelper
{
    static readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true
    };

    public static void SaveToOutput<T>(string outputFilePath,T questions)
    {
        string json = JsonSerializer.Serialize(questions, options).Replace("\\n", "").Replace(" \"", "\"");

        File.WriteAllText(outputFilePath, json, Encoding.Unicode);
    }

    public static T? GetFromOutputFile<T>(string outputFilePath)
    {
        return JsonSerializer.Deserialize<T>(File.ReadAllText(outputFilePath, Encoding.Unicode), options);

    }
}