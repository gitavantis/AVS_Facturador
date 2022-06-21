using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using AVSFacturador.Helpers;

namespace AVSFacturador.cfdv33
{
    public partial class ComprobanteEmisor
    {
    }
    public class ComprobanteEmisor_Validation
    {
        [Required(ErrorMessage = "ComprobanteEmisor.Rfc es Obligatorio")]
        [MinLength(12, ErrorMessage = "ComprobanteEmisor.Rfc debe tener al menos 12 digito")]
        [MaxLength(13, ErrorMessage = "ComprobanteEmisor.Rfc tiene un maximo de 13 digitos")]
        [RegularExpression("[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A-Z]", // Es el oficial del SAT. Hay otro que dice que [A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([A-Z\d]{3} es mejor.
            ErrorMessage = "ComprobanteEmisor.Rfc presenta un formato invalido")]
        public string Rfc;

        [MinLength(1, ErrorMessage = "ComprobanteEmisor.Nombre debe tener al menos 1 digito")]
        [MaxLength(254, ErrorMessage = "ComprobanteEmisor.Nombre tiene un maximo de 254 digitos")]
        [RegularExpression("[^|]{1,254}",
            ErrorMessage = "ComprobanteEmisor.Nombre tiene caracteres invalidos")]
        public string Nombre;

        [Required(ErrorMessage = "ComprobanteEmisor.RegimenFiscal es Obligatorio")]
        [ExistsInEnum(typeof(cfdv33_full.c_RegimenFiscal))]
        public string RegimenFiscal;
    }
}
