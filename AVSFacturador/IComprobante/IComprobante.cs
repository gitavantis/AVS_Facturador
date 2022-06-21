using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSFacturador
{
    public interface IComprobante
    {
        string DEVersionComprobante { get; }

        /// <summary>
        /// Valida que la información del comprobante sea correcta y suficinte para sellar.
        /// </summary>
        /// <returns>Regresa el Listado con los errores encontrador. Si el listado esta vacio no existen errores.</returns>
        List<string> ValidarInformacion();

        void AjustarValidacionesSAT();

        string DEEmisorRfc { get; }

        string DEEmisorRazonSocial { get; }

        string DEReceptorRfc { get; }

        string DEReceptorRazonSocial { get; }

        string DENoCertificadoEmisor { get; }

        DateTime DEFechacomprobante { get; }

        string DESerieyFolio { get; }

        string DETipoDeComprobante { get; }

        string DEMoneda { get; }

        decimal DETipoCambio { get; }

        decimal DESubtotal { get; }

        decimal DEDescuento { get; }

        decimal DETotal { get; }

        decimal DETraslados { get; }

        decimal DERetenciones { get; }

        string DEMetodoPago { get; }

        string DEFormaPago { get; }

        string DESello { get; }

        string DEUsoCFDI { get; }

        System.Xml.XmlElement[] DEComplementos { get; }
        
        System.Xml.XmlElement[] DEXMLAddendas { get; }


        Object DEAddendas { get; }

        IComprobante Deserialize(string xml);

        IComprobante Deserialize(byte[] xml);

        string Serialize();

        byte[] SerializeToArray();

        void PonerDatosCertificado(byte[] certFile);

        void PonerSelloEmisor(string sello);

        void PonerTimbreFiscal(byte[] timbre);

        void PonerComplemento(string complemento);

        void PonerAddendas(Object addendas);

        IEnumerable<string> DERelacionados { get; }

        void PonerFechaLugarExpedicion( int DiferenciaHoria );
    }
}
