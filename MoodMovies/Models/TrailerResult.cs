﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Models
{
    public class Result
    {
        public string Id { get; set; }
        public string Iso_639_1 { get; set; }
        public string Iso_3166_1 { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Site { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
    }

    public class RootTrailer
    {
        public int Id { get; set; }
        public List<Result> Results { get; set; }
    }
}
