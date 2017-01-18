using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerminalMonopoly
{
    class Cards
    {
        private string title;
        private string action;
        private string arg1;
        private string arg2;

        public Cards()
        {
            arg1 = String.Empty;
            arg2 = String.Empty;
        }

        public bool LoadFromXml(XmlNode xmlNode)
        {
            try
            {
                title = xmlNode.SelectSingleNode("title").InnerText;
                action = xmlNode.SelectSingleNode("action").InnerText;
                if (xmlNode.SelectSingleNode("arg1")!= null)
                {
                    arg1 = xmlNode.SelectSingleNode("arg1").InnerText;
                    if(xmlNode.SelectSingleNode("arg2") != null)
                    {
                        arg2 = xmlNode.SelectSingleNode("arg2").InnerText;
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        public string Title
        {
            get
            {
                return title;
            }
        }
        public string Action
        {
            get
            {
                return action;
            }
        }
        public string Arg1
        {
            get
            {
                return arg1;
            }
        }
        public string Arg2
        {
            get
            {
                return arg2;
            }
        }
    }
}
