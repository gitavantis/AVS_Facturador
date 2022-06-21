using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AVSFacturador.Complementos 
{
    public partial class ImpuestosLocales : IComplemento
    {
        public void AjustarValidacionesSAT()
        {
            
        }

        public List<string> ValidarInformacion()
        {
            return new List<string>() { };
        }

        public XmlElement ObtenerAddendaXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement lstXmlElement;
            string strXml;
            try
            {
                strXml = this.Serialize(this);
                xmlDoc.InnerXml = strXml;

                lstXmlElement = xmlDoc.DocumentElement;
                lstXmlElement.RemoveAttribute("xmlns:xsi");
                lstXmlElement.RemoveAttribute("xmlns:xsd");
                lstXmlElement.RemoveAttribute("xmlns:implocal");

                Console.WriteLine(lstXmlElement.InnerXml);
                //lstXmlElement.RemoveAllAttributes();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
            return lstXmlElement;
        }
    }
}
