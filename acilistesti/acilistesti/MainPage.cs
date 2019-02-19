using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace acilistesti {
	public class MainPage: ContentPage {
		public MainPage() {
			this.GetJSON();
		}

		public async void GetJSON() {
			try {
				NativeMessageHandler h = new NativeMessageHandler();
				var client = new HttpClient(h);
				var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
				string json = await response.Content.ReadAsStringAsync();
				ListView listViewJson = new ListView();
				if(json != "") {
					ObservableCollection<Record> rootObject = JsonConvert.DeserializeObject<ObservableCollection<Record>>(json);
					DataTemplate template = new DataTemplate(typeof(TextCell));
					template.SetBinding(TextCell.TextProperty, "title");
					template.SetBinding(TextCell.DetailProperty, "id");
					listViewJson.ItemTemplate = template;
					listViewJson.ItemsSource = rootObject;
					this.Content = listViewJson;
				}
			}
			catch(InvalidCastException e) {
				throw e;
			}
		}

		public class RootObject {
			public List<Record> LatestNews1 {
				get; set;
			}

		}
		[Preserve(AllMembers = true)]
		public class Record {

			[JsonConstructor]
			public Record() {
			}

			public string title {
				get; set;
			}

			public string body {get; set;}
			public int userId {
				get; set;
			}

			public int id {
				get; set;
			}
		}
	}
}