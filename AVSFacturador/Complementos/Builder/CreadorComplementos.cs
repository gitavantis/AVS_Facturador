using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AVSFacturador.Complementos.Funciones
{
    public class CreadorComplementos
    {
        public IComprobante Comprobante { get; set; }
        public IEnumerable<IComplemento> Complementos = new List<IComplemento>();
        public IEnumerable<IComplemento> Addendas = new List<IComplemento>();


        public CreadorComplementos(byte[] xmlBase)
        {
            CrearIComprobante crearIComp = new CrearIComprobante();
            this.Comprobante = crearIComp.CrearComprobanteFactory(xmlBase);
            PonerComplementos(this.Comprobante);
        }

        public CreadorComplementos(IComprobante Comprobante )
        {
            this.Comprobante = Comprobante;
            PonerComplementos(this.Comprobante);
        }


        public object getComplementoNomina12()
        {
            object Complemento = null;
            var complemenetoNomina12 = this.Complementos.FirstOrDefault(c => c is AVSFacturador.Complementos.Nomina12.Nomina12);
            if (complemenetoNomina12 != null)
            {
                Complemento = complemenetoNomina12 as AVSFacturador.Complementos.Nomina12.Nomina12;
            }
            return Complemento;
        }




        private void PonerComplementos(IComprobante comprobante)
        {
            //Se ponen los impuestos locales. aplica lo mismo que para los regulares.
            List<IComplemento> complementos = new List<IComplemento>();
            List<IComplemento> addendas = new List<IComplemento>();

            if (comprobante.DEComplementos != null)
            {
                foreach (XmlElement el in comprobante.DEComplementos)
                {
                    switch (el.Name)
                    {
                        case "implocal:ImpuestosLocales":
                            var impuestosLocales = PonerComplementoImpuestosLocales(el);

                            complementos.Add(impuestosLocales);
                            break;

                            //case "tfd:TimbreFiscalDigital":
                            //     PonerComplementoTimbreFiscalDigital(el);
                            //     break;
                    }
                }
                var listaComplementos = GetAddendasComplementos(comprobante.DEComplementos);
                complementos.AddRange(listaComplementos);
            }
            if (comprobante.DEAddendas != null)
            {
                if (comprobante.DEVersionComprobante == "3.3")
                {
                    var listaAddendasXML = comprobante.DEXMLAddendas;
                    if (listaAddendasXML != null)
                    {
                        addendas = GetAddendasComplementos(listaAddendasXML).ToList();
                    }
                }
            }

            Complementos = complementos;
            Addendas = addendas;
        }

        private ImpuestosLocales PonerComplementoImpuestosLocales(XmlElement el)
        {
            var nuevoimp = new ImpuestosLocales();
            nuevoimp = nuevoimp.Deserialize(el.OuterXml);
            return nuevoimp;
        }


        private ICollection<IComplemento> GetAddendasComplementos(IEnumerable<XmlElement> XmlComplementos)
        {
            ICollection<IComplemento> complementosAddendas = new List<IComplemento>();
            if (XmlComplementos != null)
            {
                var builderDirector = new ComplementoBuilderDirector(XmlComplementos);
                complementosAddendas = builderDirector.constructComprobanteComplementos();
            }
            return complementosAddendas;
        }


    }
}
