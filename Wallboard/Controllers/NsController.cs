using Wallboard.Models;
using Wallboard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wallboard.Controllers
{
    public class NsController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var nsApi = new NsApiClient();
            NsPage nsPage = new NsPage();
            nsPage.VertrekkendeTreinen = nsApi.GetActueleVertrektijden("Amsterdam Amstel");

            List<string> toStations = new List<string>() { "Alkmaar", "Utrecht", "Hoorn" };

            var reisAdviesen = new Dictionary<string, List<ReisMogelijkheid>>();
            foreach (string station in toStations)
            {
                reisAdviesen.Add(station, nsApi.GetReisAdvies("Amsterdam Amstel", station));
            }

            nsPage.ReisAdviesen = reisAdviesen;
            
            return View(nsPage);
        }
    }
}
