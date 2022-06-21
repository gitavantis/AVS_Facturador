using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AVSFacturador.Complementos.Funciones
{
    public interface IComplementoBuilder
    {
        XmlElement xmlComplemento { get; set; }



        /// <summary>
        /// Metodo para construir el objeto DTO generado del xsd del complemento
        /// </summary>
        /// <returns></returns>
        IComplemento genObjectComprobanteComplemento();

        //bool isTypeOfXmlElement();
        //bool isTypeAddendaComplemento(IAddendaComplemento complemento);
    }
}
