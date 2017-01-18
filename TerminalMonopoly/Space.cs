using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerminalMonopoly
{
    class Space
    {
        private string name;
        private string id;
        private const string type = "Special";
        private string action;

        public Space()
        {

        }

        public virtual bool LoadFromXml(XmlNode xmlNode)
        {
            try
            {
                name = xmlNode.SelectSingleNode("name").InnerText;
                id = xmlNode.SelectSingleNode("id").InnerText;
                action = xmlNode.SelectSingleNode("action").InnerText;
            }
            catch { return false; }
            return true;
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string ID
        {
            get
            {
                return id;
            }
        }
        public string Type
        {
            get { return type; }
        }
        public string Action
        {
            get
            {
                return action;
            }
        }
    }
}
