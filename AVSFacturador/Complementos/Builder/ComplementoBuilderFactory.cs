using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AVSFacturador.Complementos.Funciones
{
    public class ComplementoBuilderFactory
    {
        
        //agregamos las lista de todos los builders de complementos, para generarlo con generics
        protected ICollection<Type> BuildersTypes = new List<Type>()
        {
            typeof(ComplementoDonatariasBuilder),
            typeof(ComplementoIEDUBuilder),
            typeof(ComplementoNotariosPublicosBuilder),
            typeof(ComplementoINEBuilder),
            typeof( ComplementoNomina12Builder ),
            //typeof( AddendaNimtechBuilder ),
            typeof( ComplementoPago10Builder )
        };


        public void AddBuilderType(Type BuilderType)
        {
            BuildersTypes.Add(BuilderType);
        }
         
        public IComplementoBuilder getBuilderByXmlElement(XmlElement XmlElement)
        {
            IComplementoBuilder builder = null;
            foreach (Type BuilderType in BuildersTypes)
            {
                //BuildersType
                var methodInfo = BuilderType.GetMethod("isTypeOfXmlElement");

                var esComplemento = (bool)methodInfo.Invoke(null, new[] { XmlElement.Name });
                if (esComplemento)
                {
                    builder = (IComplementoBuilder)Activator.CreateInstance(BuilderType, new[] { XmlElement });
                }
            }
            return builder;
        }
    }
}
 