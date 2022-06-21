using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVSFacturador.Complementos
{
    public class ExtraerComplementos
    {
        public string XmlDoc { get; set; }

        public ExtraerComplementos(string xmlcomprobante) 
        {
            XmlDoc = xmlcomprobante;
        }

        public ExtraerComplementos() 
        {
        }

        public instEducativas Extraeriedu(string ieduxml)
        {
            var iedu = new instEducativas();
            iedu.Deserialize(ieduxml);
            return iedu;
        }
    }
}
