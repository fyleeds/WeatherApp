using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using HarfBuzzSharp;
using Newtonsoft.Json;
namespace WeatherApp;
using Avalonia;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Les classes et les propriétés
public class WeatherForecast
{
    // Les propriétés de la classe
    public List<Forecast> list { get; set; }
    [JsonProperty("city")]
    public City city { get; set; }
}

public class Forecast
{
    // Les propriétés de la classe
    [JsonProperty("main")]
    public Main main { get; set; }
    [JsonProperty("weather")]
    public List<Weather2> weather { get; set; }
    [JsonProperty("dt_txt")]
    public string dt { get; set; }
}

public class Main
{
    // Les propriétés de la classe
    [JsonProperty("temp")]
    public double Temp { get; set; }
    
    [JsonProperty("humidity")]
    public int Humidity { get; set; }
}

public class Weather2
{
    // Les propriétés de la classe
    [JsonProperty("description")]
    public string description { get; set; }
    [JsonProperty("icon")]
    public string icon { get; set; }
}

public class City
{
    // Les propriétés de la classe
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("coord")] public Coordinates Coord { get; set; }
}

public class Coordinates
{
    // Les propriétés de la classe
    [JsonProperty("lat")]
    public double Lat { get; set; }
    [JsonProperty("lon")]
    public double Lon { get; set; }
}
//fin de la section Api MeteoPrevu

public class weatherData
{
    // Les propriétés de la classe
    public List<Weather> weather { get; set; }

    public MainData main { get; set; }

    public string name { get; set; }
    
    public Coord coord { get; set; }


}

public class Weather
{
    // Les propriétés de la classe
    public int Id { get; set; }
    public string Main { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}

public class MainData
{
    // Les propriétés de la classe
    public double Temp { get; set; }
    public int Humidity { get; set; }
}

public class Coord
{
    // Les propriétés de la classe
    public double lon { get; set; }
    public double lat { get; set; }
}


public class Location
{
    // Les propriétés de la classe
    [JsonProperty("name")]
    public string NomVille { get; set; }
    [JsonProperty("lat")]
    public double Latitude { get; set; }
    [JsonProperty("lon")]
    public double Longitude { get; set; }
}

// La classe principale de l'application
public partial class MainWindow : Window
{
    // Les variables de la classe
    public String imageUrl;

    public MainWindow()
    {
        InitializeComponent();
        // Initialisation des composants de l'interface
        
    }

    public async void ChargerMeteoActuelle(string input, string lang="fr")
    {
        // Code asynchrone pour tester la récupération des données météorologiques
        using (HttpClient client = new HttpClient())
        {
            try
            {
                Location newLocation = new Location {};
                (newLocation.NomVille,newLocation.Latitude,newLocation.Longitude) = await ObtenirCoordonneesVilleAsync(input);

                // Spécifiez l'URL de l'API que vous souhaitez interroger
                string apiUrl =
                    $"https://api.openweathermap.org/data/2.5/weather?lat={newLocation.Latitude}&lon={newLocation.Longitude}&appid=19e8ae246f03ffc54bbdae83a37e7315&lang={lang}&units=metric&exclud=name";

                // Effectuez une requête GET
                HttpResponseMessage réponse = await client.GetAsync(apiUrl);

                // Vérifiez si la requête a réussi (code de statut 200 OK)
                if (réponse.IsSuccessStatusCode)
                {
                    // Lisez le contenu de la réponse
                    string contenu = await réponse.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<weatherData>(contenu);
                    NomVille.Text = $"Ville : {weatherData.name}";
                    TempVille.Text = $"{weatherData.main.Temp}°C";
                    Humidite.Text = $"Humidité : {weatherData.main.Humidity}%";
                    Description.Text = $"{weatherData.weather[0].Description}";
                    LatLong.Text = $"lat :{weatherData.coord.lat}, lon :{weatherData.coord.lon}";
                    imageUrl = $"http://openweathermap.org/img/w/{weatherData.weather[0].Icon}.png";
                    ChargerImageDepuisUrl(imageUrl,"MeteoImage");
                }
                else
                {
                    Console.WriteLine($"La requête a échoué avec le code de statut : {réponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
        }
    }
    public async void ChargerMeteoPrevu(string input,string lang="fr")
    {
        // Code asynchrone pour tester la récupération des prévisions météorologiques
    
        using (HttpClient client = new HttpClient())
        {
            try
            {
                Location newLocation = new Location {};
                (newLocation.NomVille,newLocation.Latitude,newLocation.Longitude) = await ObtenirCoordonneesVilleAsync(input);

                // Spécifiez l'URL de l'API que vous souhaitez interroger
                string apiUrl =
                    $"https://api.openweathermap.org/data/2.5/forecast?lat={newLocation.Latitude}&lon={newLocation.Longitude}&appid=19e8ae246f03ffc54bbdae83a37e7315&lang={lang}&units=metric&exclud=name";

                // Effectuez une requête GET
                HttpResponseMessage réponse = await client.GetAsync(apiUrl);

                // Vérifiez si la requête a réussi (code de statut 200 OK)
                if (réponse.IsSuccessStatusCode)
                {
                    // Lisez le contenu de la réponse
                    string contenu = await réponse.Content.ReadAsStringAsync();
                    var forecastData = JsonConvert.DeserializeObject<WeatherForecast>(contenu);
                    
                    if (forecastData?.list != null)
                    {
                        int i = 0;
                        NomVille1.Text = $" {forecastData.city.Name}";
                        Coords1.Text = $"lat :{forecastData.city.Coord.Lat} , lon : {forecastData.city.Coord.Lon}";
                        NomVille2.Text = $" {forecastData.city.Name}";
                        Coords2.Text = $"lat :{forecastData.city.Coord.Lat} , lon : {forecastData.city.Coord.Lon}";
                        NomVille3.Text = $" {forecastData.city.Name}";
                        Coords3.Text = $"lat :{forecastData.city.Coord.Lat} , lon : {forecastData.city.Coord.Lon}";
                        NomVille4.Text = $" {forecastData.city.Name}";
                        Coords4.Text = $"lat :{forecastData.city.Coord.Lat} , lon : {forecastData.city.Coord.Lon}";
                        NomVille5.Text = $" {forecastData.city.Name}";
                        Coords5.Text = $"lat :{forecastData.city.Coord.Lat} , lon : {forecastData.city.Coord.Lon}";
                        foreach (var forecast in forecastData.list)
                        {
                            
                            if (ComparerDateMidi(forecast.dt))
                            {
                                i++;
                                switch (i)
                                {
                                    case 1:
                                        Humidite1.Text = $"Humidité : {forecast.main.Humidity}%";
                                        TempVille1.Text = $"{forecast.main.Temp}°C";
                                        Description1.Text = $"{forecast.weather[0].description}";
                                        Date1.Text = $"{ConvertirDate(forecast.dt)}";
                                        imageUrl = $"http://openweathermap.org/img/w/{forecast.weather[0].icon}.png";
                                        ChargerImageDepuisUrl(imageUrl,"MeteoImage1");

                                        
                                        break;
                                    case 2:
                                        Humidite2.Text = $"Humidité : {forecast.main.Humidity}%";
                                        TempVille2.Text = $"{forecast.main.Temp}°C";
                                        Description2.Text = $"{forecast.weather[0].description}";
                                        Date2.Text = $"{ConvertirDate(forecast.dt)}";
                                        imageUrl = $"http://openweathermap.org/img/w/{forecast.weather[0].icon}.png";
                                        ChargerImageDepuisUrl(imageUrl,"MeteoImage2");

                                        
                                        break;
                                    case 3:
                                        Humidite3.Text = $"Humidité : {forecast.main.Humidity}%";
                                        TempVille3.Text = $"{forecast.main.Temp}°C";
                                        Description3.Text = $"{forecast.weather[0].description}";
                                        Date3.Text = $"{ConvertirDate(forecast.dt)}";
                                        imageUrl = $"http://openweathermap.org/img/w/{forecast.weather[0].icon}.png";
                                        ChargerImageDepuisUrl(imageUrl,"MeteoImage3");

                                        
                                        break;
                                    case 4:
                                        Humidite4.Text = $"Humidité : {forecast.main.Humidity}%";
                                        TempVille4.Text = $"{forecast.main.Temp}°C";
                                        Description4.Text = $"{forecast.weather[0].description}";
                                        Date4.Text = $"{ConvertirDate(forecast.dt)}";
                                        imageUrl = $"http://openweathermap.org/img/w/{forecast.weather[0].icon}.png";
                                        ChargerImageDepuisUrl(imageUrl,"MeteoImage4");

                                        
                                        break;
                                    case 5:
                                        Humidite5.Text = $"Humidité : {forecast.main.Humidity}%";
                                        TempVille5.Text = $"{forecast.main.Temp}°C";
                                        Description5.Text = $"{forecast.weather[0].description}";
                                        Date5.Text = $"{ConvertirDate(forecast.dt)}";
                                        imageUrl = $"http://openweathermap.org/img/w/{forecast.weather[0].icon}.png";
                                        ChargerImageDepuisUrl(imageUrl,"MeteoImage5");

                                        
                                        break;
                                }
                                
                            }
                            
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"La requête a échoué avec le code de statut : {réponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
        }
    }
    public void RechercherMeteo(object sender, RoutedEventArgs e)
    {
        // Méthode pour déclencher la recherche météorologique
        string entrée = SearchBox.Text;
        ChargerMeteoActuelle(entrée);
        ChargerMeteoPrevu(entrée);

    }
    
    public void ChangeLangue(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox)
        { 
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem?.Tag is string abbreviation)
            {
                // Utilisez l'abréviation ici
                ChargerMeteoActuelle(SearchBox.Text,abbreviation);
                ChargerMeteoPrevu(SearchBox.Text,abbreviation);
            }
        }

    }

    public bool ComparerDateMidi(string dateTime)
    {
        // Méthode pour comparer les dates
        return dateTime.Substring(11) == "12:00:00";
    }
    
    public string ConvertirDate(string dateString)
    {
        // Méthode pour convertir les dates
        CultureInfo cultureInfo = new CultureInfo("fr-FR");

        // date en string convertie en dateTime 
        DateTime dateFormaté = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", cultureInfo);
        
        // formater en date francaise
        return dateFormaté.ToString("dddd, d MMMM yyyy 'à' HH:mm:ss", cultureInfo);

    }
 
    public async void ChargerImageDepuisUrl(string imageUrl,string NomImage)
    {
        // Méthode asynchrone pour charger une image depuis une URL
        try
        {
            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                using (var stream = new MemoryStream(imageBytes))
                {
                    var bitmap = new Bitmap(stream);
                    var MeteoImageRef = this.FindControl<Image>(NomImage);
                    MeteoImageRef.Source = bitmap;

                }
            }
        }
        catch (Exception ex)
        {
            // Handle any potential exceptions, such as network errors or invalid URLs.
            Console.WriteLine("Error loading image: " + ex.Message);
        }
    }

    public async Task<(string NomVille, double Lat, double Lon)> ObtenirCoordonneesVilleAsync(string input)
    {
        // Méthode asynchrone pour obtenir les coordonnées d'une ville
        string apiUrl =
            $"http://api.openweathermap.org/geo/1.0/direct?q={input}&limit=1&appid=19e8ae246f03ffc54bbdae83a37e7315";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Envoyez une requête GET à l'URL spécifiée
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    // Lisez le contenu de la réponse sous forme de chaîne
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    // Lister le nom de toutes les villes
                    var Villes = JsonConvert.DeserializeObject<List<Location>>(jsonContent);
                
                    // Vérifiez si des villes ont été trouvées
                    if (Villes == null)
                    {
                        return (string.Empty, 0, 0);
                    } 
                    var Ville = Villes[0];
                    return (Ville.NomVille, Ville.Latitude, Ville.Longitude);
                
                }
                else
                {
                    Console.WriteLine($"Erreur HTTP : {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        return (string.Empty, 0, 0);
    }

    

}

