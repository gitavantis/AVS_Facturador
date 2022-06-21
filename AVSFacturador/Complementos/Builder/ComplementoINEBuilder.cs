using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AVSFacturador.Complementos;

namespace AVSFacturador.Complementos.Funciones
{
    public class ComplementoINEBuilder : IComplementoBuilder
    { 
        public XmlElement xmlComplemento { get; set; }
        public INE INE { get; set; }

    

        public ComplementoINEBuilder(XmlElement _xmlComplemento)
        {
            this.xmlComplemento = _xmlComplemento;
            INE = null;
        }

        public static bool isTypeOfXmlElement(string xmlElementName)
        {
            return xmlElementName == "ine:INE";
        }

        public IComplemento genObjectComprobanteComplemento()
        {
            if( this.xmlComplemento != null )
            {
                INE = new INE();
                INE= INE.Deserialize(this.xmlComplemento.OuterXml);
            }
            return INE;
        }
    }
}
