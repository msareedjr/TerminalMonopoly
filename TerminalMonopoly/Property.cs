using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerminalMonopoly
{
    class Property : PaidSpace
    {

        private int rent;
        private int[] multipliedRent;
        private int houseCost;
        private ConsoleColor color;

        public Property()
        {
            multipliedRent = new int[5];
            Houses = 0;
        }

        public override bool LoadFromXml(XmlNode xmlNode)
        {
            if (!base.LoadFromXml(xmlNode))
                return false;
            try
            {
                rent = Int32.Parse(xmlNode.SelectSingleNode("rent").InnerText);
                for (int i=1;i<=5;i++)
                {
                    XmlNode node = xmlNode.SelectSingleNode("multipliedrent[" + i.ToString() + "]");
                    multipliedRent[i - 1] = Int32.Parse(node.InnerText);
                }
                houseCost = Int32.Parse(xmlNode.SelectSingleNode("housecost").InnerText);
                var colorstr = xmlNode.SelectSingleNode("color").InnerText;
                switch (colorstr)
                {
                    case "brown":
                        break;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public int HouseCost
        {
            get
            {
                return houseCost;
            }
        }
        public ConsoleColor Color
        {
            get { return color; }
        }

        public int Houses
        {
            get; set;
        }
        public int Rent
        {
            get
            {
                if (Houses == 0)
                    return rent;
                else
                    return multipliedRent[Houses - 1];
            }
        }
    }
}
