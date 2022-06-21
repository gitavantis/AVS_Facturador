using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace AVSFacturador.Complementos.Funciones
{
    public class ComplementoNomina12Builder : IComplementoBuilder
    {
        public XmlElement xmlComplemento { get; set; }
        protected AVSFacturador.Complementos.Nomina12.Nomina12 complementoNomina { get;set;}
         
        public ComplementoNomina12Builder(XmlElement _xmlComplemento)
        {
            this.xmlComplemento = _xmlComplemento;
            complementoNomina = null;
        }

        public static bool isTypeOfXmlElement(string xmlElementName )
        {
            return  xmlElementName == "nomina12:Nomina";
        }
        public IComplemento genObjectComprobanteComplemento()
        {

            if (xmlComplemento != null)
            {
                complementoNomina = new AVSFacturador.Complementos.Nomina12.Nomina12();
                complementoNomina = complementoNomina.Deserialize(xmlComplemento.OuterXml);
            }
            return complementoNomina;
        }


    }
}
