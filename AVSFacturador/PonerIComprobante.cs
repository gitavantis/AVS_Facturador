using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVSFacturador
{
    /// <summary>
    /// Clase para crear un objeto IComprobante desde un string y byte[] sin importar la versión.
    /// </summary>
    public class CrearIComprobante
    {
        /// <summary>
        /// Regresa un objeto IComprobante en base a un byte[] sin importar la versión (3.2 o 3.3).
        /// </summary>
        /// <param name="xmlbytearray">CFD en byte[]</param>
        /// <returns></returns>
        public IComprobante CrearComprobanteFactory(byte[] xmlbytearray)
        {
            var xmlstring = Encoding.UTF8.GetString(xmlbytearray);
            return PonerIComprobante(xmlstring);
        }

        /// <summary>
        /// Regresa un objeto IComprobante en base a un string sin importar la versión (3.2 o 3.3).
        /// </summary>
        /// <param name="xmlstring">CFD en string</param>
        /// <returns></returns>
        public IComprobante CrearComprobanteFactory(string xmlstring)
        {
            return PonerIComprobante(xmlstring);
        }

        private IComprobante PonerIComprobante(string xmlstring)
        {
            var version = AVSFacturador.VersionCFD.Version(xmlstring);

            switch (version)
            {
                //case "3.2":
                //    var cfd32 = new AVSFacturador.cfdv32.Comprobante();
                //    cfd32 = (AVSFacturador.cfdv32.Comprobante)cfd32.Deserialize(xmlstring);
                //    return cfd32;

                case "3.3":
                    var cfd33 = new AVSFacturador.cfdv33.Comprobante();
                    cfd33 = (AVSFacturador.cfdv33.Comprobante)cfd33.Deserialize(xmlstring);
                    return cfd33;

                case "4.0":
                    var cfd40 = new AVSFacturador.cfdv40.Comprobante();
                    cfd40 = (AVSFacturador.cfdv40.Comprobante)cfd40.Deserialize(xmlstring);
                    return cfd40;

                default:
                    throw new Exception("La versión del CFD no es valida.");
            }
        }

    }
}
