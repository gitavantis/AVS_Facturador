using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AVSFacturador.Complementos.Funciones
{
   public class ComplementoBuilderDirector
    {
        private ComplementoBuilderFactory BuilderFactory;
        private ICollection<XmlElement> xmlComplementos;

        //complementos de Comprobante Complemento, business object de xsd
        private ICollection<IComplemento> ComprobanteComplementos;
        private ICollection<IComplementoBuilder> builders;

        
        public ComplementoBuilderDirector(IEnumerable<XmlElement> _xmlComplementos)
        {
            xmlComplementos = _xmlComplementos.ToList();
            ComprobanteComplementos = new List<IComplemento>();
            BuilderFactory = new ComplementoBuilderFactory();
            builders = new List<IComplementoBuilder>();
        }


        public void AddBuilderTyte(Type bulder)
        {
            BuilderFactory.AddBuilderType(bulder);
        }


        private void getBuildersByXmlElements()
        {
            if (xmlComplementos != null)
            {
                foreach (var xmlComplemento in xmlComplementos)
                {
                    var builder = BuilderFactory.getBuilderByXmlElement(xmlComplemento);
                    if (builder != null)
                    {
                        builders.Add(builder);
                    }

                }
            }
        }
          
        public ICollection<IComplemento> constructComprobanteComplementos()
        {
            getBuildersByXmlElements();
            foreach (var builder in builders)
            {
                try
                {
                    var addendaComplementoComplemento = builder.genObjectComprobanteComplemento();
                    if (addendaComplementoComplemento != null)
                    {
                        this.ComprobanteComplementos.Add(addendaComplementoComplemento);
                    }
                }
                catch (Exception e)
                {
                    //no se pudo obtener el objeto deserializado
                }
            }
            return ComprobanteComplementos;
        }
    }
}
 