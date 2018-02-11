using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MoodMovies.Resources;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using MoodMovies.Models;

namespace MoodMovies.ViewModels
{
    public class MainViewModel : Conductor<Screen>.Collection.OneActive, IHandle<string>
    {
        // constructor
        public MainViewModel()
        {
            events = new EventAggregator();
            events.Subscribe(this);
            InitialiseVMs();
            ActivateItem(MovieListVM);
            
            //Assign Commands
            SimpleSearchCommand = new RelayCommand(SimpleSearch, CanExecuteSimpleSearch);
        }

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
                if (SimpleSearchBox != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region General Properties
        private string _simpleSearchBox;
        public string SimpleSearchBox { get => _simpleSearchBox; set { _simpleSearchBox = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Child View Models
        private MovieListViewModel movieListVM;
        public MovieListViewModel MovieListVM { get => movieListVM; set { movieListVM = value; NotifyOfPropertyChange(); } }
        private AboutViewModel aboutVM;
        public AboutViewModel AboutVM { get => aboutVM; set { aboutVM = value; NotifyOfPropertyChange(); } }
        private FavouritesViewModel _favouriteVM;
        public FavouritesViewModel FavouriteVM { get => _favouriteVM; set { _favouriteVM = value; NotifyOfPropertyChange(); } }
        private FavActorViewModel _favActorVM;
        public FavActorViewModel FavActorVM { get => _favActorVM; set { _favActorVM = value; NotifyOfPropertyChange(); } }
        private ASearchViewModel _asearchVM;
        public ASearchViewModel ASearchVM { get => _asearchVM; set { _asearchVM = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Events
        EventAggregator events;
        #endregion

        #region Public Methods
        public void CloseApp()
        {
            TryClose();
        }        
        public void SimpleSearch(object obj)
        {
            //call external class function to perform async api call TBD***
            //prepare the search string
            string searchText = obj as string;
            searchText.Trim();

            #region Temp code to be moved to external class  
            string queryCode = "https://api.themoviedb.org/3/search/movie/?api_key=6d4b546936310f017557b2fb498b370b&query=";
            queryCode += searchText;
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

            string content = string.Empty;
            try
            {
                using (Stream stream = response.GetResponseStream())
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                    catch (Exception )
                    {

                    }

                }
            }
            catch (Exception )
            {

            }
            #endregion

            #region This code must also be moved and must return a collection of MovieResultLists that can be published via en event to the MovieListViewModel
            try
            {
                var model = JsonConvert.DeserializeObject<RootObject>(content);
                string address = "https://image.tmdb.org/t/p/w500/";
                                
                List<MovieSearchResult> movieSet = new List<MovieSearchResult>();

                //loop through the results
                foreach (var result in model.Results)
                {
                    movieSet.Add(new MovieSearchResult
                    {
                        Vote_count = result.Vote_count,
                        Id = result.Id,
                        Video = result.Video,
                        Vote_average =result.Vote_average ,
                        Title = result.Title,
                        Popularity = result.Popularity,
                        Poster_path = address + result.Poster_path,
                        Original_language = result.Original_language ,
                        Original_title = result.Original_title,
                        Genre_ids  = result.Genre_ids,
                        Backdrop_path = result.Backdrop_path,
                        Adult = result.Adult,
                        Overview = result.Overview,
                        Release_date = result.Release_date 
                        });                    
                }

                events.BeginPublishOnUIThread(new MovieListMessage(movieSet));
                //ImagePath = new Uri(address);
            }
            catch (Exception)
            {
            }
            #endregion
        }
        #endregion

        #region Private Methods
        private void InitialiseVMs()
        {
            //pages that will change
            Items.Add( MovieListVM = new MovieListViewModel(events) );
            Items.Add( FavActorVM = new FavActorViewModel() );
            Items.Add( AboutVM = new AboutViewModel() );
            Items.Add( FavouriteVM = new FavouritesViewModel() );

            events.Subscribe(MovieListVM);

            //static, will not change
            ASearchVM = new ASearchViewModel();
        }       
        #endregion

        #region Item Activation Methods
        public void DisplayMovieListVM()
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(MovieListVM);
        }

        public void DisplayAboutVM()            
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(AboutVM);            
        }

        public void DisplayFavouriteVM()
        {
        DeactivateItem(ActiveItem, true);
        ActivateItem(FavouriteVM);
        }
        
        public void DisplayFavActorVM()
        {
        DeactivateItem(ActiveItem, true);
        ActivateItem(FavActorVM);
        }
        #endregion

        #region IHandle Interface
        /// <summary>
        /// Handles ID received from MovieCard
        /// </summary>
        /// <param name="message"></param>
        public void Handle(string message)
        {
            // outsource the logic containg the call to the api
            string queryCode = "https://api.themoviedb.org/3/movie/" + message.Trim() + "/videos?api_key=6d4b546936310f017557b2fb498b370b";            
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

            string content = string.Empty;
            try
            {
                using (Stream stream = response.GetResponseStream())
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                    catch (Exception)
                    {

                    }

                }
            }
            catch (Exception)
            {

            }

            try
            {
                var model = JsonConvert.DeserializeObject<RootTrailer>(content);
                var key = model.Results.Select(x => x.Key).First();
                string address = "http://www.youtube.com/watch?v=" + key;

                events.BeginPublishOnUIThread(new TrailerMessage(address));

            }
            catch (Exception)
            {
            }
        }
        #endregion

    }
}