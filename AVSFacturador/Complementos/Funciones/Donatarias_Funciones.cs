using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSFacturador.Complementos
{
    public partial class Donatarias : IComplemento
    {
        public void AjustarValidacionesSAT()
        {

        }

        public List<string> ValidarInformacion()
        {
            return new List<string>(){ };
        }
    }
}
