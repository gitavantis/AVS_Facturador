using AVSFacturador.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSFacturador.Complementos.Nomina12
{
    [MetadataType(typeof(Nomina12_Validation))]
    public partial class Nomina12 : IComplemento
    {


        private void RegistrarTipos()
        {
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Nomina12), typeof(Nomina12_Validation)), typeof(Nomina12));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaEmisor), typeof(NominaEmisor_Validation)), typeof(NominaEmisor));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaEmisorEntidadSNCF), typeof(NominaEmisorEntidadSNCF_Validation)), typeof(NominaEmisorEntidadSNCF));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaReceptor), typeof(NominaReceptor_Validation)), typeof(NominaReceptor));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaReceptorSubContratacion), typeof(NominaReceptorSubContratacion_Validation)), typeof(NominaReceptorSubContratacion));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaPercepciones), typeof(NominaPercepciones_Validation)), typeof(NominaPercepciones));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaPercepcionesPercepcion), typeof(NominaPercepcionesPercepcion_Validation)), typeof(NominaPercepcionesPercepcion));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaPercepcionesPercepcionAccionesOTitulos), typeof(NominaPercepcionesPercepcionAccionesOTitulos_Validation)), typeof(NominaPercepcionesPercepcionAccionesOTitulos));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaPercepcionesPercepcionHorasExtra), typeof(NominaPercepcionesPercepcionHorasExtra_Validation)), typeof(NominaPercepcionesPercepcionHorasExtra));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaPercepcionesJubilacionPensionRetiro), typeof(NominaPercepcionesJubilacionPensionRetiro_Validation)), typeof(NominaPercepcionesJubilacionPensionRetiro));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaPercepcionesSeparacionIndemnizacion), typeof(NominaPercepcionesSeparacionIndemnizacion_Validation)), typeof(NominaPercepcionesSeparacionIndemnizacion));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaDeducciones), typeof(NominaDeducciones_Validation)), typeof(NominaDeducciones));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaDeduccionesDeduccion), typeof(NominaDeduccionesDeduccion_Validation)), typeof(NominaDeduccionesDeduccion));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaOtroPago), typeof(NominaOtroPago_Validation)), typeof(NominaOtroPago));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaOtroPagoSubsidioAlEmpleo), typeof(NominaOtroPagoSubsidioAlEmpleo_Validation)), typeof(NominaOtroPagoSubsidioAlEmpleo));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaOtroPagoCompensacionSaldosAFavor), typeof(NominaOtroPagoCompensacionSaldosAFavor_Validation)), typeof(NominaOtroPagoCompensacionSaldosAFavor));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(NominaIncapacidad), typeof(NominaIncapacidad_Validation)), typeof(NominaIncapacidad));
        }


        public List<string> ValidarInformacion()
        {
            var respuesta = new List<string>();

            RegistrarTipos();

            respuesta.AddRange(ObjectValidation.ValidateObject(this));

            if (FechaInicialPago > FechaFinalPago)
            {
                respuesta.Add("Nomina.FechaInicialPago debe ser menor al campo FechaFinalPago.");
            }

            if (this.Percepciones == null)
            {
                if (this.TotalPercepcionesSpecified)
                {
                    respuesta.Add("Nomina.TotalPercepciones no debe existir este elemento.");
                }
            }
            else
            {
                var totalesPercepciones = this.Percepciones.TotalSueldos + this.Percepciones.TotalSeparacionIndemnizacion + this.Percepciones.TotalJubilacionPensionRetiro;
                var totalGrabado = this.Percepciones.TotalGravado + this.Percepciones.TotalExento;
                if (totalesPercepciones != totalGrabado)
                {
                    respuesta.Add("La suma de los valores de los atributos TotalSueldos más " +
                        "TotalSeparacionIndemnizacion más TotalJubilacionPensionRetiro debe ser igual " +
                        "a la suma de los valores de los atributos TotalGravado más TotalExento.");
                }

                if (this.Percepciones.Percepcion != null)
                {
                    var sumaSueldos = this.Percepciones.Percepcion
                        .Where(p =>
                           p.TipoPercepcion != c_TipoPercepcion.Item022 &&
                           p.TipoPercepcion != c_TipoPercepcion.Item023 &&
                           p.TipoPercepcion != c_TipoPercepcion.Item025 &&
                           p.TipoPercepcion != c_TipoPercepcion.Item039 &&
                           p.TipoPercepcion != c_TipoPercepcion.Item044)
                        .Sum(p => p.ImporteGravado + p.ImporteExento);

                    if (this.Percepciones.TotalSueldos != sumaSueldos)
                    {
                        respuesta.Add("NOM190-El valor del atributo Nomina.Percepciones.TotalSueldos (" + this.Percepciones.TotalSueldos.ToString() + ")  " + 
                            "debe ser igual a la suma de los atributos ImporteGravado " +
                            "e ImporteExento (" + sumaSueldos.ToString() + ") donde la clave expresada en el atributo TipoPercepcion sea " +
                            "distinta de 022 Prima por Antigüedad, 023 Pagos por separación, 025 " +
                            "Indemnizaciones, 039 Jubilaciones, pensiones o haberes de retiro en una " +
                            "exhibición y 044 Jubilaciones, pensiones o haberes de retiro en parcialidades.");
                    }
                }
            }

            if (this.Deducciones == null && this.TotalDeduccionesSpecified)
            {
                respuesta.Add("Nomina.TotalDeducciones no debe existir este elemento.");
            }

            if (this.OtrosPagos == null && this.TotalOtrosPagosSpecified)
            {
                respuesta.Add("Nomina.OtrosPagos no debe existir este elemento.");
            }

            var tipoContrato = this.Receptor.TipoContrato.ToString();
            if (
                (tipoContrato == "Item01" ||
                tipoContrato == "Item02" ||
                tipoContrato == "Item03" ||
                tipoContrato == "Item04" ||
                tipoContrato == "Item05" ||
                tipoContrato == "Item06" ||
                tipoContrato == "Item07" ||
                tipoContrato == "Item08")
                )
            {
                if (string.IsNullOrEmpty(this.Emisor.RegistroPatronal))
                {
                    respuesta.Add("Nomina.Emisor.RegistroPatronal es requerido para el TipoContrato de Receptor.");
                }

                var tipoRegimen = this.Receptor.TipoRegimen.ToString();
                if (tipoRegimen != "Item02" &&
                    tipoRegimen != "Item03" &&
                    tipoRegimen != "Item04")
                {
                    respuesta.Add("Nomina.Receptor.TipoRegimen tiene un valor no válido para el TipoContrato de Receptor.");
                }


                if (!string.IsNullOrEmpty(this.Emisor.RegistroPatronal))
                {
                    if (string.IsNullOrEmpty(this.Receptor.NumSeguridadSocial))
                    {
                        respuesta.Add("Receptor.NumSeguridadSocial es requerido.");
                    }

                    if (!this.Receptor.FechaInicioRelLaboralSpecified)
                    {
                        respuesta.Add("Receptor.FechaInicioRelLaboral es requerido.");
                    }

                    if ( string.IsNullOrEmpty(this.Receptor.Antigüedad))
                    {
                        respuesta.Add("Receptor.Antigüedad es requerido.");
                    }

                    if ( !this.Receptor.RiesgoPuestoSpecified )
                    {
                        respuesta.Add("Receptor.RiesgoPuesto es requerido.");
                    }
                    if (!this.Receptor.SalarioDiarioIntegradoSpecified)
                    {
                        respuesta.Add("Receptor.SalarioDiarioIntegrado es requerido.");
                    }

                }
            }

            if( this.Emisor.EntidadSNCF != null)
            {
                var OrigenRecurso = this.Emisor.EntidadSNCF.OrigenRecurso.ToString();
                if (OrigenRecurso == "IM")
                {
                    if( !this.Emisor.EntidadSNCF.MontoRecursoPropioSpecified )
                    {
                        respuesta.Add("Emisor.EntidadSNCF.MontoRecursoPropio es requerido.");
                    }
                    else
                    {
                        var sumaTotales = this.TotalPercepciones + this.TotalOtrosPagos;
                        if ( this.Emisor.EntidadSNCF.MontoRecursoPropio > sumaTotales )
                        {
                            respuesta.Add("Emisor.EntidadSNCF.MontoRecursoPropio debe ser menor que la suma TotalPercepciones y TotalOtrosPagos.");
                        }
                    }
                }
            }

            if( OtrosPagos != null )
            {
                foreach( var otroPago in OtrosPagos )
                {
                    if(otroPago.TipoOtroPago == c_TipoOtroPago.Item004)
                    {
                        if( otroPago.CompensacionSaldosAFavor  == null )
                        {
                            respuesta.Add("El elemento OtrosPagos.CompensacionSaldosAFavor es requerido cuando TipoOtroPago es 004.");
                        }
                    }

                    if (otroPago.TipoOtroPago == c_TipoOtroPago.Item002)
                    {
                        if (otroPago.SubsidioAlEmpleo  == null)
                        {
                            respuesta.Add("El elemento OtrosPagos.SubsidioAlEmpleo es requerido cuando TipoOtroPago es 002.");
                        }
                    }
                }
            }

            return respuesta;
        }

        public void AjustarValidacionesSAT()
        {
            if (this.Percepciones != null)
            {
                TotalPercepciones = this.Percepciones.TotalSueldos + this.Percepciones.TotalSeparacionIndemnizacion + this.Percepciones.TotalJubilacionPensionRetiro;

                if (this.Percepciones.Percepcion != null)
                {
                    var sumaSueldos = this.Percepciones.Percepcion
                        .Where(p =>
                           p.TipoPercepcion != c_TipoPercepcion.Item022 &&
                           p.TipoPercepcion != c_TipoPercepcion.Item023 &&
                           p.TipoPercepcion != c_TipoPercepcion.Item025 &&
                           p.TipoPercepcion != c_TipoPercepcion.Item039 &&
                           p.TipoPercepcion != c_TipoPercepcion.Item044)
                        .Sum(p => p.ImporteGravado + p.ImporteExento);

                    this.Percepciones.TotalSueldos = sumaSueldos;
                    

                    var sumaTotalSeparacionIndemnizacion = this.Percepciones.Percepcion
                       .Where(p =>
                          p.TipoPercepcion == c_TipoPercepcion.Item022 ||
                          p.TipoPercepcion == c_TipoPercepcion.Item023 ||
                          p.TipoPercepcion == c_TipoPercepcion.Item025 )
                       .Sum(p => p.ImporteGravado + p.ImporteExento);

                    this.Percepciones.TotalSeparacionIndemnizacion = sumaTotalSeparacionIndemnizacion;


                    var sumaTotalJubilacionPensionRetiro  = this.Percepciones.Percepcion
                       .Where(p =>
                          p.TipoPercepcion == c_TipoPercepcion.Item039 ||
                          p.TipoPercepcion == c_TipoPercepcion.Item044 )
                       .Sum(p => p.ImporteGravado + p.ImporteExento);

                    this.Percepciones.TotalJubilacionPensionRetiro = sumaTotalJubilacionPensionRetiro;
                }

                if( this.Percepciones.JubilacionPensionRetiro != null )
                {
                    if( this.Percepciones.JubilacionPensionRetiro.TotalUnaExhibicionSpecified )
                    {
                        this.Percepciones.JubilacionPensionRetiro.MontoDiarioSpecified = false;
                        this.Percepciones.JubilacionPensionRetiro.TotalParcialidadSpecified = false;
                    }

                    if (this.Percepciones.JubilacionPensionRetiro.TotalParcialidadSpecified )
                    {
                        this.Percepciones.JubilacionPensionRetiro.MontoDiarioSpecified = true;
                        this.Percepciones.JubilacionPensionRetiro.TotalParcialidadSpecified = true;
                    }

                }
            }


            if (this.Deducciones != null)
            {
                TotalDeducciones = this.Deducciones.TotalOtrasDeducciones + this.Deducciones.TotalImpuestosRetenidos;
           
                if (this.Deducciones.Deduccion != null)
                {
                    var TotalImpuestosRetenidos = this.Deducciones.Deduccion
                        .Where(d => d.TipoDeduccion == c_TipoDeduccion.Item002)
                        .Sum(d => d.Importe);

                    if (TotalImpuestosRetenidos == 0)
                    {
                        this.Deducciones.TotalImpuestosRetenidosSpecified = false;
                    }
                    else
                    {
                        this.Deducciones.TotalImpuestosRetenidos = TotalImpuestosRetenidos;
                    }
                }
            }

            if (this.OtrosPagos != null)
            {
                TotalOtrosPagos = this.OtrosPagos.Sum(o => o.Importe);
            }

            

        }
    }



    [MetadataType(typeof(NominaEmisor_Validation))]
    public partial class NominaEmisor
    {

    }

    [MetadataType(typeof(NominaEmisorEntidadSNCF_Validation))]
    public partial class NominaEmisorEntidadSNCF
    {

    }


    [MetadataType(typeof(NominaReceptor_Validation))]
    public partial class NominaReceptor
    {

    }


    [MetadataType(typeof(NominaReceptorSubContratacion_Validation))]
    public partial class NominaReceptorSubContratacion
    {

    }


    [MetadataType(typeof(NominaPercepciones_Validation))]
    public partial class NominaPercepciones
    {

    }


    [MetadataType(typeof(NominaPercepcionesPercepcion_Validation))]
    public partial class NominaPercepcionesPercepcion
    {

    }

    [MetadataType(typeof(NominaPercepcionesPercepcionAccionesOTitulos_Validation))]

    public partial class NominaPercepcionesPercepcionAccionesOTitulos
    {

    }


    [MetadataType(typeof(NominaPercepcionesPercepcionHorasExtra_Validation))]
    public partial class NominaPercepcionesPercepcionHorasExtra
    {


    }

    [MetadataType(typeof(NominaPercepcionesJubilacionPensionRetiro_Validation))]
    public partial class NominaPercepcionesJubilacionPensionRetiro
    {

    }

    [MetadataType(typeof(NominaPercepcionesSeparacionIndemnizacion_Validation))]
    public partial class NominaPercepcionesSeparacionIndemnizacion
    {

    }


    [MetadataType(typeof(NominaDeducciones_Validation))]
    public partial class NominaDeducciones
    {

    }

    [MetadataType(typeof(NominaDeduccionesDeduccion_Validation))]
    public partial class NominaDeduccionesDeduccion
    {

    }

    [MetadataType(typeof(NominaOtroPago_Validation))]
    public partial class NominaOtroPago
    {

    }


    [MetadataType(typeof(NominaOtroPagoSubsidioAlEmpleo_Validation))]
    public partial class NominaOtroPagoSubsidioAlEmpleo
    {

    }


    [MetadataType(typeof(NominaOtroPagoCompensacionSaldosAFavor_Validation))]
    public partial class NominaOtroPagoCompensacionSaldosAFavor
    {

    }

    [MetadataType(typeof(NominaIncapacidad_Validation))]
    public partial class NominaIncapacidad
    {

    }

    public partial class Nomina12_Validation
    {

        [Required(ErrorMessage = "Version es requerido.")]
        public string Version { get; set; }



        [Required(ErrorMessage = "TipoNomina es requerido.")]
        public string TipoNomina { get; set; }

        [Required(ErrorMessage = "FechaPago es requerido.")]
        public System.DateTime FechaPago { get; set; }


        [Required(ErrorMessage = "FechaInicialPago es requerido.")]
        public System.DateTime FechaInicialPago { get; set; }


        [Required(ErrorMessage = "FechaFinalPago es requerido.")]
        public System.DateTime FechaFinalPago { get; set; }


        [Required(ErrorMessage = "NumDiasPagados es requerido.")]
        [RegularExpression("(([1-9][0-9]{0,4})|[0])(.[0-9]{3})?",
            ErrorMessage = "Número de Días Pagados no tiene un Formato Válido")
        ]
        public decimal NumDiasPagados { get; set; }

        //CONDICIONAL
        public decimal TotalPercepciones { get; set; }

        //CONDICIONAL
        public decimal TotalDeducciones { get; set; }


        //CONDICIONAL
        public decimal TotalOtrosPagos { get; set; }

    }



    public class NominaEmisor_Validation
    {
        //condicional
        [RegularExpression("([A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9])|[^ ]",
            ErrorMessage = "NominaEmisor.Curp del Emisor no tiene un Formato Válido")
        ]
        public string Curp { get; set; }


        //CONDICIONAL
        //[RegularExpression("([A-Z]|[a-z]|[0-9]|Ñ|ñ|!|&quot;|%|&amp;|&apos;|´|-|:|;|&gt;|=|&lt;|@|_|,|\\{|\\}|`|~|á|é|í|ó|ú|Á|É|Í|Ó|Ú|ü|Ü){1,20}",
        [RegularExpression("[^|]{1,20}",
            ErrorMessage = "NominaEmisor.RegistroPatronal no tiene un Formato Válido")
        ]
        public string RegistroPatronal { get; set; }


       
        [RegularExpression("[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?",
            ErrorMessage = "NominaEmisor.RfcPatronOrigen no tiene un Formato Válido")
        ]
        public string RfcPatronOrigen { get; set; }
    }




    public class NominaEmisorEntidadSNCF_Validation
    {
        [Required(ErrorMessage = "NominaEmisorEntidadSNCF.OrigenRecurso es requerido.")]
        public string OrigenRecurso { get; set; }

        //OPCIONAL
        public decimal MontoRecursoPropio { get; set; }
    }


    public class NominaReceptor_Validation
    {

        [RegularExpression("[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]",
            ErrorMessage = "NominaReceptor.Curp no tiene un Formato Válido")
        ]
        [Required(ErrorMessage = "NominaReceptor.Curp es requerido.")]
        public string Curp { get; set; }


        //CONDICIONAL
        [RegularExpression("[0-9]{1,15}",
            ErrorMessage = "NominaReceptor.NumSeguridadSocial no tiene un Formato Válido")
        ]
        [MinLength(1, ErrorMessage = "NominaReceptor.NumSeguridadSocial debe tener al menos 1 digito")]
        [MaxLength(1000, ErrorMessage = "NominaReceptor.NumSeguridadSocial tiene un maximo de 1000 digitos")]
        public string NumSeguridadSocial { get; set; }

        
        public System.DateTime FechaInicioRelLaboral { get; set; }


        [RegularExpression(
            "P(([1-9][0-9]{0,3})|0)W|P([1-9][0-9]?Y)?(([1-9]|1[012])M)?(0|[1-9]|[12][0-9]|3[01])D",
            ErrorMessage = "NominaReceptor.Antigüedad no tiene un Formato Válido")
        ]
        public string Antigüedad
        {
            get;
            set;
        }
        
        [Required(ErrorMessage = "NominaReceptor.TipoContrato es requerido.")]
        public string TipoContrato { get; set; }


        //OPCIONAL
        public string Sindicalizado { get; set; }


        public string TipoJornada { get; set; }


        [Required(ErrorMessage = "NominaReceptor.TipoRegimen es requerido.")]
        public string TipoRegimen { get; set; }


        [Required(ErrorMessage = "NominaReceptor.NumEmpleado es requerido.")]
        [RegularExpression(
            "[^|]{1,15}",
            ErrorMessage = "NominaReceptor.NumEmpleado no tiene un Formato Válido")
        ]
        public string NumEmpleado { get; set; }

        [RegularExpression(
            "[^|]{1,100}",
            ErrorMessage = "NominaReceptor.Departamento no tiene un Formato Válido")
        ]
        public string Departamento { get; set; }


        [RegularExpression(
            "[^|]{1,100}",
            ErrorMessage = "NominaReceptor.Puesto no tiene un Formato Válido")
        ]
        public string Puesto { get; set; }

        public string RiesgoPuesto { get; set; }


        [Required(ErrorMessage = "NominaReceptor.PeriodicidadPago es requerido.")]
        public string PeriodicidadPago { get; set; }

        public string Banco { get; set; }


        public string CuentaBancaria { get; set; }


        public decimal SalarioBaseCotApor { get; set; }


        public decimal SalarioDiarioIntegrado { get; set; }
        
        [Required(ErrorMessage = "NominaReceptor.ClaveEntFed es requerido.")]
        public string ClaveEntFed { get; set; }
    }




    public class NominaReceptorSubContratacion_Validation
    {
        [Required(ErrorMessage = "NominaReceptorSubContratacion.RfcLabora es requerido.")]
        [RegularExpression("[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?",
            ErrorMessage = "NominaReceptorSubContratacion.RfcLabora no tiene un Formato Válido")
        ]
        public string RfcLabora { get; set; }

        [Required(ErrorMessage = "NominaReceptorSubContratacion.PorcentajeTiempo es requerido.")]
        [Range(typeof(decimal), "0.001", "100.000", ErrorMessage = "NominaReceptorSubContratacion.PorcentajeTiempo fuera de rango (0.001-100.000)")]
        public decimal PorcentajeTiempo { get; set; }
    }


    public partial class NominaPercepciones_Validation
    {
        
        public NominaPercepcionesPercepcion[] Percepcion { get; set; }


        public NominaPercepcionesJubilacionPensionRetiro JubilacionPensionRetiro { get; set; }


        public NominaPercepcionesSeparacionIndemnizacion SeparacionIndemnizacion { get; set; }


        public decimal TotalSueldos { get; set; }


        public decimal TotalSeparacionIndemnizacion { get; set; }


        public decimal TotalJubilacionPensionRetiro { get; set; }

        
        [Required(ErrorMessage = "NominaPercepciones.TotalGravado es Requerido.")]
        public decimal TotalGravado { get; set; }


        [Required(ErrorMessage = "NominaPercepciones.TotalExento es Requerido")]
        public decimal TotalExento { get; set; }

    }

    public partial class NominaPercepcionesPercepcion_Validation
    {
        public NominaPercepcionesPercepcionAccionesOTitulos AccionesOTitulos { get; set; }

        public NominaPercepcionesPercepcionHorasExtra[] HorasExtra { get; set; }


        [Required(ErrorMessage = "NominaPercepcionesPercepcion.TipoPercepcion es Requerido.")]
        public string TipoPercepcion { get; set; }

        [Required(ErrorMessage = "NominaPercepcionesPercepcion.Clave es Requerido.")]
        [RegularExpression("[^|]{3,15}",
            ErrorMessage = "NominaPercepcionesPercepcion.Clave no tiene un Formato Válido")
        ]
        public string Clave { get; set; }


        [Required(ErrorMessage = "NominaPercepcionesPercepcion.Concepto es Requerido")]
        
        [RegularExpression("[^|]{1,100}",
            ErrorMessage = "NominaPercepcionesPercepcion.Clave no tiene un Formato Válido")
        ]
        public string Concepto { get; set; }

        [Required(ErrorMessage = "NominaPercepcionesPercepcion.ImporteGravado es Requerido.")]
        public decimal ImporteGravado { get; set; }


        [Required(ErrorMessage = "NominaPercepcionesPercepcion.ImporteExento es Requerido.")]
        public decimal ImporteExento { get; set; }
    }

    public partial class NominaPercepcionesPercepcionAccionesOTitulos_Validation
    {
        [Required(ErrorMessage = "NominaPercepcionesPercepcionAccionesOTitulos.ValorMercado es Requerido")]
        [Range(typeof(decimal), "0.000001", "79228162514264337593543950335", ErrorMessage = "NominaPercepcionesPercepcionAccionesOTitulos.ValorMercado no puede ser negativo")]
        public decimal ValorMercado { get; set; }


        [Required(ErrorMessage = "NominaPercepcionesPercepcionAccionesOTitulos.PrecioAlOtorgarse  es Requerido")]
        [Range(typeof(decimal), "0.000001", "79228162514264337593543950335", ErrorMessage = "NominaPercepcionesPercepcionAccionesOTitulos.PrecioAlOtorgarse no puede ser negativo")]
        public decimal PrecioAlOtorgarse { get; set; }
    }




    public partial class NominaPercepcionesPercepcionHorasExtra_Validation
    {
        
        [Required(ErrorMessage = "NominaPercepcionesPercepcionHorasExtra.Dias es requerido.")]
        public int Dias { get; set; }


        [Required(ErrorMessage = "NominaPercepcionesPercepcionHorasExtra.TipoHoras es requerido.")]
        public string TipoHoras { get; set; }

        [Required(ErrorMessage = "NominaPercepcionesPercepcionHorasExtra.HorasExtra es requerido.")]
        public int HorasExtra { get; set; }

        [Required(ErrorMessage = "NominaPercepcionesPercepcionHorasExtra.ImportePagado es requerido.")]
        public decimal ImportePagado { get; set; }


    }

    public partial class NominaPercepcionesJubilacionPensionRetiro_Validation
    {


        public decimal TotalUnaExhibicion { get; set; }


        public decimal TotalParcialidad { get; set; }

        public decimal MontoDiario { get; set; }


        [Required(ErrorMessage = "NominaPercepcionesJubilacionPensionRetiro.IngresoAcumulable es requerido.")]
        public decimal IngresoAcumulable { get; set; }

        [Required(ErrorMessage = "NominaPercepcionesJubilacionPensionRetiro.IngresoNoAcumulable es requerido.")]
        public decimal IngresoNoAcumulable { get; set; }
    }


    public partial class NominaPercepcionesSeparacionIndemnizacion_Validation
    {
        
        [Required(ErrorMessage = "NominaPercepcionesSeparacionIndemnizacion.TotalPagado es requerido.")]
        public decimal TotalPagado { get; set; }


        [Required(ErrorMessage = "NominaPercepcionesSeparacionIndemnizacion.NumAñosServicio es requerido.")]
        [Range(typeof(decimal), "0", "99", ErrorMessage = "NominaPercepcionesSeparacionIndemnizacion.NumAñosServicio fuera de rango (0-99)")]
        public int NumAñosServicio { get; set; }

        [Required(ErrorMessage = "NominaPercepcionesSeparacionIndemnizacion.UltimoSueldoMensOrd es requerido.")]
        public decimal UltimoSueldoMensOrd { get; set; }
         
        [Required(ErrorMessage = "NominaPercepcionesSeparacionIndemnizacion.IngresoAcumulable es requerido.")]
        public decimal IngresoAcumulable { get; set; }


        [Required(ErrorMessage = "NominaPercepcionesSeparacionIndemnizacion.IngresoNoAcumulable es requerido.")]
        public decimal IngresoNoAcumulable { get; set; }
    }


    public partial class NominaDeducciones_Validation
    {
        public NominaDeduccionesDeduccion[] Deduccion { get; set; }


        public decimal TotalOtrasDeducciones { get; set; }


        public decimal TotalImpuestosRetenidos { get; set; }
    }


    public partial class NominaDeduccionesDeduccion_Validation
    {
        [Required(ErrorMessage = "NominaDeduccionesDeduccion.TipoDeduccion es requerido.")]
        public string TipoDeduccion { get; set; }


        [Required(ErrorMessage = "NominaDeduccionesDeduccion.Clave es requerido.")]
        [RegularExpression("[^|]{3,15}",
            ErrorMessage = "NominaDeduccionesDeduccion.Clave no tiene un Formato Válido")
        ]
        public string Clave { get; set; }


        [Required(ErrorMessage = "NominaDeduccionesDeduccion.Concepto es Requerido")]
        [RegularExpression("[^|]{1,100}",
            ErrorMessage = "NominaDeduccionesDeduccion.Concepto no tiene un Formato Válido")
        ]
        public string Concepto { get; set; }


        [Required(ErrorMessage = "NominaDeduccionesDeduccion.Importe es Requerido")]
        public decimal Importe { get; set; }
    }




    public partial class NominaOtroPago_Validation
    {


        public NominaOtroPagoSubsidioAlEmpleo SubsidioAlEmpleo { get; set; }

        public NominaOtroPagoCompensacionSaldosAFavor CompensacionSaldosAFavor { get; set; }


        [Required(ErrorMessage = "NominaOtroPago.TipoOtroPago es Requerido.")]
        public string TipoOtroPago { get; set; }


        [Required(ErrorMessage = "NominaOtroPago.Clave es Requerido")]
        [RegularExpression("[^|]{3,15}",
            ErrorMessage = "NominaOtroPago.Clave no tiene un Formato Válido.")
        ]
        public string Clave { get; set; }


        [Required(ErrorMessage = "NominaOtroPago.Concepto es Requerido")]
        [RegularExpression("[^|]{1,100}",
            ErrorMessage = "NominaOtroPago.Concepto no tiene un Formato Válido.")
        ]
        public string Concepto { get; set; }



        [Required(ErrorMessage = "NominaOtroPago.Importe es requerido")]

        public decimal Importe { get; set; }


    }


    public partial class NominaOtroPagoSubsidioAlEmpleo_Validation
    {
        [Required(ErrorMessage = "NominaOtroPagoSubsidioAlEmpleo.SubsidioCausado es Requerido")]
        public decimal SubsidioCausado { get; set; }
    }


    public partial class NominaOtroPagoCompensacionSaldosAFavor_Validation
    {
        [Required(ErrorMessage = "NominaOtroPagoCompensacionSaldosAFavor.SaldoAFavor es Requerido")]
        public decimal SaldoAFavor { get; set; }

        
        [Required(ErrorMessage = "NominaOtroPagoCompensacionSaldosAFavor.Años es Requerido")]
        public short Año { get; set; }

        
        [Required(ErrorMessage = "NominaOtroPagoCompensacionSaldosAFavor.RemanenteSalFav es Requerido")]
        public decimal RemanenteSalFav { get; set; }
    }


    public partial class NominaIncapacidad_Validation
    {
        [Required(ErrorMessage = "NominaIncapacidad.DiasIncapacidad es Requerido")]
        public int DiasIncapacidad { get; set; }

        [Required(ErrorMessage = "NominaIncapacidad.TipoIncapacidad es Requerido")]
        public string TipoIncapacidad { get; set; }
        
        public decimal ImporteMonetario { get; set; }
    }



}
