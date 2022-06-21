using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using AVSFacturador.Complementos;

namespace AVSFacturador.Complementos.Funciones
{
    public class ComplementoNotariosPublicosBuilder : IComplementoBuilder
    {
        public XmlElement xmlComplemento { get; set; }
        protected AVSFacturador.Complementos.NotariosPublicos NotariosPublicos { get; set; }


        public ComplementoNotariosPublicosBuilder(XmlElement _xmlComplemento)
        {
            this.xmlComplemento = _xmlComplemento;
            NotariosPublicos = null;
        }

        public static bool isTypeOfXmlElement(string xmlElementName)
        {
            return xmlElementName == "notariospublicos:NotariosPublicos";
        }

          
        public IComplemento genObjectComprobanteComplemento()
        {
            if (xmlComplemento != null)
            {
                NotariosPublicos = new AVSFacturador.Complementos.NotariosPublicos();
                NotariosPublicos = NotariosPublicos.Deserialize(xmlComplemento.OuterXml);
            }

            return NotariosPublicos;
        }
    }
}
