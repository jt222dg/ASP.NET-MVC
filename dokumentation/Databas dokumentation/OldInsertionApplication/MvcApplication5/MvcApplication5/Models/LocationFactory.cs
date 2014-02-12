using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MvcApplication5.Models
{
    public class LocationFactory
    {
        public static Location Create(XElement row)
        {

            var node1 = row.Attribute("A");
            int? typ = null;
            if (node1 != null)
            {
                typ = int.Parse(node1.Value);
            }

            var node2 = row.Attribute("B");
            decimal? latitud = null;
            if (node2 != null)
            {
                latitud = decimal.Parse(node2.Value.Replace(".", ","));
            }

            var node3 = row.Attribute("C");
            decimal? longitude = null;
            if (node3 != null)
            {
                longitude = decimal.Parse(node3.Value.Replace(".",","));
            }

            var node4 = row.Attribute("D");
            string ritningsnamn = "null";
            if (node4 != null)
            {
                ritningsnamn = node4.Value;
            }

            var node5 = row.Attribute("E");
            string rumsnamn2 = "null";
            if (node5 != null)
            {
                rumsnamn2 = node5.Value;
            }

            var node6 = row.Attribute("F");
            string rumsnamn3 = "null";
            if (node6 != null)
            {
                rumsnamn3 = node6.Value;
            }

            var node7 = row.Attribute("G");
            string rumsnamn4 = "null";
            if (node7 != null)
            {
                rumsnamn4 = node7.Value;
            }

            var node8 = row.Attribute("H");
            string byggnad_sv = "null";
            if (node8 != null)
            {
                byggnad_sv = node8.Value;
            }

            var node9 = row.Attribute("I");
            string byggnad_en = "null";
            if (node9 != null)
            {
                byggnad_en = node9.Value;
            }

            var node10 = row.Attribute("J");
            byte? vaning = null;
            if (node10 != null)
            {
                vaning = byte.Parse(node10.Value);
            }

            var node11 = row.Attribute("K");
            string stad = "null";
            if (node11 != null)
            {
                stad = node11.Value;
            }

            var node12 = row.Attribute("L");
            string rumstyp_sv = "null";
            if (node12 != null)
            {
                rumstyp_sv = node12.Value;
            }

            var node13 = row.Attribute("M");
            string rumstyp_en = "null";
            if (node13 != null)
            {
                rumstyp_en = node13.Value;
            }

            Location n = new Location();
            n.typ = typ;
            n.latitud = latitud;
            n.longitud = longitude;
            n.ritningsnamn = ritningsnamn;
            n.rumsnamn2 = rumsnamn2;
            n.rumsnamn3 = rumsnamn3;
            n.rumsnamn4 = rumsnamn4;
            n.byggnad_sv = byggnad_sv;
            n.byggnad_en = byggnad_en;
            n.vaning = vaning;
            n.stad = stad;
            n.rumstyp_sv = rumstyp_sv;
            n.rumstyp_en = rumstyp_en;
            return n;

        }
    }
}