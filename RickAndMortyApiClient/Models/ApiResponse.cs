using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RickAndMortyApiClient.Models
{
   public class ApiResponse
    {
        public Info? Info { get; set; } // Info puede ser null
        public IList<Result>? Results { get; set; } // Results puede ser null
    }

    public class Info
    {
        public int count { get; set; }
        public int pages { get; set; }
        public string? next { get; set; } // next puede ser null
        public string? prev { get; set; } // prev puede ser null
    }

    public class Result
    {
        public int id { get; set; }
        public string? name { get; set; } // name puede ser null
        public string? air_date { get; set; } // air_date puede ser null
        public string? episode { get; set; } // episode puede ser null
        public IList<string>? characters { get; set; } // characters puede ser null
        public string? url { get; set; } // url puede ser null
        public DateTime created { get; set; }
    }
}