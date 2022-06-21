using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AVSFacturador.Addendas
{
    public partial class AddendaDTO : IComplemento
    {

        public void AjustarValidacionesSAT()
        {

        }

        public List<string> ValidarInformacion()
        {
            return new List<string>() { };
        }

        private string Serialize()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("dto", "http://www.ncontrol.mx/AddendaNimtech");

            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(this.GetType());
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xmlSerializer.Serialize(xmlTextWriter, this, ns);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            System.IO.StreamReader streamReader = new System.IO.StreamReader(memoryStream);
            return streamReader.ReadToEnd();
        }

        public XmlElement ObtenerAddendaXml()
        {
            var xmlString = this.Serialize();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            return xmlDoc.DocumentElement;
        }

    }
}

