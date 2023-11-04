using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Avalonia.Media.Imaging;
using Newtonsoft.Json;
namespace WeatherApp;
using Avalonia;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class weatherData
{
    public List<Weather> weather { get; set; }
    public string @base { get; set; }
    public MainData main { get; set; }
    public int visibility { get; set; }
    public Wind wind { get; set; }
    public long dt { get; set; }
    public int timezone { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public int cod { get; set; }
}

public class Weather
{
    public int Id { get; set; }
    public string Main { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}

public class MainData
{
    public double Temp { get; set; }
    public double FeelsLike { get; set; }
    public double TempMin { get; set; }
    public double TempMax { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
}

public class Wind
{
    public double Speed { get; set; }
    public int Deg { get; set; }
}

public class Location
{
    [JsonProperty("name")]
    public string NameCity { get; set; }

    [JsonProperty("lat")]
    public double Latitude { get; set; }

    [JsonProperty("lon")]
    public double Longitude { get; set; }

}



public partial class MainWindow : Window
{


    public String imageUrl;
    public Image Weatherimage;
    public TextBlock Lienicon;

    public MainWindow()
    {

        InitializeComponent();
        //T1.Text = "Today 10° C";
        //T2.Text = "Partially Cloudy";
        //T3.Text = "Precipitation: 25 %";
        //T4.Text = "High:20°C";
        //T5.Text = "Low 0°C";
        //T6.Text = "Feels like : 10°C";
        Weatherimage = this.FindControl<Image>("WeatherImage");
        test();
    }

    public async void test()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                Location newLocation = new Location {};
                (newLocation.NameCity,newLocation.Latitude,newLocation.Longitude) = await GetCityAsync("London");
                Console.WriteLine($"Second Check : Name : {newLocation.NameCity}Latitude: {newLocation.Latitude}, Longitude: {newLocation.Longitude}");

                // Spécifiez l'URL de l'API que vous souhaitez interroger
                string apiUrl =
                    $"https://api.openweathermap.org/data/2.5/weather?lat={newLocation.Latitude}&lon={newLocation.Longitude}&appid=19e8ae246f03ffc54bbdae83a37e7315&lang=fr&units=metric&exclud=name";

                // Effectuez une requête GET
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Vérifiez si la requête a réussi (code de statut 200 OK)
                if (response.IsSuccessStatusCode)
                {
                    // Lisez le contenu de la réponse
                    string content = await response.Content.ReadAsStringAsync();
                    string jsonString = content;
                    var weatherData = JsonConvert.DeserializeObject<weatherData>(jsonString);
                    NameVille.Text = $"Ville : {weatherData.name}";
                    TempVille.Text = $"Température : {weatherData.main.Temp}°C";
                    Humidite.Text = $"Humidité : {weatherData.main.Humidity}%";
                    Description.Text = $"Description météo : {weatherData.weather[0].Description}";
                    // LienIcon.Text = $"Lien icon : http://openweathermap.org/img/w/{weatherData.weather[0].Icon}.png";
                    imageUrl = $"http://openweathermap.org/img/w/{weatherData.weather[0].Icon}.png";
                    LoadImageFromUrl(imageUrl);
                }
                else
                {
                    Console.WriteLine($"La requête a échoué avec le code de statut : {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
        }
    }

    public async void LoadImageFromUrl(string imageUrl)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                using (var stream = new MemoryStream(imageBytes))
                {
                    var bitmap = new Bitmap(stream);
                    Weatherimage.Source = bitmap;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any potential exceptions, such as network errors or invalid URLs.
            Console.WriteLine("Error loading image: " + ex.Message);
        }
    }

    public async Task<(string NameLocation, double Lat, double Lon)> GetCityAsync(string input)
    {
        Console.WriteLine("here");
        string apiUrl =
            $"http://api.openweathermap.org/geo/1.0/direct?q=London&limit=1&appid=19e8ae246f03ffc54bbdae83a37e7315";

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

                    //list the name of all cities
                    var Cities = JsonConvert.DeserializeObject<List<Location>>(jsonContent);
                    //Console.WriteLine($"result1:{Cities}");
                    // Check if any cities were found
                    if (Cities == null)
                    {
                        Console.WriteLine("null");
                        return (String.Empty, 0, 0);
                    }

                    // the first city in the list
                    var City = Cities[0];
                    Console.WriteLine($"First city found: Name: {City.NameCity}, Latitude: {City.Latitude}, Longitude: {City.Longitude}");

                    return (City.NameCity, City.Latitude, City.Longitude);
                    
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

        return (String.Empty, 0, 0);
    }
}

