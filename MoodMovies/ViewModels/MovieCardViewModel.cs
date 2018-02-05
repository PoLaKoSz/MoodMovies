using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using MoodMovies.Resources;
using MoodMovies.Views;
using Newtonsoft.Json;

namespace MoodMovies.ViewModels
{
    public class MovieCardViewModel: Screen
    {
        public MovieCardViewModel()
        {
            TestMethod();
        }

        private string title;
        public string Title { get => title; set { title = value; NotifyOfPropertyChange(); } }
        private Uri imagepath;
        public Uri ImagePath { get => imagepath; set { imagepath = value; NotifyOfPropertyChange(); } }
        private string _overview;
        public string Overview { get => _overview; set { _overview = value; NotifyOfPropertyChange(); } }
        private string _releaseDate;
        public string ReleaseDate { get => _releaseDate; set { _releaseDate = value; NotifyOfPropertyChange(); } }
        private string _voteCount;
        public string VoteCount { get => _voteCount; set { _voteCount = value; NotifyOfPropertyChange(); } }
        private double _popularity;
        public double Popularity { get => _popularity; set { _popularity = value; NotifyOfPropertyChange(); } }
        private string _language;
        public string Language { get => _language; set { _language = value; NotifyOfPropertyChange(); } }

        
        
        
        
        
        private void TestMethod()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.themoviedb.org/3/search/movie/?api_key=6d4b546936310f017557b2fb498b370b&query=007");

            request.Method = "GET";
            //request.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 57.0) Gecko / 20100101 Firefox / 57.0";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
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
                    catch (Exception ex1)
                    {
                        
                    }

                }
            }
            catch (Exception ex)
            {
                
            }

            try
            {
                
                var model = JsonConvert.DeserializeObject<RootObject>(content);
                string address = "https://image.tmdb.org/t/p/w500/";
                //foreach (var result in model.results)
                //{
                //    address = "";
                //    testBox.Text += $"{result.title} ";
                //    testBox.Text += $"{result.id}\n";
                //    address += $"{result.poster_path}";
                //}
                Title += $"{model.results[0].title} ";
                address += model.results[0].poster_path;
                Overview += model.results[0].overview;
                ReleaseDate += model.results[0].release_date;
                Popularity += model.results[0].popularity;
                Language += model.results[0].original_language;
                VoteCount += model.results[0].vote_count;
                Console.WriteLine(address);
                ImagePath = new Uri(address);
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class Result
    {
        public int vote_count { get; set; }
        public int id { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public string title { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public List<int> genre_ids { get; set; }
        public string backdrop_path { get; set; }
        public bool adult { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
    }

    public class RootObject
    {
        public int page { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public List<Result> results { get; set; }
    }
}
