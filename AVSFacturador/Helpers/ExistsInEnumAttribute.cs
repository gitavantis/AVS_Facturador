using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AVSFacturador.Helpers
{
    class ExistsInEnumAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "El valor '{0}' no se encuentra en el arreglo {1}.";

        private RequiredAttribute innerAttribute = new RequiredAttribute();
        private string _value;

        private Type _enumType;

        public ExistsInEnumAttribute(Type enumType) : base(_defaultErrorMessage)
        {
            _enumType = enumType;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString,
                _value.ToString(), _enumType.ToString());
        }

        public override bool IsValid(object value)
        {
            if (value != null)
                if (value is String)
                {
                    _value = value.ToString();
                    return Enum.IsDefined(_enumType, "Item" + _value) || Enum.IsDefined(_enumType, _value);
                }
                else
                    throw new ValidationException("ExistsInEnumAttribute only works on string types.");
            else
                return true;
        }
    }

    class ExistsExplicitInEnumAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "El valor '{0}' no se encuentra en el arreglo {1}.";

        private RequiredAttribute innerAttribute = new RequiredAttribute();
        private string _value;

        private Type _enumType;

        public ExistsExplicitInEnumAttribute(Type enumType) : base(_defaultErrorMessage)
        {
            _enumType = enumType;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString,
                _value.ToString(), _enumType.ToString());
        }

        public override bool IsValid(object value)
        {
            if (value != null)
                if (value is String)
                {
                    _value = value.ToString();
                    return GetXmlAttrNameFromEnumValueHelper.ExistsInXmlAttribute(_enumType, value as string);
                }
                else
                    throw new ValidationException("ExistsInEnumAttribute only works on string types.");
            else
                return true;
        }
    }
}
