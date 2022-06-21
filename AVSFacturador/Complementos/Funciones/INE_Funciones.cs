using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSFacturador.Complementos
{
    public partial class INE : IComplemento
    {
        public void AjustarValidacionesSAT()
        {
            

        }

        public List<string> ValidarInformacion()
        {
            var respuesta = new List<string>();

            if (this.TipoProceso == INETipoProceso.Ordinario)
            {
                if( ! tipoComiteFieldSpecified  )
                {
                    respuesta.Add("Atributo TipoProceso: con valor Ordinario, Debe existir el atributo ine:TipoComite.");
                }
            }
            

            if (this.TipoProceso == INETipoProceso.Precampaña ||
                this.TipoProceso == INETipoProceso.Campaña)
            {
                var totalEntidades = this.entidadField == null ? 0  : this.entidadField.Count( e=> e.AmbitoSpecified );
                if (totalEntidades == 0)
                {
                    respuesta.Add("Atributo TipoProceso con el valor Precampaña o el valor Campaña, Se debe registrar al menos un elemento ine:Entidad con atribulo Entidad:Ambito.");
                }

                if( this.tipoComiteFieldSpecified  )
                {
                    respuesta.Add("Atributo TipoProceso con el valor Precampaña o el valor Campaña, No debe existir el atributo ine:TipoComite.");
                }

                if (this.idContabilidadFieldSpecified)
                {
                    respuesta.Add("Atributo TipoProceso con el valor Precampaña o el valor Campaña, No debe existir el atributo ine:IdContabilidad.");
                }

            }

            if ( this.tipoComiteFieldSpecified && this.TipoComite == INETipoComite.EjecutivoNacional)
            {
                //Puede existir el atributo ine:IdContabilidad.
                //No debe existir ningún elemento ine:Entidad.

                if (this.Entidad != null && this.Entidad.Count() > 0)
                {
                    respuesta.Add("Atributo TipoComite con valor Ejecutivo Nacional, no debe existir ningún elemento ine:Entidad.");
                }
            }


            if (this.tipoComiteFieldSpecified &&  this.TipoComite == INETipoComite.EjecutivoEstatal)
            {
                //No debe existir el atributo ine:IdContabilidad
                //Debe existir al menos un elemento ine: Entidad y en cada
                //entidad que se registre no debe existir el atributo
                //ine:Entidad: Ambito
                //this.IdContabilidadSpecified = false;
                if( this.idContabilidadFieldSpecified)
                {
                    respuesta.Add("Atributo TipoComite con valor Ejecutivo Estatal, no debe existir ine:idContabilidad.");
                }

                if (this.Entidad == null ||  
                    (this.Entidad != null  && ( this.Entidad.Count() == 0 || this.Entidad.Count(e => e.AmbitoSpecified) > 0))
                    )
                {
                    respuesta.Add("Atributo TipoComite con valor Ejecutivo Estatal, debe existir al menos un elemento ine: Entidad  y en cada entidad que se registre no debe existir el atributo ine: Entidad:Ambito.");
                }
            }


            if (this.tipoComiteFieldSpecified &&  this.TipoComite == INETipoComite.DirectivoEstatal )
            {
                //Puede existir el atributo ine:IdContabilidad
                // Debe existir al menos un elemento ine: Entidad y en cada
                //entidad que se registre no debe existir el atributo
                //ine:Entidad: Ambito

                if (this.Entidad == null || 
                    (this.Entidad != null && (this.Entidad.Count() == 0  || this.Entidad.Count(e => e.AmbitoSpecified)  >  0 ))
                    )
                {
                    respuesta.Add("Atributo TipoComite con valor Directivo Estata, debe existir al menos un elemento ine:Entidad  y en cada entidad que se registre no debe existir el atributo ine: Entidad:Ambito.");
                }
            }

            if (this.Entidad != null) {
                foreach ( var entidad in this.Entidad)
                {
                    if( entidad.Ambito == INEEntidadAmbito.Local)
                    {
                        if( entidad.ClaveEntidad == t_ClaveEntidad.NAC ||
                            entidad.ClaveEntidad == t_ClaveEntidad.CR1 ||
                            entidad.ClaveEntidad == t_ClaveEntidad.CR2 ||
                            entidad.ClaveEntidad == t_ClaveEntidad.CR3 ||
                            entidad.ClaveEntidad == t_ClaveEntidad.CR4 ||
                            entidad.ClaveEntidad == t_ClaveEntidad.CR5 )
                        {
                            respuesta.Add("No se pueden seleccionar las claves NAC, CR1, CR2, CR3,CR4 y CR5 en el atributo ine:EntidadCircunscripcion:ClaveEntidadCircunscripcion.");
                        }
                    }

                }
            }
            return respuesta;   
        }
    }
}
