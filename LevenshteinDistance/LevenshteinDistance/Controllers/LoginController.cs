using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using LevenshteinDistance.Models;

namespace LevenshteinDistance.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {  
            return View();
        }
        
        public ActionResult CalculateDistance(string value1, string value2)
        {           
            Tuple<string, int> result = LevenshteinDistanceAlgorithm.CalculateLevenshteinDistance(value1, value2);
            var matrixValue = result.Item1;
            var calculatedDistance = result.Item2;
            var data = new { Result = matrixValue, CalculatedDistance = calculatedDistance };

            return Json(data: data, behavior: JsonRequestBehavior.AllowGet);
        }
    }
}