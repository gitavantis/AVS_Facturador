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

namespace AVSFacturador.cfdv40.Funciones
{
    [MetadataType(typeof(ComprobanteCfdiRelacionados_Validation))]
    public partial class ComprobanteCfdiRelacionados
    {
    }

    public class ComprobanteCfdiRelacionados_Validation
    {
        [Required(ErrorMessage = "ComprobanteCfdiRelacionados.TipoRelacion es Obligatorio")]
        [ExistsInEnum(typeof(cfdv40_full.c_TipoRelacion))]
        public string TipoRelacion;

        [Required(ErrorMessage = "El nodo ComprobanteCfdiRelacionados.CfdiRelacionado es Obligatorio")]
        [MinLength(1, ErrorMessage = "El nodo ComprobanteCfdiRelacionados.CfdiRelacionado debe tener al menos 1 ComprobanteCfdiRelacionadosCfdiRelacionado")]
        public ComprobanteCfdiRelacionadosCfdiRelacionado[] CfdiRelacionado;
    }

    [MetadataType(typeof(ComprobanteCfdiRelacionadosCfdiRelacionado_Validation))]
    public partial class ComprobanteCfdiRelacionadosCfdiRelacionado
    {
    }

    public class ComprobanteCfdiRelacionadosCfdiRelacionado_Validation
    {
        [Required(ErrorMessage = "ComprobanteCfdiRelacionadosCfdiRelacionado.UUID es Obligatorio")]
        [MinLength(36, ErrorMessage = "ComprobanteCfdiRelacionadosCfdiRelacionado.UUID debe tener 36 digitos")]
        [MaxLength(36, ErrorMessage = "ComprobanteCfdiRelacionadosCfdiRelacionado.UUID debe tener 36 digitos")]
        [RegularExpression("[a-f0-9A-F]{8}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{12}",
            ErrorMessage = "ComprobanteCfdiRelacionadosCfdiRelacionado.UUID tiene formato invalido")]
        public string UUID;
    }
}
