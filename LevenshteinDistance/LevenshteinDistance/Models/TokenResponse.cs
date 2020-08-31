using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LevenshteinDistance.Models
{
    public class TokenResponse
    {
        public string Status { get; set; }
        public string JwtToken { get; set; }
        public string Message { get; set; }
    }
}