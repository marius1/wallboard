using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Wallboard.Models
{
    public class VertrekkendeTrein
    {
        public int RitNummer { get; set; }
        public DateTime VertrekTijd { get; set; }
        public Vertraging VertrekVertraging { get; set; }
        public string EindBestemming { get; set; }
        public string TreinSoort { get; set; }
        public string Route { get; set; }
        public string Vervoerder { get; set; }
        public Spoor VertrekSpoor { get; set; }
        public List<string> Opmerkingen { get; set; }
        public string ReisTip { get; set; }

        public VertrekkendeTrein(XmlNode node)
        {
            var navigator = node.CreateNavigator();

            RitNummer = Convert.ToInt32(navigator.Evaluate("number(RitNummer)"));
            VertrekTijd = DateTime.ParseExact(navigator.Evaluate("string(VertrekTijd)").ToString(), "yyyy-MM-ddTHH:mm:sszzz", null);

            var vertrekVertraging = navigator.Evaluate("string(VertrekVertraging)").ToString();
            if (!String.IsNullOrEmpty(vertrekVertraging))
            {
                VertrekVertraging = new Vertraging()
                {
                    Duur = XmlConvert.ToTimeSpan(vertrekVertraging),
                    Tekst = navigator.Evaluate("string(VertrekVertragingTekst)").ToString()
                };
            }

            EindBestemming = navigator.Evaluate("string(EindBestemming)").ToString();
            TreinSoort = navigator.Evaluate("string(TreinSoort)").ToString();
            Route = navigator.Evaluate("string(RouteTekst)").ToString();
            Vervoerder = navigator.Evaluate("string(Vervoerder)").ToString();

            VertrekSpoor = new Spoor()
            {
                Gewijzigd = bool.Parse(navigator.Evaluate("string(VertrekSpoor/@wijziging)").ToString()),
                Nummer = navigator.Evaluate("string(VertrekSpoor)").ToString()
            };

            Opmerkingen = new List<string>();
            foreach (XmlNode a in node.SelectNodes("Opmerkingen/Opmerking"))
            {
                Opmerkingen.Add(a.InnerText.Trim());
            }

            ReisTip = navigator.Evaluate("string(ReisTip)").ToString();
        }
    }

    public class Vertraging
    {
        public TimeSpan Duur { get; set; }
        public string Tekst { get; set; }
    }
       
    public class Spoor
    {
        public string Nummer { get; set; }
        public bool Gewijzigd { get; set; }
    }
}