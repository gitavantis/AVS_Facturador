using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSFacturador.Helpers
{
    public static class ObjectValidation
    {
        public static List<string> ValidateObject(object objeto)
        {
            var respuesta = new List<string>();

            ValidationContext context = new ValidationContext(objeto, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(objeto, context, results, true);

            if (!valid)
            {
                foreach (var error in results)
                {
                    respuesta.Add(error.MemberNames.First() + " - " + error.ErrorMessage);
                }
            }

            return respuesta;
        }
    }
}
