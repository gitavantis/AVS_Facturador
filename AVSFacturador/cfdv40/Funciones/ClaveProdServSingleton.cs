using AVSFacturador.cfdv40_full;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSFacturador.cfdv40.Funciones
{
    public class ClaveProdServSingleton
    {

        static ClaveProdServSingleton _instance;


        public static ClaveProdServSingleton Instance
        {
            get
            {
                return _instance ?? (_instance = new ClaveProdServSingleton());
            }
        }

        private ClaveProdServSingleton()
        {

        }

        public c_ClaveProdServ ClaveProdServ { get; set; }
    }
}
