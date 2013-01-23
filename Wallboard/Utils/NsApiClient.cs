using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Wallboard.Models;

namespace Wallboard.Utils
{
    public class NsApiClient
    {
        private const string BASE_URL = "http://webservices.ns.nl/";
        private WebClient webClient;

        public NsApiClient()
        {
            webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["NsApiUser"], ConfigurationManager.AppSettings["NsApiPassword"]);
        }

        public List<VertrekkendeTrein> GetActueleVertrektijden(string station)
        {
            List<VertrekkendeTrein> vertrekkendeTreinen = new List<VertrekkendeTrein>();
            string data = DoRequest(String.Format("ns-api-avt?station={0}", station));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);

            
            foreach (XmlNode node in doc.SelectNodes("//ActueleVertrekTijden/VertrekkendeTrein"))
            {
                vertrekkendeTreinen.Add(new VertrekkendeTrein(node));
            }

            return vertrekkendeTreinen.OrderBy(x => x.VertrekTijd).ToList();
        }

        public List<ReisMogelijkheid> GetReisAdvies(string from, string to)
        {
            List<ReisMogelijkheid> reisModelijkheden = new List<ReisMogelijkheid>();

            string dateTime = DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-ddTHH:mm:ss");
            string data = DoRequest(String.Format("ns-api-treinplanner?fromStation={0}&toStation={1}&dateTime={2}&previousAdvices=0", from, to, dateTime));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);

            foreach (XmlNode node in doc.SelectNodes("//ReisMogelijkheden/ReisMogelijkheid"))
            {
                reisModelijkheden.Add(new ReisMogelijkheid(node));
            }

            return reisModelijkheden.OrderBy(x => x.GeplandeVertrekTijd).ToList();
        }

        private string DoRequest(string requestUrl)
        {
            string fullRequestUrl = BASE_URL + requestUrl;         

            using (Stream data = webClient.OpenRead(fullRequestUrl))
            {
                return new StreamReader(data).ReadToEnd();
            }
        }
    }
}