using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Wallboard.Models;
using Wallboard.Utils;

namespace WallboardTest
{    
    public class NsApiClientTests
    {
        
        public static void TestActueleTijden()
        {
            List<VertrekkendeTrein> vertrekkendeTreinen = new List<VertrekkendeTrein>();

            string data = File.ReadAllText("VertrekkendeTreinenData.xml");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);

            foreach (XmlNode node in doc.SelectNodes("//ActueleVertrekTijden/VertrekkendeTrein"))
            {
                vertrekkendeTreinen.Add(new VertrekkendeTrein(node));
            }

        }

        public static void TestReisAdvies()
        {
            List<ReisMogelijkheid> reisModelijkheden = new List<ReisMogelijkheid>();
            string data = File.ReadAllText("ReisMogelijkheidData.xml");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);

            foreach (XmlNode node in doc.SelectNodes("//ReisMogelijkheden/ReisMogelijkheid"))
            {
                reisModelijkheden.Add(new ReisMogelijkheid(node));
            }

        }
    }
}
