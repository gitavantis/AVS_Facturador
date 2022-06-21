using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSFacturador
{
    public interface IValidacionAddendaComplemento
    {

        void RegistrarTipos();
        void ValidarInformacion();
        
        void AjustarValidacionesSAT();
    }
}
