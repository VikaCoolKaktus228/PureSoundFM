using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Controls.Primitives;
using System.Security.Policy;

namespace clearsound
{
    /// <summary>
    /// Логика взаимодействия для allmusicpage.xaml
    /// </summary>
    public partial class allmusicpage : Page
    {

        private const string DeezerApiUrl = "https://api.deezer.com/search?q=top";
        public ObservableCollection<Track> Tracks { get; set; } = new ObservableCollection<Track>();
        public allmusicpage()
        {
            InitializeComponent();
            TracksList.ItemsSource = Tracks;
            LoadTracks();

        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(query))
            {
                MessageBox.Show("Введите название песни или исполнителя.");
                return;
            }

            //await SearchTracks(query);
        }
        //    private async Task SearchTracks(string query)
        //    {
        //        try
        //        {
        //            //using HttpClient client = new HttpClient();



        //            using (HttpClient client = new HttpClient())
        //            {
        //                string url = DeezerApiUrl + Uri.EscapeDataString(query);
        //                string jsonResponse = await client.GetStringAsync(url);
        //                var result = JsonConvert.DeserializeObject<DeezerResponse>(jsonResponse);
        //            }

        //            Tracks.Clear();
        //            foreach (var item in result.Data)
        //            {
        //                Tracks.Add(new Track
        //                {
        //                    Title = item.Title,
        //                    Artist = item.Artist.Name,
        //                    Duration = TimeSpan.FromSeconds(item.Duration).ToString(@"mm\:ss"),
        //                    CoverUrl = item.Album.CoverSmall
        //                });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
        //        }
        //    }
        //}
        private async void LoadTracks()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string jsonResponse = await client.GetStringAsync(DeezerApiUrl);

                    Console.WriteLine(jsonResponse);

                    
                    var result = JsonConvert.DeserializeObject<DeezerResponse>(jsonResponse);

                    Tracks.Clear();
                    foreach (var item in result.Data)
                    {
                        Tracks.Add(new Track
                        {
                            Title = item.Title,
                            Artist = item.Artist.Name,
                            Duration = TimeSpan.FromSeconds(item.Duration).ToString(@"mm\:ss"),
                            CoverUrl = item.Album.CoverSmall
                        });
                    }

                    MessageBox.Show($"Загружено треков: {Tracks.Count}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        public class Track
        {
            public string Title { get; set; }
            public string Artist { get; set; }
            public string Duration { get; set; }
            public string CoverUrl { get; set; }
        }

        public class DeezerResponse
        {
            public DeezerTrack[] Data { get; set; }
        }

        public class DeezerTrack
        {
            public string Title { get; set; }
            public int Duration { get; set; }
            public DeezerArtist Artist { get; set; }
            public DeezerAlbum Album { get; set; }
        }

        public class DeezerArtist
        {
            public string Name { get; set; }
        }

        public class DeezerAlbum
        {
            public string CoverSmall { get; set; }
        }
    }
}
