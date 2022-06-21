using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using AVSFacturador.Helpers;
using AVSFacturador.Complementos.Funciones;
using AVSFacturador.cfdv33.Funciones;

namespace AVSFacturador.cfdv33
{
    [MetadataType(typeof(Comprobante33_Validation))]
    public partial class Comprobante : IComprobante
    {
        [XmlAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string xsiSchemaLocation = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd";

        public string DEVersionComprobante
        {
            get
            {
                return this.Version;
            }
        }

        public string DEEmisorRfc
        {
            get
            {
                if (this.Emisor != null)
                    return this.Emisor.Rfc;
                return null;
            }
        }

        public string DEEmisorRazonSocial
        {
            get
            {
                if (this.Emisor != null)
                    return this.Emisor.Nombre;
                return null;
            }
        }
        public string DEUsoCFDI
        {
            get
            {
                if (this.Receptor != null)
                    return this.Receptor.UsoCFDI;
                return null;
            }
        }


        public string DEReceptorRfc
        {
            get
            {
                if (this.Receptor != null)
                    return this.Receptor.Rfc;
                return null;
            }
        }

        public string DEReceptorRazonSocial
        {
            get
            {
                if (this.Receptor != null)
                    return this.Receptor.Nombre;
                return null;
            }
        }

        public string DENoCertificadoEmisor
        {
            get { return this.NoCertificado; }
        }

        public DateTime DEFechacomprobante
        {
            get
            {
                return this.Fecha;
            }
        }

        public string DESerieyFolio
        {
            get
            {
                return this.Serie + " - " + this.Folio;
            }
        }

        public string DETipoDeComprobante
        {
            get
            {
                return this.TipoDeComprobante.ToString();
            }
        }

        public string DEMoneda
        {
            get
            {
                return this.Moneda;
            }
        }

        public decimal DETipoCambio
        {
            get
            {
                return this.TipoCambio;
            }
        }

        public decimal DESubtotal
        {
            get
            {
                return this.SubTotal;
            }
        }

        public decimal DEDescuento
        {
            get
            {
                return this.Descuento;
            }
        }

        public decimal DETotal
        {
            get
            {
                return this.Total;
            }
        }

        public decimal DETraslados
        {
            get
            {
                if (this.Impuestos != null)
                    if (this.Impuestos.TotalImpuestosTrasladadosSpecified)
                        return this.Impuestos.TotalImpuestosTrasladados;

                return 0;
            }
        }

        public decimal DERetenciones
        {
            get
            {
                if (this.Impuestos != null)
                    if (this.Impuestos.TotalImpuestosRetenidosSpecified)
                        return this.Impuestos.TotalImpuestosRetenidos;

                return 0;
            }
        }

        public string DEMetodoPago
        {
            get
            {
                return this.MetodoPago;
            }
        }

        public string DEFormaPago
        {
            get
            {
                return this.FormaPago;
            }
        }        
        public string DESello
        {
            get
            {
                return this.Sello;
            }
        }

        public System.Xml.XmlElement[] DEComplementos
        {
            get
            {
                return this.Complemento != null ? this.Complemento.Any : null;
            }
        }

        public System.Xml.XmlElement[] DEXMLAddendas
        {
            get
            {
                return this.Addenda != null ? this.Addenda.Any : null;
            }
        }


        public IEnumerable<string> DERelacionados
        {
            get
            {
                IEnumerable<string> listaRelacionados = new List<string>();
                if (this.CfdiRelacionados != null &&
                    this.CfdiRelacionados.CfdiRelacionado != null)
                {
                    listaRelacionados = this.CfdiRelacionados.CfdiRelacionado.Select(r => r.UUID);
                }
                return listaRelacionados;
            }
        }

        public void PonerSelloEmisor(string sello)
        {
            this.Sello = sello;
        }

        //TODO: Hacer prueba unitaria de esta función para asegurar que no se quitan complementos.
        public void PonerComplemento(string complemento)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(complemento);

            if (this.Complemento == null)
            {
                this.Complemento = new ComprobanteComplemento()
                {
                    Any = new XmlElement[0],
                };
            }
            else if (this.Complemento.Any == null)
            {
                this.Complemento.Any = new XmlElement[0];
            }

            int count = 0;
            var Complementos = new System.Xml.XmlElement[this.Complemento.Any.Count() + 1];
            Complementos[count] = xmldoc.DocumentElement;
            count++;

            foreach (var comp in this.Complemento.Any)
            {
                Complementos[count] = comp;
                count++;
            }


            this.Complemento = new ComprobanteComplemento()
            {
                Any = Complementos,
            };
        }


        //TODO: Hacer prueba unitaria de esta función para asegurar que no se quitan las addendas.
        public void PonerXmlAddenda(string xmlAddenda)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlAddenda);

            if (this.Addenda == null)
            {
                this.Addenda = new ComprobanteAddenda()
                {
                    Any = new XmlElement[0],
                };
            }
            else if (this.Addenda.Any == null)
            {
                this.Addenda.Any = new XmlElement[0];
            }

            int count = 0;
            var addendas = new System.Xml.XmlElement[this.Addenda.Any.Count() + 1];
            addendas[count] = xmldoc.DocumentElement;
            count++;

            foreach (var comp in this.Addenda.Any)
            {
                addendas[count] = comp;
                count++;
            }


            this.Addenda = new ComprobanteAddenda()
            {
                Any = addendas,
            };
        }

        public Object DEAddendas
        {
            get
            {
                return this.Addenda != null ? this.Addenda : null;
            }
        }

        public void PonerAddendas(Object addendas)
        {
            if (addendas == null || addendas.GetType() == typeof(cfdv33.ComprobanteAddenda))
            {
                this.Addenda = addendas as cfdv33.ComprobanteAddenda;
            }
        }

        public void PonerDatosCertificado(byte[] certFile)
        {
            DatosCertificado datosCert = new DatosCertificado(certFile);
            this.NoCertificado = datosCert.SerialNumber;
            this.Certificado = datosCert.CertBase64;
        }


        public IComprobante Deserialize(byte[] xml)
        {
            Comprobante comprobante;

            System.IO.StreamReader stringReader = new System.IO.StreamReader(new MemoryStream(xml));
            System.Xml.XmlTextReader xmlTextReader = new System.Xml.XmlTextReader(stringReader);

            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Comprobante));

            comprobante = (Comprobante)xmlSerializer.Deserialize(xmlTextReader);

            return comprobante;
        }

        public IComprobante Deserialize(string xml)
        {
            var bytes = Encoding.UTF8.GetBytes(xml);
            return Deserialize(bytes);
        }

        public string Serialize()
        {
            string result = Encoding.UTF8.GetString(SerializeToArray());
            return result;
        }

        public byte[] SerializeToArray()
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(Comprobante));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            ns.Add("cfdi", "http://www.sat.gob.mx/cfd/3");

            if (this.Conceptos != null && this.Conceptos != null)
                foreach (var concepto in this.Conceptos)
                {
                    if (concepto.ComplementoConcepto != null && concepto.ComplementoConcepto.Any != null)
                    {
                        foreach (var complementoConcepto in concepto.ComplementoConcepto.Any)
                        {
                            if (complementoConcepto.Prefix == "iedu")
                            {
                                ns.Add("iedu", "http://www.sat.gob.mx/iedu");

                                if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/iedu http://www.sat.gob.mx/sitio_internet/cfd/iedu/iedu.xsd"))
                                    this.xsiSchemaLocation += " http://www.sat.gob.mx/iedu http://www.sat.gob.mx/sitio_internet/cfd/iedu/iedu.xsd ";
                            }
                        }
                    }
                }

            if (this.Complemento != null && this.Complemento.Any != null)
                foreach (var n in this.Complemento.Any)
                {
                    if (n.Prefix == "tfd")
                    {
                        ns.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/sitio_internet/cfd/TimbreFiscalDigital/TimbreFiscalDigitalv11.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/sitio_internet/cfd/TimbreFiscalDigital/TimbreFiscalDigitalv11.xsd ";
                    }
                    if (n.Prefix == "implocal")
                    {
                        ns.Add("implocal", "http://www.sat.gob.mx/implocal");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/implocal http://www.sat.gob.mx/sitio_internet/cfd/implocal/implocal.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/implocal http://www.sat.gob.mx/sitio_internet/cfd/implocal/implocal.xsd ";
                    }
                    if (n.Prefix == "iedu")
                    {
                        ns.Add("iedu", "http://www.sat.gob.mx/iedu");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/iedu http://www.sat.gob.mx/sitio_internet/cfd/iedu/iedu.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/iedu http://www.sat.gob.mx/sitio_internet/cfd/iedu/iedu.xsd ";
                    }
                    if (n.Prefix == "nomina12")
                    {
                        ns.Add("nomina", "http://www.sat.gob.mx/nomina");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/nomina http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina12.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/nomina http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina12.xsd ";
                    }
                    if (n.Prefix == "donat")
                    {
                        ns.Add("donat", "http://www.sat.gob.mx/donat");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/donat http://www.sat.gob.mx/sitio_internet/cfd/donat/donat11.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/donat http://www.sat.gob.mx/sitio_internet/cfd/donat/donat11.xsd ";
                    }
                    if (n.Prefix == "notariospublicos")
                    {
                        ns.Add("notariospublicos", "http://www.sat.gob.mx/notariospublicos");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/notariospublicos http://www.sat.gob.mx/sitio_internet/cfd/notariospublicos/notariospublicos.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/notariospublicos http://www.sat.gob.mx/sitio_internet/cfd/notariospublicos/notariospublicos.xsd ";
                    }
                    if (n.Prefix == "ine")
                    {
                        ns.Add("ine", "http://www.sat.gob.mx/ine");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/ine http://www.sat.gob.mx/sitio_internet/cfd/ine/ine11.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/ine http://www.sat.gob.mx/sitio_internet/cfd/ine/ine11.xsd ";
                    }
                    if (n.Prefix == "cce11")
                    {
                        ns.Add("cce11", "http://www.sat.gob.mx/ComercioExterior11");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/ComercioExterior11 http://www.sat.gob.mx/sitio_internet/cfd/ComercioExterior11/ComercioExterior11.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/ComercioExterior11 http://www.sat.gob.mx/sitio_internet/cfd/ComercioExterior11/ComercioExterior11.xsd ";
                    }
                    if (n.Prefix == "detallista")
                    {
                        ns.Add("detallista", "http://www.sat.gob.mx/detallista");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/detallista http://www.sat.gob.mx/sitio_internet/cfd/detallista/detallista.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/detallista http://www.sat.gob.mx/sitio_internet/cfd/detallista/detallista.xsd ";
                    }
                    if (n.Prefix == "aerolineas")
                    {
                        ns.Add("aerolineas", "http://www.sat.gob.mx/aerolineas");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/aerolineas http://www.sat.gob.mx/sitio_internet/cfd/aerolineas/aerolineas.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/aerolineas http://www.sat.gob.mx/sitio_internet/cfd/aerolineas/aerolineas.xsd ";
                    }
                    if (n.Prefix == "arteantiguedades")
                    {
                        ns.Add("arteantiguedades", "http://www.sat.gob.mx/arteantiguedades");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/arteantiguedades http://www.sat.gob.mx/sitio_internet/cfd/arteantiguedades/obrasarteantiguedades.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/arteantiguedades http://www.sat.gob.mx/sitio_internet/cfd/arteantiguedades/obrasarteantiguedades.xsd ";
                    }
                    if (n.Prefix == "destruccion")
                    {
                        ns.Add("destruccion", "http://www.sat.gob.mx/certificadodestruccion");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/certificadodestruccion http://www.sat.gob.mx/sitio_internet/cfd/certificadodestruccion/certificadodedestruccion.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/certificadodestruccion http://www.sat.gob.mx/sitio_internet/cfd/certificadodestruccion/certificadodedestruccion.xsd ";
                    }
                    if (n.Prefix == "consumodecombustibles")
                    {
                        ns.Add("consumodecombustibles", "http://www.sat.gob.mx/consumodecombustibles");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/consumodecombustibles http://www.sat.gob.mx/sitio_internet/cfd/consumodecombustibles/consumodecombustibles.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/consumodecombustibles http://www.sat.gob.mx/sitio_internet/cfd/consumodecombustibles/consumodecombustibles.xsd ";
                    }
                    if (n.Prefix == "divisas")
                    {
                        ns.Add("divisas", "http://www.sat.gob.mx/divisas");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/divisas http://www.sat.gob.mx/sitio_internet/cfd/divisas/divisas.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/divisas http://www.sat.gob.mx/sitio_internet/cfd/divisas/divisas.xsd ";
                    }
                    if (n.Prefix == "ecc11")
                    {
                        ns.Add("ecc11", "http://www.sat.gob.mx/EstadoDeCuentaCombustible");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/EstadoDeCuentaCombustible http://www.sat.gob.mx/sitio_internet/cfd/EstadoDeCuentaCombustible/ecc11.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/EstadoDeCuentaCombustible http://www.sat.gob.mx/sitio_internet/cfd/EstadoDeCuentaCombustible/ecc11.xsd ";
                    }
                    if (n.Prefix == "leyendasFisc")
                    {
                        ns.Add("leyendasFisc", "http://www.sat.gob.mx/leyendasFiscales");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/leyendasFiscales http://www.sat.gob.mx/sitio_internet/cfd/leyendasFiscales/leyendasFisc.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/leyendasFiscales http://www.sat.gob.mx/sitio_internet/cfd/leyendasFiscales/leyendasFisc.xsd ";
                    }
                    if (n.Prefix == "pagoenespecie")
                    {
                        ns.Add("pagoenespecie", "http://www.sat.gob.mx/pagoenespecie");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/pagoenespecie http://www.sat.gob.mx/sitio_internet/cfd/pagoenespecie/pagoenespecie.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/pagoenespecie http://www.sat.gob.mx/sitio_internet/cfd/pagoenespecie/pagoenespecie.xsd ";
                    }
                    if (n.Prefix == "valesdedespensa")
                    {
                        ns.Add("valesdedespensa", "http://www.sat.gob.mx/valesdedespensa");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/valesdedespensa http://www.sat.gob.mx/sitio_internet/cfd/valesdedespensa/valesdedespensa.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/valesdedespensa http://www.sat.gob.mx/sitio_internet/cfd/valesdedespensa/valesdedespensa.xsd ";
                    }
                    if (n.Prefix == "pago10")
                    {
                        ns.Add("pago10", "http://www.sat.gob.mx/Pagos");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/Pagos http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos10.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/Pagos http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos10.xsd ";
                    }
                    if (n.Prefix == "pfic")
                    {
                        ns.Add("pfic", "http://www.sat.gob.mx/pfic");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/pfic http://www.sat.gob.mx/sitio_internet/cfd/pfic/pfic.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/pfic http://www.sat.gob.mx/sitio_internet/cfd/pfic/pfic.xsd ";
                    }
                    if (n.Prefix == "registrofiscal")
                    {
                        ns.Add("registrofiscal", "http://www.sat.gob.mx/registrofiscal");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/registrofiscal http://www.sat.gob.mx/sitio_internet/cfd/cfdiregistrofiscal/cfdiregistrofiscal.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/registrofiscal http://www.sat.gob.mx/sitio_internet/cfd/cfdiregistrofiscal/cfdiregistrofiscal.xsd ";
                    }
                    if (n.Prefix == "decreto")
                    {
                        ns.Add("decreto", "http://www.sat.gob.mx/renovacionysustitucionvehiculos");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/renovacionysustitucionvehiculos http://www.sat.gob.mx/sitio_internet/cfd/renovacionysustitucionvehiculos/renovacionysustitucionvehiculos.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/renovacionysustitucionvehiculos http://www.sat.gob.mx/sitio_internet/cfd/renovacionysustitucionvehiculos/renovacionysustitucionvehiculos.xsd ";
                    }
                    if (n.Prefix == "servicioparcial")
                    {
                        ns.Add("servicioparcial", "http://www.sat.gob.mx/servicioparcialconstruccion");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/servicioparcialconstruccion http://www.sat.gob.mx/sitio_internet/cfd/servicioparcialconstruccion/servicioparcialconstruccion.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/servicioparcialconstruccion http://www.sat.gob.mx/sitio_internet/cfd/servicioparcialconstruccion/servicioparcialconstruccion.xsd ";
                    }
                    if (n.Prefix == "spei")
                    {
                        ns.Add("spei", "http://www.sat.gob.mx/spei");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/spei http://www.sat.gob.mx/sitio_internet/cfd/spei/spei.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/spei http://www.sat.gob.mx/sitio_internet/cfd/spei/spei.xsd ";
                    }
                    if (n.Prefix == "terceros")
                    {
                        ns.Add("terceros", "http://www.sat.gob.mx/terceros");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/terceros http://www.sat.gob.mx/sitio_internet/cfd/terceros/terceros11.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/terceros http://www.sat.gob.mx/sitio_internet/cfd/terceros/terceros11.xsd ";
                    }
                    if (n.Prefix == "tpe")
                    {
                        ns.Add("tpe", "http://www.sat.gob.mx/TuristaPasajeroExtranjero");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/TuristaPasajeroExtranjero http://www.sat.gob.mx/sitio_internet/cfd/TuristaPasajeroExtranjero/TuristaPasajeroExtranjero.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/TuristaPasajeroExtranjero http://www.sat.gob.mx/sitio_internet/cfd/TuristaPasajeroExtranjero/TuristaPasajeroExtranjero.xsd ";
                    }
                    if (n.Prefix == "vehiculousado")
                    {
                        ns.Add("vehiculousado", "http://www.sat.gob.mx/vehiculousado");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/vehiculousado http://www.sat.gob.mx/sitio_internet/cfd/vehiculousado/vehiculousado.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/vehiculousado http://www.sat.gob.mx/sitio_internet/cfd/vehiculousado/vehiculousado.xsd ";
                    }
                    if (n.Prefix == "ventavehiculos")
                    {
                        ns.Add("ventavehiculos", "http://www.sat.gob.mx/ventavehiculos");

                        if (!this.xsiSchemaLocation.Contains("http://www.sat.gob.mx/ventavehiculos http://www.sat.gob.mx/sitio_internet/cfd/ventavehiculos/ventavehiculos11.xsd"))
                            this.xsiSchemaLocation += " http://www.sat.gob.mx/ventavehiculos http://www.sat.gob.mx/sitio_internet/cfd/ventavehiculos/ventavehiculos11.xsd ";
                    }
                }

            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xs.Serialize(xmlTextWriter, this, ns);
            var result = memoryStream.ToArray();
            return result;
        }

        private void RegistrarTipos()
        {
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Comprobante), typeof(Comprobante33_Validation)), typeof(Comprobante));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteCfdiRelacionados), typeof(ComprobanteCfdiRelacionados_Validation)), typeof(ComprobanteCfdiRelacionados));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteCfdiRelacionadosCfdiRelacionado), typeof(ComprobanteCfdiRelacionadosCfdiRelacionado_Validation)), typeof(ComprobanteCfdiRelacionadosCfdiRelacionado));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteEmisor), typeof(ComprobanteEmisor_Validation)), typeof(ComprobanteEmisor));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteReceptor), typeof(ComprobanteReceptor_Validation)), typeof(ComprobanteReceptor));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteConcepto), typeof(ComprobanteConcepto_Validation)), typeof(ComprobanteConcepto));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteConceptoImpuestos), typeof(ComprobanteConceptoImpuestos_Validation)), typeof(ComprobanteConceptoImpuestos));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteConceptoImpuestosTraslado), typeof(ComprobanteConceptoImpuestosTraslado_Validation)), typeof(ComprobanteConceptoImpuestosTraslado));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteConceptoImpuestosRetencion), typeof(ComprobanteConceptoImpuestosRetencion_Validation)), typeof(ComprobanteConceptoImpuestosRetencion));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteConceptoInformacionAduanera), typeof(ComprobanteConceptoInformacionAduanera_Validation)), typeof(ComprobanteConceptoInformacionAduanera));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteConceptoCuentaPredial), typeof(ComprobanteConceptoCuentaPredial_Validation)), typeof(ComprobanteConceptoCuentaPredial));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteImpuestos), typeof(ComprobanteImpuestos_Validation)), typeof(ComprobanteImpuestos));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteImpuestosRetencion), typeof(ComprobanteImpuestosRetencion_Validation)), typeof(ComprobanteImpuestosRetencion));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ComprobanteImpuestosTraslado), typeof(ComprobanteImpuestosTraslado_Validation)), typeof(ComprobanteImpuestosTraslado));
        }

        public List<string> ValidarInformacion()
        {
            var respuesta = new List<string>();

            RegistrarTipos();

            respuesta.AddRange(ObjectValidation.ValidateObject(this));

            //Validar RFC del Receptor que sea numero o letra A.
            var validos = "0123456789aA";
            if (this.Receptor != null)
            {
                var digito = this.Receptor.Rfc.Substring(this.Receptor.Rfc.Length - 1, 1);
                if (!validos.Contains(digito))
                    respuesta.Add("Receptor.Rfc no valido. El ultimo caracter del RFC debe ser 0-9 o la letra A.");
            }

            //Validar Regla CFDI33114: El campo TipoCambio se debe registrar cuando el campo Moneda tiene un valor distinto de MXN y XXX. 
            if (this.Moneda != "MXN" && this.Moneda != "XXX")
            {
                if (this.TipoCambioSpecified == false)
                    respuesta.Add("Comprobante.TipoCambio es obligatorio si Moneda es distinto a MXN o XXX.");
            }

            if (this.TipoDeComprobante == "I" || this.TipoDeComprobante == "E" || this.TipoDeComprobante == "N")
            {
                if (!FormaPagoSpecified || String.IsNullOrWhiteSpace(this.FormaPago))
                {
                    respuesta.Add("FormaPago es obligatorio para comprobantes tipo I, E o N.");
                }
                else
                {
                    AVSFacturador.Helpers.ExistsInEnumAttribute enumval = new ExistsInEnumAttribute(typeof(cfdv33_full.c_FormaPago));
                    if (!enumval.IsValid(this.FormaPago))
                        respuesta.Add("FormaPago no se encuentra en el catálogo c_FormaPago");
                }
            }


            if (this.CfdiRelacionados != null)
            {
                respuesta.AddRange(ObjectValidation.ValidateObject(this.CfdiRelacionados));

                if (this.CfdiRelacionados.CfdiRelacionado != null && this.CfdiRelacionados.CfdiRelacionado.Count() > 0)
                    foreach (var cfdirel in CfdiRelacionados.CfdiRelacionado)
                    {
                        respuesta.AddRange(ObjectValidation.ValidateObject(cfdirel));
                    }
            }

            if (this.Emisor != null)
                respuesta.AddRange(ObjectValidation.ValidateObject(this.Emisor));

            if (this.Receptor != null)
            {
                respuesta.AddRange(ObjectValidation.ValidateObject(this.Receptor));
            }

            if (this.Conceptos != null && this.Conceptos.Count() > 0)
                foreach (var concepto in this.Conceptos)
                {
                    respuesta.AddRange(ObjectValidation.ValidateObject(concepto));

                    //Regla CFDI33142 - El campo ClaveProdServ, no contiene un valor del catálogo c_ClaveProdServ
                    var claveProdServConc = concepto.ClaveProdServ;
                    if (string.IsNullOrEmpty(claveProdServConc))
                    {
                        respuesta.Add("ComprobanteConcepto.ClaveProdServ no contiene un valor del catálogo c_ClaveProdServ.");
                    }
                    else
                    {
                        var exist = Enum.IsDefined(typeof(cfdv33_full.c_ClaveProdServ), Funciones.ClaveProdServSingleton.Instance.ClaveProdServ);
                        if (!exist)
                        {
                            respuesta.Add("ComprobanteConcepto.ClaveProdServ no contiene un valor del catálogo c_ClaveProdServ.");
                        }
                    }

                    if (this.TipoDeComprobante == "I" || this.TipoDeComprobante == "E" || this.TipoDeComprobante == "N")
                    {
                        //Regla 33147: El valor valor del campo ValorUnitario debe ser mayor que cero (0) cuando el tipo de comprobante es Ingreso, Egreso o Nomina.
                        if (concepto.ValorUnitario <= 0)
                            respuesta.Add("ComprobanteConcepto.ValorUnitario no puede ser 0 o menor para comprobantes tipo I, E o N.");
                    }

                    if (concepto.Impuestos != null)
                    {
                        respuesta.AddRange(ObjectValidation.ValidateObject(concepto.Impuestos));

                        if (concepto.Impuestos.Traslados != null)
                            foreach (var traslado in concepto.Impuestos.Traslados)
                                respuesta.AddRange(ObjectValidation.ValidateObject(traslado));

                        if (concepto.Impuestos.Retenciones != null)
                            foreach (var retencion in concepto.Impuestos.Retenciones)
                                respuesta.AddRange(ObjectValidation.ValidateObject(retencion));
                    }

                    if (concepto.InformacionAduanera != null && concepto.InformacionAduanera.Count() > 0)
                    {
                        foreach (var aduana in concepto.InformacionAduanera)
                            respuesta.AddRange(ObjectValidation.ValidateObject(aduana));
                    }

                    if (concepto.CuentaPredial != null)
                        respuesta.AddRange(ObjectValidation.ValidateObject(concepto.CuentaPredial));
                }

            if (this.Impuestos != null)
            {
                respuesta.AddRange(ObjectValidation.ValidateObject(this.Impuestos));

                if (this.Impuestos.Retenciones != null && this.Impuestos.Retenciones.Count() > 0)
                    foreach (var retencion in this.Impuestos.Retenciones)
                        respuesta.AddRange(ObjectValidation.ValidateObject(retencion));

                if (this.Impuestos.Traslados != null && this.Impuestos.Traslados.Count() > 0)
                    foreach (var traslado in this.Impuestos.Traslados)
                        respuesta.AddRange(ObjectValidation.ValidateObject(traslado));
            }

            //validacion de complementos
            var creadorComplementos = new CreadorComplementos(this);
            foreach (var complemento in creadorComplementos.Complementos)
            {
                if (complemento is IComplemento)
                {
                    respuesta.AddRange(complemento.ValidarInformacion());
                }
            }

            return respuesta;
        }

        public void AjustarValidacionesSAT()
        {
            decimal subTotal = this.SubTotal;
            decimal totalDescuento = this.Descuento;
            decimal totalTraslado = 0m;
            decimal totalRetencion = 0m;
            decimal totalTrasImpLocal = 0m;
            decimal totalRetImpLocal = 0m;
            decimal totalFinal = this.Total;
            decimal conceptoTraslado = 0m;
            decimal conceptoRetecion = 0m;
            decimal conceptoDescuento = 0m;
            decimal conceptoSubtotal = 0m;
            decimal sumaTraslados = 0m;
            decimal sumaRetenciones = 0m;

            this.Fecha = new DateTime(this.Fecha.Year, this.Fecha.Month, this.Fecha.Day, this.Fecha.Hour, this.Fecha.Minute, this.Fecha.Second);

            //Ajuste regla CFDI33106: El valor de este campo SubTotal excede la cantidad de decimales que soporta la moneda.
            if (BitConverter.GetBytes(decimal.GetBits(this.SubTotal)[3])[2] > 2)
                this.SubTotal = Decimal.Round(this.SubTotal, 2);

            //Ajuste regla CFDI33111: El valor del campo Descuento excede la cantidad de decimales que soporta la moneda.
            if (BitConverter.GetBytes(decimal.GetBits(this.Descuento)[3])[2] > 2)
                this.Descuento = Decimal.Round(this.descuentoField, 2);

            //Ajuste regla CFDI33113: El campo TipoCambio no tiene el valor "1" y la moneda indicada es MXN.
            if (this.Moneda == "MXN")
                this.TipoCambio = 1m;

            //Ajuste regla CFDI33115: El campo TipoCambio no se debe registrar cuando el campo Moneda tiene el valor XXX.
            if (this.Moneda == "XXX")
            {
                this.TipoCambioSpecified = false;
                this.TipoCambio = 1m;
            }

            if (this.Receptor != null)
            {
                //Ajuste de regla CFDI33134 y CFDI33134: Si el RFC del receptor es de un RFC registrado en el SAT o un RFC genérico nacional, este atributo NO debe existir.
                //Y reglas CFDI33137: El valor del campo es un RFC inscrito no cancelado en el SAT o un RFC genérico nacional, y se registró el campo NumRegIdTrib.
                if (this.Receptor.Rfc != "XEXX010101000")
                {
                    this.Receptor.NumRegIdTrib = null;
                    this.Receptor.ResidenciaFiscal = null;
                    this.receptorField.ResidenciaFiscalSpecified = false;
                }
            }

            if (this.TipoDeComprobante == "T" || this.TipoDeComprobante == "P")
            {
                //Ajuste de regla CFDI33123: Se debe omitir el campo MetodoPago cuando el TipoDeComprobante es T o P.
                this.MetodoPago = null;
                this.MetodoPagoSpecified = false;

                //Ajuste de regla CFDI33179: Cuando el TipoDeComprobante sea T o P, el elemento Impuestos no debe existir.
                this.Impuestos = null;

                //Ajuste regla CFDI33103: Si existe el complemento para recepción de pagos el campo FormaPago no debe existir.
                this.FormaPago = null;
                this.FormaPagoSpecified = false;
            }

            var recalcularTraslados = false;

            if (this.Conceptos != null && this.Conceptos.Count() > 0)
            {
                foreach (var concepto in this.Conceptos)
                {
                    //Fuerza el maximo numero de decimales a 6 para los problemas de XML mal formado.
                    concepto.ValorUnitario = Math.Round(concepto.ValorUnitario, 6);
                    concepto.Cantidad = Math.Round(concepto.Cantidad, 6);

                    conceptoDescuento += concepto.Descuento;

                    //CFDI33148 - El valor del campo Importe debe tener hasta la cantidad de decimales que soporte la moneda. Se refiere a Importe en el Concepto.
                    //CRP120 - El valor del campo Importe debe ser cero "0".
                    if (concepto.Importe != 0m)
                        concepto.Importe = Convert.ToDecimal(Math.Round(concepto.Importe, 2).ToString("N2"));
                    else
                        concepto.Importe = Convert.ToDecimal(Math.Round(concepto.Importe, 0).ToString("N0"));

                    conceptoSubtotal += concepto.Importe;

                    //TODO: Falta prueba unitaria
                    //CFDI33154: El valor del campo Base que corresponde a Traslado debe ser mayor que cero.
                    if ((concepto.Importe - concepto.Descuento) <= 0)
                        concepto.Impuestos = null;

                    //TODO: Validar este error, parece que hay unas formulas para saber si esta entre los limites inferior y superior
                    //CFDI33151:
                    if (concepto.DescuentoSpecified)
                        concepto.Descuento = Convert.ToDecimal(Math.Round(concepto.Descuento, 2).ToString("N2"));

                    if (concepto.Impuestos != null)
                    {
                        if (concepto.Impuestos.Traslados != null)
                            foreach (var traslado in concepto.Impuestos.Traslados)
                            {
                                //CFDI33161 - El valor del campo Importe o que corresponde a Traslado no se encuentra entre el limite inferior y superior permitidoIn: Excepción al timbrar con Factura en Segundos901 - CFDI33161 - El valor del campo Importe o que corresponde a Traslado no se encuentra entre el limite inferior y superior permitido
                                traslado.TasaOCuota = Convert.ToDecimal(traslado.TasaOCuota.ToString("N6"));

                                //traslado.Importe = traslado.Importe.Normalize();
                                //Estas lienas no estaban haciendo nada porque de todos modos se recalculaba abajo.
                                //var trsltemp = Math.Round(traslado.Base * traslado.TasaOCuota, 4);
                                //var trsltempmin = trsltemp - .1m;
                                //var trsltempmax = trsltemp + .1m;
                                //if (traslado.Importe < trsltempmin || traslado.Importe > trsltempmax)
                                //{
                                //    traslado.Importe = trsltemp;
                                //    recalcularTraslados = true;
                                //}

                                //CFDI33195 - El campo Importe correspondiente a Traslado no es igual a la suma de los importes de los impuestos trasladados registrados en los conceptos donde el impuesto del concepto sea igual al campo impuesto de este elemento y la TasaOCuota del concepto sea igual al campo TasaOCuota de este elemento.
                                //traslado.Importe = Math.Round(traslado.Importe, 4);
                                //recalcularTraslados = true;

                                var trsltemp = Math.Round(traslado.Base * traslado.TasaOCuota, 4, MidpointRounding.AwayFromZero);
                                if (traslado.Importe != trsltemp)
                                {
                                    traslado.Importe = trsltemp;
                                    recalcularTraslados = true;
                                }

                                //CFDI33195 - El campo Importe correspondiente a Traslado no es igual a la suma de los importes de los impuestos trasladados registrados en los conceptos donde el impuesto del concepto sea igual al campo impuesto de este elemento y la TasaOCuota del concepto sea igual al campo TasaOCuota de este elemento.
                                traslado.Importe = Math.Round(traslado.Importe, 4, MidpointRounding.AwayFromZero);
                                recalcularTraslados = true;

                                conceptoTraslado += traslado.Importe;
                            }

                        if (concepto.Impuestos.Retenciones != null)
                            foreach (var retencion in concepto.Impuestos.Retenciones)
                            {
                                retencion.TasaOCuota = Convert.ToDecimal(retencion.TasaOCuota.ToString("N6"));
                                retencion.Importe = Math.Round(retencion.Importe, 4);
                                conceptoRetecion += retencion.Importe;
                            }
                    }

                    //TODO: Falta prueba unitaria.
                    //CFDI33110 - Cuando el TipoDeComprobante sea I, E o N y algún concepto incluya el atributo Descuento, debe existir este atributo y debe ser igual a la suma de los atributos Descuento registrados en los conceptos
                    if (!this.DescuentoSpecified)
                        concepto.DescuentoSpecified = false;
                }
            }
            if (this.Impuestos != null)
            {
                //Ajuste de traslados y retenciones a 2 decimales. Regla CFDI33180 y CFDI33182.
                this.Impuestos.TotalImpuestosTrasladados = Decimal.Round(this.Impuestos.TotalImpuestosTrasladados, 2, MidpointRounding.AwayFromZero);
                this.Impuestos.TotalImpuestosRetenidos = Decimal.Round(this.impuestosField.TotalImpuestosRetenidos, 2, MidpointRounding.AwayFromZero);

                if (this.Impuestos.Retenciones != null && this.Impuestos.Retenciones.Count() > 0)
                {

                    foreach (var retencion in this.Impuestos.Retenciones)
                    {
                        sumaRetenciones += retencion.Importe;
                    }
                    //TODO: Falta Unit Test
                    //Regla CFDI33181: El valor del campo TotalImpuestosRetenidos debe ser igual a la suma de los importes registrados en el elemento hijo Retencion
                    this.Impuestos.TotalImpuestosRetenidosSpecified = true;
                    if (this.Impuestos.TotalImpuestosRetenidos != sumaRetenciones)
                    {
                        sumaRetenciones = 0;
                        foreach (var retencion in this.Impuestos.Retenciones)
                        {
                            retencion.Importe = Decimal.Round(retencion.Importe, 2, MidpointRounding.AwayFromZero);
                            sumaRetenciones += retencion.Importe;
                        }
                        this.Impuestos.TotalImpuestosRetenidos = sumaRetenciones;
                    }
                }

                if (this.Impuestos.Traslados != null && this.Impuestos.Traslados.Count() > 0)
                {
                    foreach (var traslado in this.Impuestos.Traslados)
                    {
                        //CFDI33195 - El campo Importe correspondiente a Traslado no es igual a la suma de los importes de los impuestos trasladados registrados en los conceptos donde el impuesto del concepto sea igual al campo impuesto de este elemento y la TasaOCuota del concepto sea igual al campo TasaOCuota de este elemento.
                        traslado.TasaOCuota = Convert.ToDecimal(traslado.TasaOCuota.ToString("N6"));
                        if (recalcularTraslados)
                        {
                            var totalEsteTraslado = 0m;
                            foreach (var concepto in this.Conceptos)
                            {
                                if (concepto.Impuestos != null && concepto.Impuestos.Traslados != null)
                                {
                                    totalEsteTraslado += concepto.Impuestos.Traslados.Where(m => m.Impuesto == traslado.Impuesto && m.TasaOCuota == traslado.TasaOCuota).Sum(m => m.Importe);
                                }
                            }
                            traslado.Importe = Decimal.Round(totalEsteTraslado, 2, MidpointRounding.AwayFromZero);
                        }

                        sumaTraslados += traslado.Importe;
                    }

                    //TODO: Falta Unit Test
                    //Regla CFDI33183: El valor del campo TotalImpuestosTrasladados no es igual a la suma de los importes registrados en el elemento hijo Traslado.
                    this.Impuestos.TotalImpuestosTrasladadosSpecified = true;
                    if (this.Impuestos.TotalImpuestosTrasladados != sumaTraslados)
                    {
                        sumaTraslados = 0;
                        foreach (var traslado in this.Impuestos.Traslados)
                        {
                            traslado.Importe = Decimal.Round(traslado.Importe, 2);
                            sumaTraslados += traslado.Importe;
                        }
                        this.Impuestos.TotalImpuestosTrasladados = sumaTraslados;
                    }
                }

                totalTraslado += this.Impuestos.TotalImpuestosTrasladados;
                totalRetencion += this.impuestosField.TotalImpuestosRetenidos;

                if (this.Complemento != null && this.Complemento.Any.Count() > 0)
                    foreach (var complemento in this.Complemento.Any)
                    {
                        if (complemento.Prefix == "implocal")
                        {
                            var implocal = new AVSFacturador.Complementos.ImpuestosLocales();
                            implocal = implocal.Deserialize(complemento.OuterXml);
                            totalTrasImpLocal = implocal.TotaldeTraslados;
                            totalRetImpLocal = implocal.TotaldeRetenciones;
                        }
                    }
            }

            //Ajuste a regla CFDI33118: Que cuadré el total perfectamente.
            ValidarTotales(subTotal, totalDescuento, totalTraslado, totalRetencion, totalTrasImpLocal, totalRetImpLocal, totalFinal);


            //ajustar complementos
            var creadorComplementos = new CreadorComplementos(this);
            foreach (var complemento in creadorComplementos.Complementos)
            {
                if (complemento is IComplemento)
                {
                    complemento.AjustarValidacionesSAT();
                }
            }
        }

        private void ValidarTotales(decimal subTotal, decimal totalDescuento, decimal totalTraslado, decimal totalRetencion, decimal totalTrasImpLocal, decimal totalRetImpLocal, decimal totalFinal)
        {
            var totalCalc = subTotal - totalDescuento + totalTraslado + totalTrasImpLocal - totalRetencion - totalRetImpLocal;
            if (totalCalc != totalFinal)
                this.Total = totalCalc;
        }

        public void PonerTimbreFiscal(byte[] timbre)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(UTF8Encoding.UTF8.GetString(timbre));

            if (this.Complemento == null || this.Complemento.Any == null)
            {
                this.Complemento = new ComprobanteComplemento()
                {
                    Any = new System.Xml.XmlElement[0] { },
                };
            }

            var listaComplementos = this.Complemento.Any.ToList();
            listaComplementos.Add(xmldoc.DocumentElement);
            this.Complemento.Any = listaComplementos.ToArray();
        }

        public void PonerFechaLugarExpedicion(int DiferenciaHoria)
        {
            this.Fecha = this.Fecha.AddHours(DiferenciaHoria);
        }
    }

    public class Comprobante33_Validation
    {
        [MinLength(1, ErrorMessage = "Serie debe tener al menos 1 digito")]
        [MaxLength(25, ErrorMessage = "Serie tiene un maximo de 25 digitos")]
        [RegularExpression("[^|]{1,25}",
            ErrorMessage = "Serie tiene caracteres invalidos")]
        public string Serie;

        [MinLength(1, ErrorMessage = "Folio debe tener al menos 1 digito")]
        [MaxLength(40, ErrorMessage = "Folio tiene un maximo de 40 digitos")]
        [RegularExpression("[^|]{1,40}",
            ErrorMessage = "Folio tiene caracteres invalidos")]
        public string Folio;

        [Range(typeof(DateTime), "2017-01-01", "3000-01-01", ErrorMessage = "Fecha debe ser mayor a 01-01-2017")]
        public System.DateTime Fecha;

        //[Required(ErrorMessage = "FormaPago es Obligatorio")]
        //[ExistsInEnum(typeof(cfdv33_full.c_FormaPago))]
        public string FormaPago;

        [MinLength(1, ErrorMessage = "CondicionesDePago debe tener al menos 1 digito")]
        [MaxLength(1000, ErrorMessage = "CondicionesDePago tiene un maximo de 1000 digitos")]
        [RegularExpression("[^|]{1,1000}",
            ErrorMessage = "CondicionesDePago tiene caracteres invalidos")]
        public string CondicionesDePago;

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "SubTotal no puede ser negativo")]
        public decimal SubTotal;

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Descuento no puede ser negativo")]
        public decimal Descuento;

        [Required(ErrorMessage = "Moneda es Obligatorio")]
        [ExistsInEnum(typeof(cfdv33_full.c_Moneda))]
        public string Moneda;

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Total no puede ser negativo")]
        public decimal Total;

        [Required(ErrorMessage = "TipoDeComprobante es Obligatorio")]
        [ExistsInEnum(typeof(cfdv33_full.c_TipoDeComprobante))]
        public string TipoDeComprobante;

        [ExistsInEnum(typeof(cfdv33_full.c_MetodoPago))]
        public string MetodoPago;

        [Required(ErrorMessage = "LugarExpedicion es Obligatorio")]
        [RegularExpression("^[0-9]{5}$",
            ErrorMessage = "LugarExpedicion tiene caracteres invalidos")]
        //[ExistsInEnum(typeof(cfdv33_full.c_CodigoPostal))]
        [MinLength(5, ErrorMessage = "LugarExpedicion debe tener 5 digitos")]
        [MaxLength(5, ErrorMessage = "LugarExpedicion debe tener 5 digitos")]
        public string LugarExpedicion;

        [MinLength(5, ErrorMessage = "Confirmacion debe tener 5 digitos")]
        [MaxLength(5, ErrorMessage = "Confirmacion debe tener 5 digitos")]
        [RegularExpression("[0-9a-zA-Z]{5}", ErrorMessage = "Confirmacion tiene caracteres invalidos")]
        public string Confirmacion;

        [Required(ErrorMessage = "El nodo Emisor es Obligatorio")]
        public ComprobanteEmisor Emisor;

        [Required(ErrorMessage = "El nodo Receptor es Obligatorio")]
        public ComprobanteReceptor Receptor;

        [Required(ErrorMessage = "El nodo Conceptos es Obligatorio")]
        [MinLength(1, ErrorMessage = "El nodo Conceptos debe tener al menos 1 ComprobanteConcepto")]
        public ComprobanteConcepto[] Conceptos;
    }
}
