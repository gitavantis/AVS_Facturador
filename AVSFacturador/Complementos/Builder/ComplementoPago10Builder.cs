using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AVSFacturador.Complementos;

namespace AVSFacturador.Complementos.Funciones
{
    public class ComplementoPago10Builder : IComplementoBuilder
    { 
        public XmlElement xmlComplemento { get; set; }
        public AVSFacturador.Complementos.Pagos Pagos { get; set; }

    

        public ComplementoPago10Builder(XmlElement _xmlComplemento)
        {
            this.xmlComplemento = _xmlComplemento;
            Pagos = null;
        }

        public static bool isTypeOfXmlElement(string xmlElementName)
        {
            return xmlElementName == "pago10:Pagos";
        }

        public IComplemento genObjectComprobanteComplemento()
        {
            if( this.xmlComplemento != null )
            {
                var bytes = Encoding.UTF8.GetBytes( this.xmlComplemento.OuterXml );
                Pagos = new Pagos();
                Pagos = Pagos.Deserialize(bytes);
            }
            return Pagos;
        }
    }
}
