using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wallboard.Models
{
    public class NsPage
    {
        public List<VertrekkendeTrein> VertrekkendeTreinen { get; set; }
        public Dictionary<string, List<ReisMogelijkheid>> ReisAdviesen { get; set; }
    }
}