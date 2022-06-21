using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AVSFacturador.Addendas
{
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ncontrol.mx/AddendaNimtech")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ncontrol.mx/AddendaNimtech", IsNullable = false)]
    public partial class AddendaDTO
    {
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string schemaLocation { get; set; }

        public AddendaDTO()
        {
            this.schemaLocation = "http://www.ncontrol.mx/AddendaNimtech http://sistema.ncontrol.mx/Content/xsd/AddendaNimtech.xsd";
        }
        public static bool Deserialize(string xml, out AddendaDTO obj, out System.Exception exception)
        {
            exception = null;
            obj = null;
            try
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(xml);
                System.IO.StreamReader stringReader = new System.IO.StreamReader(new System.IO.MemoryStream(bytes));
                System.Xml.XmlTextReader xmlTextReader = new System.Xml.XmlTextReader(stringReader);

                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(AddendaDTO));
                obj = (AddendaDTO)xmlSerializer.Deserialize(xmlTextReader);
                return true;
            }
            catch (System.Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        public AddendaDTO Deserialize(string xml)
        {
            System.Exception e;
            AddendaDTO addendaNimtech;
            if (Deserialize(xml, out addendaNimtech, out e))
            {
                return addendaNimtech;
            }
            else
            {
                throw e;
            }
        }

        public DatoAdicional[] ComprobanteDatosAdicionales { get; set; }

        public ConceptoDatosAdicionales[] ConceptosDatosAdicionales { get; set; }

        public DatoAdicional[] TotalesDatosAdicionales { get; set; }
    }


    //public class ComprobanteDatosAdicionales
    //{
    //    ICollection<DatoAdicional> DatosAdicionales { get; set; }
    //}

    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ncontrol.mx/AddendaNimtech")]
    public class ConceptoDatosAdicionales
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int NumeroLineaConcepto { get; set; }

        public DatoAdicional[] DatosAdicionales { get; set; }
    }

    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ncontrol.mx/AddendaNimtech")]
    public class DatoAdicional
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Nombre { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Valor { get; set; }
    }




    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ncontrol.mx/AddendaNimtech")]
    //[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ncontrol.mx/AddendaNimtech", IsNullable = false)]
    //public partial class AddendaDTO
    //{
    //    [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    //    public string schemaLocation { get; set; }

    //    public AddendaDTO()
    //    {
    //        this.schemaLocation = "http://www.ncontrol.mx/AddendaNimtech http://sistema.ncontrol.mx/Content/xsd/AddendaNimtech.xsd";
    //    }
    //    public static bool Deserialize(string xml, out AddendaDTO obj, out System.Exception exception)
    //    {
    //        exception = null;
    //        obj = null;
    //        try
    //        {
    //            var bytes = System.Text.Encoding.UTF8.GetBytes(xml);
    //            System.IO.StreamReader stringReader = new System.IO.StreamReader(new System.IO.MemoryStream(bytes));
    //            System.Xml.XmlTextReader xmlTextReader = new System.Xml.XmlTextReader(stringReader);

    //            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(AddendaDTO));
    //            obj = (AddendaDTO)xmlSerializer.Deserialize(xmlTextReader);
    //            return true;
    //        }
    //        catch (System.Exception ex)
    //        {
    //            exception = ex;
    //            return false;
    //        }
    //    }

    //    public AddendaDTO Deserialize(string xml)
    //    {
    //        System.Exception e;
    //        AddendaDTO addendaNimtech;
    //        if (Deserialize(xml, out addendaNimtech, out e))
    //        {
    //            return addendaNimtech;
    //        }
    //        else
    //        {
    //            throw e;
    //        }
    //    }

    //    public DatoAdicional[] ComprobanteDatosAdicionales { get; set; }

    //    public ConceptoDatosAdicionales[] ConceptosDatosAdicionales { get; set; }

    //    public DatoAdicional[] TotalesDatosAdicionales { get; set; }
    //}


    ////public class ComprobanteDatosAdicionales
    ////{
    ////    ICollection<DatoAdicional> DatosAdicionales { get; set; }
    ////}

    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ncontrol.mx/AddendaNimtech")]
    //public class ConceptoDatosAdicionales
    //{
    //    [System.Xml.Serialization.XmlAttributeAttribute()]
    //    public int NumeroLineaConcepto { get; set; }

    //    public DatoAdicional[] DatosAdicionales { get; set; }
    //}

    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ncontrol.mx/AddendaNimtech")]
    //public class DatoAdicional
    //{
    //    [System.Xml.Serialization.XmlAttributeAttribute()]
    //    public string Nombre { get; set; }

    //    [System.Xml.Serialization.XmlAttributeAttribute()]
    //    public string Valor { get; set; }
    //}
}
