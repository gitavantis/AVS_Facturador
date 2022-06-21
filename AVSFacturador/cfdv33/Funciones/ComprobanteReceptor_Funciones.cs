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
    [MetadataType(typeof(ComprobanteReceptor_Validation))]
    public partial class ComprobanteReceptor
    {
    }

    public class ComprobanteReceptor_Validation
    {
        [Required(ErrorMessage = "ComprobanteReceptor.Rfc es Obligatorio")]
        [MinLength(12, ErrorMessage = "ComprobanteReceptor.Rfc debe tener al menos 12 digito")]
        [MaxLength(13, ErrorMessage = "ComprobanteReceptor.Rfc tiene un maximo de 13 digitos")]
        [RegularExpression("[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A-Z]", // Es el oficial del SAT. Hay otro que dice que [A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([A-Z\d]{3} es mejor.
            ErrorMessage = "ComprobanteReceptor.Rfc presenta un formato invalido")]
        public string Rfc;

        [MinLength(1, ErrorMessage = "ComprobanteReceptor.Nombre debe tener al menos 1 digito")]
        [MaxLength(254, ErrorMessage = "ComprobanteReceptor.Nombre tiene un maximo de 254 digitos")]
        [RegularExpression("[^|]{1,254}",
            ErrorMessage = "ComprobanteReceptor.Nombre tiene caracteres invalidos")]
        public string Nombre;

        [MinLength(1, ErrorMessage = "ComprobanteReceptor.NumRegIdTrib debe tener al menos 1 digito")]
        [MaxLength(40, ErrorMessage = "ComprobanteReceptor.NumRegIdTrib tiene un maximo de 40 digitos")]
        public string NumRegIdTrib;

        [Required(ErrorMessage = "ComprobanteReceptor.UsoCFDI es Obligatorio")]
        [ExistsInEnum(typeof(cfdv33_full.c_UsoCFDI))]
        public string UsoCFDI;

        //[ExistsInEnum(typeof(cfdv33_full.c_Pais))]
        [MinLength(3, ErrorMessage = "ComprobanteReceptor.ResidenciaFiscal debe tener al menos 3 digitos")]
        [MaxLength(3, ErrorMessage = "ComprobanteReceptor.ResidenciaFiscal tiene un maximo de 3 digitos")]
        public string ResidenciaFiscal;
    }
}
