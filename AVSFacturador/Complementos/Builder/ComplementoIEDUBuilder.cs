
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace AVSFacturador.Complementos.Funciones
{
    public class ComplementoIEDUBuilder: IComplementoBuilder
    {
        public XmlElement xmlComplemento { get; set; }

        protected instEducativas instEducativasComp { get; set; }

         

        public ComplementoIEDUBuilder(XmlElement _xmlComplemento )
        {
            this.xmlComplemento = _xmlComplemento;
            instEducativasComp = null;
        }

        public static bool isTypeOfXmlElement(string xmlElementName)
        {
            return xmlElementName == "iedu:instEducativas";
        }

      


        public IComplemento genObjectComprobanteComplemento()
        {
            if( this.xmlComplemento != null )
            {
                instEducativasComp = new instEducativas();
                instEducativasComp = instEducativasComp.Deserialize(this.xmlComplemento.OuterXml); 
            }
            return instEducativasComp;
        }


    }
}
