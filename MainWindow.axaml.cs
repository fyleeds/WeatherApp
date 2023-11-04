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
using Avalonia.Interactivity;
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

public class WeatherForecast
{

    public List<Forecast> list { get; set; }
    [JsonProperty("city")]
    public City city { get; set; }
}

public class Forecast
{
    
    [JsonProperty("main")]
    public Main main { get; set; }
    [JsonProperty("weather")]
    public List<Weather2> weather { get; set; }
    [JsonProperty("dt_txt")]
    public string dt { get; set; }

    
}

public class Main
{
    [JsonProperty("temp")]
    public double Temp { get; set; }
    
    [JsonProperty("humidity")]
    public int Humidity { get; set; }

}

public class Weather2
{
    [JsonProperty("description")]
    public string description { get; set; }
    [JsonProperty("icon")]
    public string icon { get; set; }
}

public class City
{

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("coord")] public Coordinates Coord { get; set; }
}

public class Coordinates
{
    [JsonProperty("lat")]
    public double Lat { get; set; }

    [JsonProperty("lon")]
    public double Lon { get; set; }
}
//end

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

    }

    public async void test(string input)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                Location newLocation = new Location {};
                (newLocation.NameCity,newLocation.Latitude,newLocation.Longitude) = await GetCityAsync(input);

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
                    //LienIcon= $"Lien icon : http://openweathermap.org/img/w/{weatherData.weather[0].Icon}.png";
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
    public async void testForecast(string input)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                Location newLocation = new Location {};
                (newLocation.NameCity,newLocation.Latitude,newLocation.Longitude) = await GetCityAsync(input);
                Console.WriteLine($"Third Check : Name : {newLocation.NameCity}Latitude: {newLocation.Latitude}, Longitude: {newLocation.Longitude}");

                // Spécifiez l'URL de l'API que vous souhaitez interroger
                string apiUrl =
                    $"https://api.openweathermap.org/data/2.5/forecast?lat={newLocation.Latitude}&lon={newLocation.Longitude}&appid=19e8ae246f03ffc54bbdae83a37e7315&lang=fr&units=metric&exclud=name";

                // Effectuez une requête GET
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Vérifiez si la requête a réussi (code de statut 200 OK)
                if (response.IsSuccessStatusCode)
                {
                    // Lisez le contenu de la réponse
                    string content = await response.Content.ReadAsStringAsync();
                    var forecastData = JsonConvert.DeserializeObject<WeatherForecast>(content);
                    
                    if (forecastData?.list != null)
                    {
                        int i = 0;
                        NameVille1.Text = $" {forecastData.city.Name}";
                        Coords1.Text = $"lat :{forecastData.city.Coord.Lat} , lon : {forecastData.city.Coord.Lon}";
                        NameVille2.Text = $" {forecastData.city.Name}";
                        Coords2.Text = $"lat :{forecastData.city.Coord.Lat} , lon : {forecastData.city.Coord.Lon}";
                        NameVille3.Text = $" {forecastData.city.Name}";
                        Coords3.Text = $"lat :{forecastData.city.Coord.Lat} , lon : {forecastData.city.Coord.Lon}";
                        NameVille4.Text = $" {forecastData.city.Name}";
                        Coords4.Text = $"lat :{forecastData.city.Coord.Lat} , lon : {forecastData.city.Coord.Lon}";
                        NameVille5.Text = $" {forecastData.city.Name}";
                        Coords5.Text = $"lat :{forecastData.city.Coord.Lat} , lon : {forecastData.city.Coord.Lon}";
                        foreach (var forecast in forecastData.list)
                        {
                            
                            //Console.WriteLine($"Date and Time: {forecast.dt}");
                            
                            if (CompareDateNoon(forecast.dt))
                            {
                                i++;
                                switch (i)
                                {
                                    case 1:
                                        Humidite1.Text = $"Humidité : {forecast.main.Humidity}%";
                                        TempVille1.Text = $"{forecast.main.Temp}°C";
                                        Description1.Text = $"{forecast.weather[0].description}";
                                        Date1.Text = $"{forecast.dt}";

                                        Console.WriteLine(
                                            $"Date and Time: {forecast.dt}, Temperature: {forecast.main.Temp}");
                                        Console.WriteLine($"index: {i}");
                                        break;
                                    case 2:
                                        Humidite2.Text = $"Humidité : {forecast.main.Humidity}%";
                                        TempVille2.Text = $"{forecast.main.Temp}°C";
                                        Description2.Text = $"{forecast.weather[0].description}";
                                        Date2.Text = $"{forecast.dt}";

                                        Console.WriteLine(
                                            $"Date and Time: {forecast.dt}, Temperature: {forecast.main.Temp}");
                                        Console.WriteLine($"index: {i}");
                                        break;
                                    case 3:
                                        Humidite3.Text = $"Humidité : {forecast.main.Humidity}%";
                                        TempVille3.Text = $"{forecast.main.Temp}°C";
                                        Description3.Text = $"{forecast.weather[0].description}";
                                        Date3.Text = $"{forecast.dt}";

                                        Console.WriteLine(
                                            $"Date and Time: {forecast.dt}, Temperature: {forecast.main.Temp}");
                                        Console.WriteLine($"index: {i}");
                                        break;
                                    case 4:
                                        Humidite4.Text = $"Humidité : {forecast.main.Humidity}%";
                                        TempVille4.Text = $"{forecast.main.Temp}°C";
                                        Description4.Text = $"{forecast.weather[0].description}";
                                        Date4.Text = $"{forecast.dt}";

                                        Console.WriteLine(
                                            $"Date and Time: {forecast.dt}, Temperature: {forecast.main.Temp}");
                                        Console.WriteLine($"index: {i}");
                                        break;
                                    case 5:
                                        Humidite5.Text = $"Humidité : {forecast.main.Humidity}%";
                                        TempVille5.Text = $"{forecast.main.Temp}°C";
                                        Description5.Text = $"{forecast.weather[0].description}";
                                        Date5.Text = $"{forecast.dt}";

                                        Console.WriteLine(
                                            $"Date and Time: {forecast.dt}, Temperature: {forecast.main.Temp}");
                                        Console.WriteLine($"index: {i}");
                                        break;
                                }


                                //LienIcon= $"Lien icon : http://openweathermap.org/img/w/{weatherData.weather[0].Icon}.png";
                                //imageUrl = $"http://openweathermap.org/img/w/{weatherData.weather[0].Icon}.png";
                                //LoadImageFromUrl(imageUrl);
                                //Console.WriteLine($"Date and Time: {forecast.dt}, Temperature: {forecast.main.Temp}");
                            }
                            
                        }
                    }
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
    public void SearchWeather(object sender, RoutedEventArgs e)
    {
        string input = SearchBox.Text;
        test(input);
        testForecast(input);

    }

    public bool CompareDateNoon(string dateTime)
    {
        return dateTime.Substring(11) == "12:00:00";
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
        string apiUrl =
            $"http://api.openweathermap.org/geo/1.0/direct?q={input}&limit=1&appid=19e8ae246f03ffc54bbdae83a37e7315";

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
                    
                    // Check if any cities were found
                    if (Cities == null)
                    {
                        return (String.Empty, 0, 0);
                    } 
                    var City = Cities[0];
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

