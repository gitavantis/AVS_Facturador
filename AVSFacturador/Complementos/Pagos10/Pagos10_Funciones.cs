using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AVSFacturador.Complementos
{
    public partial class Pagos : IComplemento
    {
        public virtual string Serialize()
        {
            string result = Encoding.UTF8.GetString(SerializeToArray());
            return result;
        }

        public byte[] SerializeToArray()
        {
            this.AjustarValidacionesSAT();

            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(Pagos));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("pago10", "http://www.sat.gob.mx/Pagos");

            xs.Serialize(memoryStream, this, ns);
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            System.IO.StreamReader streamReader = new System.IO.StreamReader(memoryStream);
            return UTF8Encoding.UTF8.GetBytes(streamReader.ReadToEnd());
        }

        public Pagos Deserialize(byte[] xml)
        {
            Pagos pago;

            System.IO.StreamReader stringReader = new System.IO.StreamReader(new MemoryStream(xml));
            System.Xml.XmlTextReader xmlTextReader = new System.Xml.XmlTextReader(stringReader);

            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Pagos));

            pago = (Pagos)xmlSerializer.Deserialize(xmlTextReader);

            return pago;
        }

        public Pagos Deserialize(string xml)
        {
            var bytes = Encoding.UTF8.GetBytes(xml);
            return Deserialize(bytes);
        }

        //public List<string> ValidarInformacion()
        //{
        //    return null;
        //}

        public void AjustarValidacionesSAT()
        {
            if (this.Pago != null && this.Pago.Count() > 0)
            {
                foreach (var pag in this.Pago)
                {
                    //TODO: Falta prueba unitaria
                    //CRP208 - El valor del campo Monto debe tener hasta la cantidad de decimales que soporte la moneda registrada en el campo MonedaP.
                    pag.Monto = Math.Round(pag.Monto, 2);

                    //TODO: Falta prueba unitaria
                    //CRP201, CRP202, CRP203, CRP204
                    if (pag.MonedaP == "MXN")
                    {
                        pag.TipoCambioP = 1;
                        pag.TipoCambioPSpecified = false;
                    }

                    if (pag.DoctoRelacionado != null && pag.DoctoRelacionado.Count() > 0)
                    {
                        foreach (var docto in pag.DoctoRelacionado)
                        {
                            //TODO: Falta prueba unitaria
                            //Ajuste regla CRP224 - El valor del campo ImpPagado debe tener hasta la cantidad de decimales que soporte la moneda registrada en el campo MonedaDR.
                            if (docto.ImpPagado > 0m)
                                docto.ImpPagado = Math.Round(docto.ImpPagado, 2);
                            else
                                docto.ImpPagadoSpecified = false;

                            //TODO: Falta prueba unitaria
                            //CRP219 - El campo TipoCambioDR no se debe registrar.
                            if (pag.MonedaP == docto.MonedaDR)
                                docto.TipoCambioDRSpecified = false;

                            //TODO: Falta prueba unitaria
                            //CRP222 - El valor del campo ImpSaldoAnt debe tener hasta la cantidad de decimales que soporte la moneda registrada en el campo MonedaDR.
                            if (docto.ImpSaldoAnt > 0m)
                                docto.ImpSaldoAnt = Math.Round(docto.ImpSaldoAnt, 2);
                            else
                                docto.ImpSaldoAntSpecified = false;
                        }
                    }
                }
            }
        }

        public List<string> ValidarInformacion()
        {

            
            var lista = new List<string>();
            if (this.Pago != null && this.Pago.Count() > 0)
            {
                foreach (var pag in this.Pago)
                {
                    if (pag.DoctoRelacionado != null && pag.DoctoRelacionado.Count() > 0)
                    {
                        foreach (var docto in pag.DoctoRelacionado)
                        {
                            //CRP234 - El campo ImpSaldoAnt se debe registrar
                            if (!docto.ImpSaldoAntSpecified)
                            {
                                lista.Add("El campo ImpSaldoAnt se debe registrar.");
                            }
                            
                            //CRP226 - El campo ImpSaldoInsoluto debe ser mayor o igual a cero y calcularse con la suma de los campos ImSaldoAnt menos el ImpPagado o el Monto
                            if (docto.ImpSaldoInsolutoSpecified &&  docto.ImpSaldoInsoluto < 0) {
                                lista.Add("El campo ImpSaldoInsoluto (" + docto.ImpSaldoInsoluto.ToString() +  ") debe ser mayor o igual a cero y calcularse con la suma de los campos ImSaldoAnt menos el ImpPagado o el Monto.");
                            }
                        }

                        //CRP206 - La suma de los valores registrados en el campo ImpPagado de los apartados DoctoRelacionado no es menor o igual que el valor del campo Monto
                        //var totalPagados = pag.DoctoRelacionado.Where(m => m.ImpPagadoSpecified).Sum(d => d.ImpPagado);
                        //if(totalPagados > pag.Monto)
                        //{
                        //    lista.Add("La suma de los valores registrados en el campo ImpPagado de los apartados DoctoRelacionado (" + totalPagados.ToString() + ") no es menor o igual que el valor del campo Monto (" + pag.Monto.ToString() + ").");
                        //}
                    }
                }
            }
            return lista;
        }
    }
}
