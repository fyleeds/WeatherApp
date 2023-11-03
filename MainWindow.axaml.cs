using Avalonia.Controls;
using System;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
namespace WeatherApp;

public partial class MainWindow : Window
{
    
    // private string json = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}";
    // Person person = new Person(); // Assuming Person is a class you have defined.
    // Person person = JsonConvert.DeserializeObject<Person>(json);
    //
    //
    public MainWindow()
    {
        InitializeComponent();
        T1.Text = "Today 10° C";
        T2.Text = "Partially Cloudy";
        T3.Text = "Precipitation: 25 %";
        T4.Text = "High:20°C";
        T5.Text = "Low 0°C";
        T6.Text = "Feels like : 10°C";
    }


    // private void GetWeatherData(string location)
    // {
    //     var client = new RestClient();
    //     var request = new RestRequest()
    // }
}