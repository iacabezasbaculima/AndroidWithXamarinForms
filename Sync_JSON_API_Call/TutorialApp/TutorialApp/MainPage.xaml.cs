using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace TutorialApp
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage
	{
		private const string _API_KEY = "YOUR_API_KEY";
		public MainPage()
		{
			InitializeComponent();

			GetData("london");
		}

		public void GetData(string location)
		{
			try
			{
				var client = new HttpClient();
				client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
				var query = $"weather?q={location}&type=accurate&units=metric&mode=json&appid={_API_KEY}";
				var json = client.GetStringAsync(query).Result;	// sync call

				var data = JsonConvert.DeserializeObject<MyData.RootObject>(json);

				CityName.Text = data.name;
				// alt + 0176 = degree symbol
				Temperature.Text = $"{data.main.temp} °C";
				Description.Text = data.weather[0].description;
				MaxTemperature.Text = $"Maximum Temperature: {data.main.temp_max}";
				MinTemperature.Text = $"Minimum Temperature: {data.main.temp_min}";
				Pressure.Text = $"Pressure: {data.main.pressure} %";
				Humidity.Text = $"Humidity: {data.main.humidity} %";
			}
			catch (HttpRequestException e)
			{
				//Console.WriteLine($"GetData ERROR: {e.Message}");
				throw new HttpRequestException("GetData ERROR:", e);
			}
			catch(AggregateException e)
			{
				throw new AggregateException("Please check that your Android device has WiFi or mobile data enabled:\n", e);
			}
		}

		class MyData
		{
			public class Coord
			{
				public double lon { get; set; }
				public double lat { get; set; }
			}
			public class Weather
			{
				public int id { get; set; }
				public string main { get; set; }
				public string description { get; set; }
				public string icon { get; set; }
			}
			public class Main
			{
				public double temp { get; set; }
				public int pressure { get; set; }
				public int humidity { get; set; }
				public double temp_min { get; set; }
				public double temp_max { get; set; }
			}
			public class Wind
			{
				public double speed { get; set; }
				public int deg { get; set; }
			}
			public class Clouds
			{
				public int all { get; set; }
			}
			public class Sys
			{
				public int type { get; set; }
				public int id { get; set; }
				public string country { get; set; }
				public int sunrise { get; set; }
				public int sunset { get; set; }
			}

			public class RootObject
			{
				public Coord coord { get; set; }
				public List<Weather> weather { get; set; }
				public string @base { get; set; }
				public Main main { get; set; }
				public int visibility { get; set; }
				public Wind wind { get; set; }
				public Clouds clouds { get; set; }
				public int dt { get; set; }
				public Sys sys { get; set; }
				public int timezone { get; set; }
				public int id { get; set; }
				public string name { get; set; }
				public int cod { get; set; }
			}
		}
	}
}
