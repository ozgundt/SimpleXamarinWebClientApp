using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace acilistesti {
	public class HomePageData {
		public ObservableCollection<Record> Adverts {
			get; set;
		}

		public ObservableCollection<Record> Albums {
			get; set;
		}

		public ObservableCollection<Record> HeadlinesBig {
			get; set;
		}

		public ObservableCollection<Record> HeadlinesBottom {
			get; set;
		}

		public ObservableCollection<Record> HeadlinesTop {
			get; set;
		}
		public ObservableCollection<Record> LatestNews1 {
			get; set;
		}
		public ObservableCollection<Record> LatestNews2 {
			get; set;
		}

		public ObservableCollection<Record> GlobalFavorites {
			get; set;
		}

		public ObservableCollection<Record> Videos {
			get; set;
		}

	}

	public class RootPageModelView: INotifyPropertyChanged {
		
		private void NotifyPropertyChanged(string propertyName) {
			if(PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		private ObservableCollection<Record> ht;
		private ObservableCollection<Record> hb;
		private ObservableCollection<Record> hbig;
		private ObservableCollection<Record> ln1;
		private ObservableCollection<Record> ln2;
		private ObservableCollection<Record> globalFavorites;
		private ObservableCollection<Record> albums;
		private ObservableCollection<Record> videos;
		private ObservableCollection<Record> adverts;
		private ObservableCollection<Indicator> indc;
		public ObservableCollection<Indicator> HeadlinesBigIndicators {
			get => this.indc; set {
				this.indc = value;
				this.NotifyPropertyChanged("HeadlinesBigIndicators");
			}
		}
		public ObservableCollection<Record> HeadlinesTop {
			get => this.ht; set {
				this.ht = value;
				this.NotifyPropertyChanged("HeadlinesTop");
			}
		}
		public ObservableCollection<Record> HeadlinesBig {
			get => this.hb; set {
				this.hb = value;
				this.NotifyPropertyChanged("HeadlinesBig");
			}
		}
		public ObservableCollection<Record> HeadlinesBottom {
			get => this.hb; set {
				this.hb = value;
				this.NotifyPropertyChanged("HeadlinesBottom");
			}
		}
		public ObservableCollection<Record> LastNews1 {
			get => this.ln1; set {
				this.ln1 = value;
				this.NotifyPropertyChanged("LastNews1");
			}
		}
		public ObservableCollection<Record> LastNews2 {
			get => this.ln2; set {
				this.ln2 = value;
				this.NotifyPropertyChanged("LastNews2");
			}
		}
		public ObservableCollection<Record> GlobalFavorites {
			get => this.globalFavorites; set {
				this.globalFavorites = value;
				this.NotifyPropertyChanged("GlobalFavorites");
			}
		}
		public ObservableCollection<Record> Albums {
			get => this.albums; set {
				this.albums = value;
				this.NotifyPropertyChanged("Albums");
			}
		}
		public ObservableCollection<Record> Videos {
			get => this.videos; set {
				this.videos = value;
				this.NotifyPropertyChanged("Videos");
			}
		}
		public ObservableCollection<Record> Adverts {
			get => this.adverts; set {
				this.adverts = value;
				this.NotifyPropertyChanged("Adverts");
			}
		}
		
		public RootPageModelView() {
			this.HeadlinesTop = new ObservableCollection<Record>();
			this.HeadlinesBottom = new ObservableCollection<Record>();
			this.LastNews1 = new ObservableCollection<Record>();
			this.LastNews2 = new ObservableCollection<Record>();
			this.GlobalFavorites = new ObservableCollection<Record>();
			this.Albums = new ObservableCollection<Record>();
			this.Videos = new ObservableCollection<Record>();
			this.Adverts = new ObservableCollection<Record>();
		}
		public event PropertyChangedEventHandler PropertyChanged;
		
	}

	public class Indicator: INotifyPropertyChanged {
		public string Number {
			get; set;
		}

		public double ItemWidth {
			get; set;
		}

		private Color c;
		public Color Color {
			get => this.c; set {
				this.c = value;
				this.NotifyPropertyChanged("Color");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName) {
			if(PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}

	public class Record {
		public string DateTime {
			get; set;
		}

		public int ID {
			get; set;
		}

		public string ImageURL {
			get; set;
		}

		public string Title {
			get; set;
		}

		public string Type {
			get; set;
		}

		public string Url {
			get; set;
		}

		public bool IsMedia {
			get => this.Type == "M";
		}

		public bool IsVideo {
			get => this.Type == "V";
		}

		public bool HasTitle {
			get => !string.IsNullOrEmpty(this.Title);
		}
	}

	public class RecordList {
		public int TotalRecords {
			get; set;
		}

		public short TotalPages {
			get; set;
		}

		public ObservableCollection<Record> Records {
			get; set;
		}
	}
	public enum ExceptionActions {
		None,
		Login,
		Logout,
		Retry
	}
	public class ServerException {
		public ServerException() {
			this.Action = ExceptionActions.None;
		}

		public ServerException(string message) {
			this.Action = ExceptionActions.None;
			this.Message = message;
		}

		public ServerException(string message, ExceptionActions action) {
			this.Action = action;
			this.Message = message;
		}

		public ExceptionActions Action {
			get; set;
		}

		public string Description {
			get; set;
		}

		public string Message {
			get; set;
		}

		public string StackTrace {
			get; set;
		}

		public int StatusCode {
			get; set;
		}
	}

	public class ServiceResult<T> {
		public ServerException Exception {
			get; set;
		}

		public bool IsSuccess {
			get => this.Exception == null && this.Result != null;
		}

		public T Result {
			get; set;
		}
	}
	public partial class MainPage: ContentPage {
		RootPageModelView modelview = new RootPageModelView();
		public MainPage() {
			this.InitializeComponent();
			this.BindingContext = this.modelview;
			this.GetData();
		}

		public async void GetData() {

			try {
				ServiceResult<HomePageData> result = await this.GetData<HomePageData>("https://data.memurlar.net/v5/app/news/home3.json.aspx").ConfigureAwait(false);
				if(result != null && result.IsSuccess) {
					this.modelview.LastNews1 = result.Result.LatestNews1;
				}
			}
			catch(Exception ex) {

			}
		}

		public async Task<ServiceResult<T>> GetData<T>(string url, bool notimeout = false, string LogGrpupName = null) where T : class {
			ServiceResult<T> result = new ServiceResult<T>();
			try {
				using(HttpClient client = new HttpClient()) {
					try {
						using(HttpResponseMessage response = await client.GetAsync(url)) {
							if(response.IsSuccessStatusCode) {
								byte[] buffer = await response.Content.ReadAsByteArrayAsync();
								string json = Encoding.GetEncoding("UTF-8").GetString(buffer, 0, buffer.Length);
								result.Result = (T)JsonConvert.DeserializeObject(json, typeof(T));
							}
						}
					}
					catch(Exception ex) {

					}
				}
			}
			catch(Exception ex) {

			}
			return result;
		}
	}
}
