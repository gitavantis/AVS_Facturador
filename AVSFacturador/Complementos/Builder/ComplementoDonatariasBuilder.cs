using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace AVSFacturador.Complementos.Funciones
{
    public class ComplementoDonatariasBuilder : IComplementoBuilder
    {
        public XmlElement xmlComplemento { get; set; }

        public AVSFacturador.Complementos.Donatarias Donatarias  { get; set; }

 
        public ComplementoDonatariasBuilder(XmlElement _xmlComplemento )
        {
            this.xmlComplemento = _xmlComplemento;
            Donatarias = null;
        }

        public static bool isTypeOfXmlElement(string xmlElementName)
        {
            return xmlElementName == "donat:Donatarias";
        }

        public IComplemento genObjectComprobanteComplemento()
        {
            if (xmlComplemento != null)
            {
                Donatarias = new AVSFacturador.Complementos.Donatarias();
                Donatarias = Donatarias.Deserialize(xmlComplemento.OuterXml);
            }
            return Donatarias;
        }
    }
}
