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

    static async Task GetData()
    {
        string apiUrl = "http://api.openweathermap.org/geo/1.0/direct?q=London&limit=5&appid=19e8ae246f03ffc54bbdae83a37e7315";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Send a GET request to the specified URL
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    // Read the content of the response as a string
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    JArray London = JArray.Parse(jsonContent);
                    //list the name of all cities
                    foreach (var item in London)
                    {
                        Console.WriteLine(item["name"]);
                    }
                    // Console.WriteLine($"ID: {London[1]["name"]}");
                    // Console.WriteLine($"Title: {London["lat"]}");
                }
                else
                {
                    Console.WriteLine($"HTTP Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public static void Main(string[] args)
    {
        
        //1st Method
    string json = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}";
    Person person = JsonConvert.DeserializeObject<Person>(json);
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

    Person person2 = new Person { Name = "John", Age = 30, City = "New York" };
    string json2 = JsonConvert.SerializeObject(person2);
    // Console.WriteLine(json2);
    string json3 = "[{\"name\":\"John\",\"age\":30,\"city\":\"New York\"},{\"name\":\"Camille\",\"age\":30,\"city\":\"Chicago\"}]";
    JArray jArray = JArray.Parse(json3);
    string nameOfSecondElement = jArray[1]["name"].ToString();
    // Console.WriteLine(nameOfSecondElement);
    var query2 = jArray.Where(x => (int)x["age"] == 30).ToList();

    GetData();
    // Console.WriteLine(London);
            // foreach (var item in query2)
            // {
            //     Console.WriteLine(item);
            // }
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            } 


    // Avalonia configuration, don't remove; also used by visual designer.
   



}