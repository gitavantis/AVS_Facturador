using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml.Xsl;

using AVSFacturador.Helpers;

namespace AVSFacturador
{
    public static class GeneradorCadenaOriginal
    {
        public static string GenerarCadena(byte[] xml, string version, string xsltPath)
        {
            try
            {
                ExtraerXSLT();
                XslCompiledTransform cadenaXslTransform = null;
                StringWriterWithEncoding stWrite = new StringWriterWithEncoding(new StringBuilder(), System.Text.Encoding.UTF8);

                System.IO.MemoryStream stream = new MemoryStream(xml);

                XPathDocument myXPathDocument = new XPathDocument(stream);
                if (cadenaXslTransform == null)
                {
                    cadenaXslTransform = new XslCompiledTransform();
                    var location = Assembly.GetExecutingAssembly().Location;

                    switch (version)
                    {
                        case "TFD":
                            var tfd = new Uri(FileTools.GetIsolatedFullFilePath("cadenaoriginal_TFD_1_0.xslt")).AbsoluteUri;
                            cadenaXslTransform.Load(tfd);
                            break;
                        case "TFD11":
                            var tfd11 = new Uri(FileTools.GetIsolatedFullFilePath("cadenaoriginal_TFD_1_1.xslt")).AbsoluteUri;
                            cadenaXslTransform.Load(tfd11);
                            break;
                        case "3.2":
                            var cfd32 = new Uri(FileTools.GetIsolatedFullFilePath("cadenaoriginal_3_2.xslt")).AbsoluteUri;
                            cadenaXslTransform.Load(cfd32);
                            break;
                        case "3.3":
                            var cfd33 = new Uri(FileTools.GetIsolatedFullFilePath("cadenaoriginal_3_3.xslt")).AbsoluteUri;
                            cadenaXslTransform.Load(cfd33);
                            break;
                        case "4.0":
                            //var x = FileTools.GetIsolatedFullFilePath("cadenaoriginal_3_3.xslt");
                            //var cfd40 = new Uri(FileTools.GetIsolatedFullFilePath("cadenaoriginal_4_0.xslt")).AbsoluteUri;
                            var cfd40 = new Uri(xsltPath).AbsoluteUri;
                            cadenaXslTransform.Load(cfd40);
                            break;
                        default:
                            throw new InvalidDataException("La versión es invalida. Los valores aceptados son unicamente TFD, 3.2, 3.3, 4.0");
                    }
                }
                cadenaXslTransform.Transform(myXPathDocument, null, stWrite);

                stWrite.Close();
                return stWrite.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //TODO: Hay que mejorar este metodo. Es un pedo con todos los cambios que mete el SAT y el Isolated Storage.
        //Aunque no es problema en producción en pruebas locales y unitarias puede arrojar errores falsos.
        private static void ExtraerXSLT()
        {
            var fileList = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(m => m.StartsWith("NimFacturador33.xslt"));

            foreach (var file in fileList)
            {
                var nombreArchivo = file.Replace("NimFacturador33.xslt.", "");
#if DEBUG
                byte[] archivo = FileTools.ReadResourcesFile(file);
                FileTools.ByteArrayToIsolatedFile(nombreArchivo, archivo);
#else
                if (!FileTools.FileExistsInIsolatedFile(nombreArchivo))
                {
                    byte[] archivo = FileTools.ReadResourcesFile(file);
                    FileTools.ByteArrayToIsolatedFile(nombreArchivo, archivo);
                }
#endif
            }
        }
    }


    internal class StringWriterWithEncoding : StringWriter
    {
        Encoding encoding;

        public StringWriterWithEncoding(StringBuilder builder, Encoding encoding)
            : base(builder)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}
