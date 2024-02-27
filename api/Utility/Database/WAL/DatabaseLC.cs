using Newtonsoft.Json;
using API.Utility;
using Database.Models;
using Newtonsoft.Json.Converters;
using System.Xml.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace API.Utility.Database.WAL;

public class DatabaseLC
{
    private string cachePath = Program.Settings.CachePath;

    public void WriteToLC<T>(T obj)
    {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Converters.Add(new JavaScriptDateTimeConverter());

        using (StreamWriter sw = new StreamWriter(cachePath))
        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            serializer.Serialize(writer, obj);
        }
    }

    public void ReadFromLC<T>() 
    {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Converters.Add(new JavaScriptDateTimeConverter());

        using (StreamReader file = File.OpenText(cachePath))
        {
            Product prod = (Product)serializer.Deserialize(file, typeof(Product));
            Console.WriteLine(prod.Name);
        }
    }
}
