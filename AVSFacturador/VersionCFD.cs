using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace AVSFacturador
{
    public static class VersionCFD
    {
        /// <summary>
        /// Extrae la versión de un CFD contenido en un string.
        /// </summary>
        /// <param name="xmlstring">CFD en String</param>
        /// <returns></returns>
        public static string Version(string xmlstring)
        {
            var bytes = Encoding.UTF8.GetBytes(xmlstring);
            System.IO.StreamReader stringReader = new System.IO.StreamReader(new MemoryStream(bytes));
            //System.Xml.XmlTextReader xmlTextReader = new System.Xml.XmlTextReader(stringReader);

            var cfd = XElement.Load(stringReader);
            var version = cfd.Attribute("version");

            if (version != null)
            {
                return version.Value;
            }
            else
            {
                version = cfd.Attribute("Version");
                if (version != null)
                    return version.Value;

                return "El xml no tiene versión o no es XML válido.";
            }
        }

        /// <summary>
        /// Extrae la versión de un CFD contenido en un byte[].
        /// </summary>
        /// <param name="xmlarray">CFD en Byte[] UTF8</param>
        /// <returns></returns>
        public static string Version(byte[] xmlarray)
        {
            var xmlstring = System.Text.Encoding.UTF8.GetString(xmlarray);

            return Version(xmlstring);
        }
    }
}
