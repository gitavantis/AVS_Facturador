using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


namespace AVSFacturador
{
    class DatosCertificado
    {
        private byte[] certFile;
        private X509Certificate cer;
        public DateTime ExpirationDate { get; set; }
        public string SerialNumber { get; set; }

        public string IssuerName { get; set; }


        public string CertBase64
        {
            get
            {
                return Convert.ToBase64String(certFile);
            }
        }

        public DatosCertificado(byte[] certificate)
        {
            certFile = certificate;
            cer = new X509Certificate(certificate);
            SetExpirationDate();
            SetSerialNumber();
            SetIssuerName();
        }

        private void SetExpirationDate()
        {
            string sFecha = cer.GetExpirationDateString();
            ExpirationDate = Convert.ToDateTime(sFecha);
        }

        private void SetSerialNumber()
        {
            string sSerial = cer.GetSerialNumberString();
            StringBuilder result = new StringBuilder();
            string nibble = "";
            int i = 0;
            foreach (char x in sSerial)
            {
                nibble += x;
                i++;
                if ((i % 4) == 0)
                {
                    result.Append(nibble[1]);
                    result.Append(nibble[3]);
                    nibble = "";
                }
            }
            SerialNumber = result.ToString();
        }

        private void SetIssuerName()
        {
            this.IssuerName = cer.Issuer;
        }

    }
}
