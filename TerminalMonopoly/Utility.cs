using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerminalMonopoly
{
    class Utility:Space
    {
        private int price;
        private Player ownedby;
        private bool mortgaged;

        public Utility()
        {
        }

        public override bool LoadFromXml(XmlNode xmlNode)
        {

            return false;
        }
        
    }
}
