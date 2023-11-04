using Avalonia;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherApp;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    

    

    public static void Main(string[] args)
    {
        
        //1st Method
    string json = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}";
    //Person person = JsonConvert.DeserializeObject<Person>(json);
    // Console.WriteLine(person.Name);
    // Console.WriteLine(person.City);
    JObject jObject = JObject.Parse(json);
    
    //2nd Method
    string name = (string)jObject["name"];
    int age = (int)jObject["age"];
    string city = (string)jObject["city"];
    string city2 = (string)jObject["city2"];
    
    // Console.WriteLine(jObject);
    // Console.WriteLine(age);
    // Console.WriteLine(city);
    // Console.WriteLine(city2);

    //Person person2 = new Person { Name = "John", Age = 30, City = "New York" };
    //string json2 = JsonConvert.SerializeObject(person2);
    // Console.WriteLine(json2);
    //string json3 = "[{\"name\":\"John\",\"age\":30,\"city\":\"New York\"},{\"name\":\"Camille\",\"age\":30,\"city\":\"Chicago\"}]";
    //JArray jArray = JArray.Parse(json3);
    //string nameOfSecondElement = jArray[1]["name"].ToString();
    // Console.WriteLine(nameOfSecondElement);
    //var query2 = jArray.Where(x => (int)x["age"] == 30).ToList();

    // Console.WriteLine(London);
            // foreach (var item in query2)
            // {
            //     Console.WriteLine(item);
            // }
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            } 


    // Avalonia configuration, don't remove; also used by visual designer.
   



}