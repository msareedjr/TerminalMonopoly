using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerminalMonopoly
{
    class PaidSpace : Space
    {
        private int price;
        private Player ownedby;
        private bool mortgaged;

        public PaidSpace()
        {
            mortgaged = false;
            ownedby = Player.None;
        }

        public override bool LoadFromXml(XmlNode xmlNode)
        {
            if (!base.LoadFromXml(xmlNode))
                return false;
            try
            {
                price = Int32.Parse(xmlNode.SelectSingleNode("price").InnerText);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool Mortgaged
        {
            get; set;
        }
        public Player OwnedBy
        {
            get; set;
        }
        public int Price
        {
            get
            {
                return price;
            }
        }
    }
}
