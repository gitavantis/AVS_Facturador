using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AVSFacturador.Helpers
{
    public class GetXmlAttrNameFromEnumValueHelper
    {
        public static string GetXmlAttrNameFromEnumValue(Type enumType, object pEnumVal)
        {
            Type type = pEnumVal.GetType();
            FieldInfo info = type.GetField(Enum.GetName(enumType, pEnumVal));
            XmlEnumAttribute att = (XmlEnumAttribute)info.GetCustomAttributes(typeof(XmlEnumAttribute), false)[0];

            return att.Name;
        }

        public static object GetCode(Type enumType, string value)
        {
            foreach (object o in System.Enum.GetValues(enumType))
            {
                if (GetXmlAttrNameFromEnumValue(enumType, o).Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return o;
                }
            }

            throw new ArgumentException("No XmlEnumAttribute code exists for type " + enumType.ToString() + " corresponding to value of " + value);
        }

        public static bool ExistsInXmlAttribute(Type enumType, string value)
        {
            foreach (object o in System.Enum.GetValues(enumType))
            {
                if (GetXmlAttrNameFromEnumValue(enumType, o).Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
