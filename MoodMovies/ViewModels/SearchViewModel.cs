using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Models;
using MoodMovies.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoodMovies.ViewModels
{
    internal class SearchViewModel : Screen
    {
        public SearchViewModel(IEventAggregator _event)
        {
            eventAgg = _event;

            //Assign Commands
            SimpleSearchCommand = new RelayCommand(SimpleSearch, CanExecuteSimpleSearch);
        }

        #region Events
        public IEventAggregator eventAgg;
        #endregion

        #region Command Related Stuff
        public ICommand SimpleSearchCommand { get; set; }

        /// <summary>
        /// Check to see if we can execute our Method
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool CanExecuteSimpleSearch(object parameter)
        {
            if (string.IsNullOrEmpty(SimpleSearchBox))
            {
                return false;
            }
            else
            {
                return SimpleSearchBox != "";
            }
        }
        #endregion

        #region Properties
        private string _simpleSearchBox;
        public string SimpleSearchBox { get => _simpleSearchBox; set { _simpleSearchBox = value; NotifyOfPropertyChange(); } }
        private string _searchText;
        public string SearchText { get => _searchText; set { _searchText = value; NotifyOfPropertyChange(); } }

        private string SearchContent;

        private readonly List<MovieSearchResult> MovieSet = new List<MovieSearchResult>();
        private readonly List<RootObject> Pages = new List<RootObject>();
        #endregion

        public void SimpleSearch(object obj)
        {
            if (MovieSet != null)
            {
                MovieSet.Clear();
                Pages.Clear();
            }
            SearchText = obj as string;
            SearchText.Trim();
            SearchText = "query=" + SearchText;
            CallApi(CreateQueryCode(SearchText));
            GetAllPages();
            PublishResults();
        }

        private string CreateQueryCode(string command)
        {
            return "https://api.themoviedb.org/3/search/movie/?api_key=6d4b546936310f017557b2fb498b370b&" + command;
        }

        private void CallApi(string queryCode)
        {
            // Temp code to be moved to external class  

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(queryCode);
            request.Method = "GET";
            //request.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 57.0) Gecko / 20100101 Firefox / 57.0";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception)
            {
                response = null;
            }

            SearchContent = string.Empty;

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    SearchContent = sr.ReadToEnd();
                }
            }
        }

        private void GetAllPages()
        {
            try
            {
                var model = JsonConvert.DeserializeObject<RootObject>(SearchContent);
                Pages.Add(model);
                if (model.Total_pages > 1)
                {
                    for (int i = 2; i <= model.Total_pages; i++)
                    {
                        CallApi(CreateQueryCode(SearchText + $"&page={i}"));
                        var mod = JsonConvert.DeserializeObject<RootObject>(SearchContent);
                        Pages.Add(mod);
                    }
                }
            }
            catch
            {

            }
        }

        private void PublishResults()
        {
            const string address = "https://image.tmdb.org/t/p/w500/";

            foreach (var page in Pages)
            {
                //loop through the results
                foreach (var result in page.Results)
                {
                    MovieSet.Add(new MovieSearchResult
                    {
                        Vote_count = result.Vote_count,
                        Id = result.Id,
                        Video = result.Video,
                        Vote_average = result.Vote_average,
                        Title = result.Title,
                        Popularity = result.Popularity,
                        Poster_path = address + result.Poster_path,
                        Original_language = result.Original_language,
                        Original_title = result.Original_title,
                        Genre_ids = result.Genre_ids,
                        Backdrop_path = result.Backdrop_path,
                        Adult = result.Adult,
                        Overview = result.Overview,
                        Release_date = result.Release_date
                    });
                }
            }

            eventAgg.BeginPublishOnUIThread(new MovieListMessage(MovieSet, true, SearchText));
        }
    }
}
