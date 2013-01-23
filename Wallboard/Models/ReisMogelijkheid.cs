using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Wallboard.Models
{
    public class ReisMogelijkheid
    {
        public Melding Melding { get; set; }
        public int AantalOverstappen { get; set; }
        public TimeSpan GeplandeReisTijd { get; set; }
        public TimeSpan ActueleReisTijd { get; set; }
        public string VertrekVertraging { get; set; }
        public string AankomstVertraging { get; set; }
        public bool Optimaal { get; set; }
        public DateTime GeplandeVertrekTijd { get; set; }
        public DateTime ActueleVertrekTijd { get; set; }
        public DateTime GeplandeAankomstTijd { get; set; }
        public DateTime ActueleAankomstTijd { get; set; }        
        public ReisMogelijkheidStatus Status { get; set; }
        public List<ReisDeel> ReisDelen { get; set; }

        public ReisMogelijkheid(XmlNode node)
        {
            var navigator = node.CreateNavigator();
            AantalOverstappen = Convert.ToInt32(navigator.Evaluate("number(AantalOverstappen)"));
            GeplandeReisTijd = TimeSpan.Parse(navigator.Evaluate("string(GeplandeReisTijd)").ToString());
            ActueleReisTijd = TimeSpan.Parse(navigator.Evaluate("string(ActueleReisTijd)").ToString());
            VertrekVertraging = navigator.Evaluate("string(VertrekVertraging)").ToString();
            AankomstVertraging = navigator.Evaluate("string(AankomstVertraging)").ToString();
            Optimaal = bool.Parse(navigator.Evaluate("string(Optimaal)").ToString());
            GeplandeVertrekTijd = DateTime.ParseExact(navigator.Evaluate("string(GeplandeVertrekTijd)").ToString(), "yyyy-MM-ddTHH:mm:sszzz", null);
            ActueleVertrekTijd = DateTime.ParseExact(navigator.Evaluate("string(ActueleVertrekTijd)").ToString(), "yyyy-MM-ddTHH:mm:sszzz", null);
            GeplandeAankomstTijd = DateTime.ParseExact(navigator.Evaluate("string(GeplandeAankomstTijd)").ToString(), "yyyy-MM-ddTHH:mm:sszzz", null);
            ActueleAankomstTijd = DateTime.ParseExact(navigator.Evaluate("string(ActueleAankomstTijd)").ToString(), "yyyy-MM-ddTHH:mm:sszzz", null);

            var meldingId = navigator.Evaluate("string(Melding/Id)").ToString();
            if (!String.IsNullOrEmpty(meldingId))
            {
                Melding = new Melding()
                {
                    Id = meldingId,
                    Ernstig = bool.Parse(navigator.Evaluate("string(Melding/Ernstig)").ToString()),
                    Tekst = navigator.Evaluate("string(Melding/Text)").ToString()
                };
            }

            ReisDelen = new List<ReisDeel>();
            foreach (XmlNode n in node.SelectNodes("ReisDeel"))
            {
                ReisDelen.Add(new ReisDeel(n));
            }
        }
    }
    
    public class Melding
    {
        public string Id { get; set; }
        public bool Ernstig { get; set; }
        public string Tekst { get; set; }
    }

    public class ReisDeel
    {
        public string Vervoerder { get; set; }
        public string VervoerType { get; set; }
        public int RitNummer { get; set; }
        public ReisDeelStatus Status { get; set; }
        public List<string> ReisDetails { get; set; }
        public string GeplandeSoringId { get; set; }
        public string OngeplandeStoringId { get; set; }
        public List<ReisStop> ReisStops { get; set; }

        public ReisDeel(XmlNode node)
        {
            var navigator = node.CreateNavigator();
            Vervoerder = navigator.Evaluate("string(Vervoerder)").ToString();
            VervoerType = navigator.Evaluate("string(VervoerType)").ToString();
            RitNummer = Convert.ToInt32(navigator.Evaluate("number(RitNummer)").ToString());
            GeplandeSoringId = navigator.Evaluate("string(GeplandeSoringId)").ToString();
            OngeplandeStoringId = navigator.Evaluate("string(OngeplandeStoringId)").ToString();

            ReisStops = new List<ReisStop>();
            foreach (XmlNode n in node.SelectNodes("ReisStop"))
            {
                ReisStops.Add(new ReisStop(n));
            }
        }
    }

    public class ReisStop
    {
        public string Station { get; set; }
        public DateTime Vertrektijd { get; set; }
        public string VertrekVertraging { get; set; }
        public Spoor Spoor { get; set; }

        public ReisStop(XmlNode node)
        {
            var navigator = node.CreateNavigator();
            Station = navigator.Evaluate("string(Naam)").ToString();
            Vertrektijd = DateTime.ParseExact(navigator.Evaluate("string(Tijd)").ToString(), "yyyy-MM-ddTHH:mm:sszzz", null);
            VertrekVertraging = navigator.Evaluate("string(VertrekVertraging )").ToString();

            var spoor = navigator.Evaluate("string(Spoor)").ToString();
            if (!String.IsNullOrEmpty(spoor))
            {
                Spoor = new Spoor()
                {
                    Gewijzigd = bool.Parse(navigator.Evaluate("string(Spoor/@wijziging)").ToString()),
                    Nummer = spoor
                };
            }
        }
    }

    public enum ReisDeelStatus
    {
        VolgensPlan,
        Geannuleerd,
        Gewijzigd,
        OverstapNietMogelijk,
        Vertraagd,
        Nieuw
    }

    public enum ReisMogelijkheidStatus
    {
        VolgensPlan,
        Gewijzigd,
        Vertraagd,
        Nieuw,
        NietOptimaal,
        NietMogelijk,
        PlanGewijzidg
    }
}