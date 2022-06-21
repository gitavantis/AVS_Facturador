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
    [MetadataType(typeof(ComprobanteImpuestosTraslado_Validation))]
    public partial class ComprobanteImpuestosTraslado { }

    public class ComprobanteImpuestosTraslado_Validation
    {
        [Required(ErrorMessage = "ComprobanteImpuestosTraslado.Impuesto es Obligatorio")]
        [ExistsInEnum(typeof(cfdv33_full.c_Impuesto))]
        public string Impuesto;

        [Required(ErrorMessage = "ComprobanteImpuestosTraslado.TipoFactor es Obligatorio")]
        [ExistsInEnum(typeof(cfdv33_full.c_TipoFactor))]
        public string TipoFactor;

        [Required(ErrorMessage = "ComprobanteImpuestosTraslado.TasaOCuota es Obligatorio")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "ComprobanteImpuestosTraslado.TasaOCuota no puede ser negativo")]
        public decimal TasaOCuota;

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "ComprobanteImpuestosTraslado.Importe no puede ser negativo")]
        public decimal Importe;
    }

    [MetadataType(typeof(ComprobanteImpuestosRetencion_Validation))]
    public partial class ComprobanteImpuestosRetencion { }

    public class ComprobanteImpuestosRetencion_Validation
    {
        [Required(ErrorMessage = "ComprobanteImpuestosRetencion.Impuesto es Obligatorio")]
        [ExistsInEnum(typeof(cfdv33_full.c_Impuesto))]
        public string Impuesto;

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "ComprobanteImpuestosRetencion.Importe no puede ser negativo")]
        public decimal Importe;
    }

    [MetadataType(typeof(ComprobanteImpuestos_Validation))]
    public partial class ComprobanteImpuestos
    {

    }

    public class ComprobanteImpuestos_Validation
    {
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "ComprobanteImpuestos.TotalImpuestosRetenidos no puede ser negativo")]
        public decimal TotalImpuestosRetenidos;

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "ComprobanteImpuestos.TotalImpuestosRetenidos no puede ser negativo")]
        public decimal TotalImpuestosTrasladados;

        [MinLength(1, ErrorMessage = "El nodo ComprobanteImpuestos.Retenciones debe tener al menos 1 ComprobanteImpuestosRetencion")]
        public ComprobanteImpuestosRetencion[] Retenciones;

        [MinLength(1, ErrorMessage = "El nodo ComprobanteImpuestos.Traslados debe tener al menos 1 ComprobanteImpuestosTraslado")]
        public ComprobanteImpuestosTraslado[] Traslados;
    }
}
