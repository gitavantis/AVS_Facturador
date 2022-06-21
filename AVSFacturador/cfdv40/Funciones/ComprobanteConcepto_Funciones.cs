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
using Foolproof;
using System.ComponentModel;
using AVSFacturador.Helpers;

namespace AVSFacturador.cfdv40
{
    [MetadataType(typeof(ComprobanteConceptoCuentaPredial_Validation))]
    public partial class ComprobanteConceptoCuentaPredial { }

    public class ComprobanteConceptoCuentaPredial_Validation
    {
        [Required(ErrorMessage = "ComprobanteConceptoCuentaPredial.Numero es Obligatorio")]
        [MinLength(1, ErrorMessage = "ComprobanteConceptoCuentaPredial.Numero debe tener al menos 1 digito")]
        [MaxLength(150, ErrorMessage = "ComprobanteConceptoCuentaPredial.Numero tiene un maximo de 100 digitos")]
        [RegularExpression("[0-9]{1,150}",
            ErrorMessage = "ComprobanteConceptoCuentaPredial.Numero tiene formato incorrecto. Solo se aceptan numeros.")]
        public string Numero;
    }

    [MetadataType(typeof(ComprobanteConceptoInformacionAduanera_Validation))]
    public partial class ComprobanteConceptoInformacionAduanera { }

    public class ComprobanteConceptoInformacionAduanera_Validation
    {
        [Required(ErrorMessage = "ComprobanteConceptoInformacionAduanera.NumeroPedimento es Obligatorio")]
        [MinLength(1, ErrorMessage = "ComprobanteConceptoInformacionAduanera.NumeroPedimento debe tener 21 digitos")]
        [MaxLength(21, ErrorMessage = "ComprobanteConceptoInformacionAduanera.NumeroPedimento debe tener 21 digitos")]
        [RegularExpression("[0-9]{2}  [0-9]{2}  [0-9]{4}  [0-9]{7}",
            ErrorMessage = "ComprobanteConceptoInformacionAduanera.NumeroPedimento tiene formato incorrecto")]
        public string NumeroPedimento;
    }

    [MetadataType(typeof(ComprobanteConceptoImpuestosRetencion_Validation))]
    public partial class ComprobanteConceptoImpuestosRetencion { }

    public class ComprobanteConceptoImpuestosRetencion_Validation
    {
        [Required(ErrorMessage = "ComprobanteConceptoImpuestosRetencion.Impuesto es Obligatorio")]
        [ExistsInEnum(typeof(cfdv40_full.c_Impuesto))]
        public string Impuesto;

        [Required(ErrorMessage = "ComprobanteConceptoImpuestosRetencion.TipoFactor es Obligatorio")]
        [ExistsInEnum(typeof(cfdv40_full.c_TipoFactor))]
        public string TipoFactor;

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "ComprobanteConceptoImpuestosRetencion.TasaOCuota no puede ser negativo")]
        public decimal TasaOCuota;
    }

    [MetadataType(typeof(ComprobanteConceptoImpuestosTraslado_Validation))]
    public partial class ComprobanteConceptoImpuestosTraslado
    {
        public bool TasaOCuotaRequired
        {
            get
            {
                return this.TipoFactor == "Tasa" || this.TipoFactor == "Cuota";
            }
        }
    }

    public class ComprobanteConceptoImpuestosTraslado_Validation
    {
        [Required(ErrorMessage = "ComprobanteConceptoImpuestosTraslado.Impuesto es Obligatorio")]
        [ExistsInEnum(typeof(cfdv40_full.c_Impuesto))]
        public string Impuesto;

        [Required(ErrorMessage = "ComprobanteConceptoImpuestosTraslado.TipoFactor es Obligatorio")]
        [ExistsInEnum(typeof(cfdv40_full.c_TipoFactor))]
        public string TipoFactor;

        [EqualTo("TasaOCuotaRequired", ErrorMessage = "ComprobanteConceptoImpuestosTraslado.TasaOCuota es Obligatorio cuanto TipoFactor es Tasa o Cuota")]
        public bool TasaOCuotaSpecified { get; set; }

        [EqualTo("TasaOCuotaRequired", ErrorMessage = "ComprobanteConceptoImpuestosTraslado.Importe es Obligatorio cuanto TipoFactor es Tasa o Cuota")]
        public bool ImporteSpecified { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "ComprobanteConceptoImpuestosTraslado.TasaOCuota no puede ser negativo")]
        public decimal TasaOCuota;
    }

    [MetadataType(typeof(ComprobanteConceptoImpuestos_Validation))]
    public partial class ComprobanteConceptoImpuestos { }

    public class ComprobanteConceptoImpuestos_Validation
    {
        [MinLength(1, ErrorMessage = "El nodo ComprobanteConceptoImpuestos.Traslados debe tener al menos 1 ComprobanteConceptoImpuestosTraslado")]
        public ComprobanteConceptoImpuestosTraslado[] Traslados;

        [MinLength(1, ErrorMessage = "El nodo ComprobanteConceptoImpuestos.Retencion debe tener al menos 1 ComprobanteConceptoImpuestosRetencion")]
        public ComprobanteConceptoImpuestosRetencion[] Retenciones;
    }

    [MetadataType(typeof(ComprobanteConcepto_Validation))]
    public partial class ComprobanteConcepto { }

    public class ComprobanteConcepto_Validation
    {
        [Required(ErrorMessage = "ComprobanteConcepto.ClaveProdServ es Obligatorio")]
        [MinLength(8, ErrorMessage = "ComprobanteConcepto.ClaveUnidad debe tener al menos 8 digito")]
        [MaxLength(8, ErrorMessage = "ComprobanteConcepto.ClaveProdServ tiene un maximo de 8 digitos")]
        [RegularExpression("^[0-9]{8}$",
            ErrorMessage = "ComprobanteConcepto.ClaveProdServ tiene caracteres invalidos")]
        //[ExistsInEnum(typeof(cfdv40_full.c_ClaveProdServ))]
        public string ClaveProdServ;

        [MinLength(1, ErrorMessage = "ComprobanteConcepto.NoIdentificacion debe tener al menos 1 digito")]
        [MaxLength(100, ErrorMessage = "ComprobanteConcepto.NoIdentificacion tiene un maximo de 100 digitos")]
        [RegularExpression("[^|]{1,100}",
            ErrorMessage = "ComprobanteConcepto.NoIdentificacion tiene caracteres invalidos")]
        public string NoIdentificacion;

        [Range(typeof(decimal), "0.000001", "79228162514264337593543950335", ErrorMessage = "ComprobanteConcepto.Cantidad tiene que ser minimo 0.000001")]
        public decimal Cantidad;

        [Required(ErrorMessage = "ComprobanteConcepto.ClaveUnidad es Obligatorio")]
        [MinLength(2, ErrorMessage = "ComprobanteConcepto.ClaveUnidad debe tener al menos 2 digito")]
        [MaxLength(3, ErrorMessage = "ComprobanteConcepto.ClaveUnidad tiene un maximo de 3 digitos")]
        //[ExistsInEnum(typeof(cfdv40_full.c_ClaveUnidad))]
        public string ClaveUnidad;

        [MinLength(1, ErrorMessage = "ComprobanteConcepto.Unidad debe tener al menos 1 digito")]
        [MaxLength(20, ErrorMessage = "ComprobanteConcepto.Unidad tiene un maximo de 20 digitos")]
        [RegularExpression("[^|]{1,20}",
            ErrorMessage = "ComprobanteConcepto.Unidad tiene caracteres invalidos")]
        public string Unidad;

        [Required(ErrorMessage = "ComprobanteConcepto.Descripcion es Obligatorio")]
        [MinLength(1, ErrorMessage = "ComprobanteConcepto.Descripcion debe tener al menos 1 digito")]
        [MaxLength(1000, ErrorMessage = "ComprobanteConcepto.Descripcion tiene un maximo de 1000 digitos")]
        [RegularExpression("[^|]{1,1000}",
            ErrorMessage = "ComprobanteConcepto.Descripcion tiene caracteres invalidos")]
        public string Descripcion;

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "ComprobanteConcepto.Importe no puede ser negativo")]
        public decimal Importe;

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "ComprobanteConcepto.Descuento no puede ser negativo")]
        public decimal Descuento;

        [MinLength(1, ErrorMessage = "El nodo ComprobanteConceptoInformacionAduanera.InformacionAduanera debe tener al menos 1 ComprobanteConceptoInformacionAduanera")]
        public ComprobanteConceptoInformacionAduanera[] InformacionAduanera;
    }
}
