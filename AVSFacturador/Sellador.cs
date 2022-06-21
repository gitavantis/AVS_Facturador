using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AVSFacturador
{
    /// <summary>
    /// Esta clase se encarga del proceso de firmar el archivo.
    /// </summary>
    public static class Sellador
    {
        public static string GenerarSelloPorVersion(string cadena, string version, byte[] key, string keyPassword)
        {
            switch (version)
            {
                case "3.2":
                    return GenerarSelloSHA1(cadena, key, keyPassword);
                case "3.3":
                    return GenerarSelloSHA256(cadena, key, keyPassword);
                case "4.0":
                    return GenerarSelloSHA256(cadena, key, keyPassword);
                default:
                    throw new InvalidDataException("La versión es invalida. Los valores aceptados son unicamente 3.2, 3.3, 4.0");
            }
        }

        /// <summary>
        /// Genera el sello de una cadena utilizando digestion SHA1.
        /// </summary>
        /// <param name="cadena">Cadena a Sellar</param>
        /// <param name="key">Llave Privada</param>
        /// <param name="keyPassword">Contraseña de Llave Privada</param>
        /// <returns>Sello de la Cadena en Base64</returns>
        public static string GenerarSelloSHA1(string cadena, byte[] key, string keyPassword)
        {
            try
            {
                byte[] hash;
                byte[] signedHash;
                byte[] raw = key;
                RSACryptoServiceProvider rsa = opensslkey.DecodeEncryptedPrivateKeyInfo(raw, keyPassword);
                if (rsa == null)
                {
                    throw new Exception("No se encontro un certificado Valido.");
                }

                SHA1 sha1 = new SHA1CryptoServiceProvider();
                hash = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(cadena));
                signedHash = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));

                var sello = Convert.ToBase64String(signedHash);
                return sello;
            }
            catch (Exception e)
            {
                throw new Exception("Error al generar el Sello, ver error interno", e);
            }
        }

        /// <summary>
        /// Genera el sello de una cadena utilizando digestion SHA256.
        /// </summary>
        /// <param name="cadena">Cadena a Sellar</param>
        /// <param name="key">Llave Privada</param>
        /// <param name="keyPassword">Contraseña de Llave Privada</param>
        /// <returns>Sello de la Cadena en Base64</returns>
        public static string GenerarSelloSHA256(string cadena, byte[] key, string keyPassword)
        {
            try
            {
                byte[] hash;
                byte[] signedHash;
                byte[] raw = key;
                RSACryptoServiceProvider rsa = opensslkey.DecodeEncryptedPrivateKeyInfo(raw, keyPassword);
                if (rsa == null)
                {
                    throw new Exception("No se encontro un certificado Valido.");
                }

                SHA256 sha256 = new SHA256CryptoServiceProvider();
                hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(cadena));
                signedHash = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));

#if DEBUG
                var hasStr = String.Empty;
                foreach (byte x in hash)
                {
                    hasStr += String.Format("{0:x2}", x);
                }
#endif

                var sello = Convert.ToBase64String(signedHash);
                return sello;
            }
            catch (Exception e)
            {
                throw new Exception("Error al generar el Sello, ver error interno", e);
            }
        }

        /// <summary>
        /// Verifica el Sello de una Cadena con digestión SHA1.
        /// </summary>
        /// <param name="cadenaOriginal">La cadena.</param>
        /// <param name="sello">El sello en Base64.</param>
        /// <param name="certificadopublico">El certificado publico.</param>
        /// <returns>True si es correcta, False si no coincide.</returns>
        public static bool VerifySHA1Sign(string cadenaOriginal, string sello, byte[] certificadopublico)
        {
            try
            {
                X509Certificate2 x509 = new X509Certificate2();
                x509.Import(certificadopublico);

                bool regreso = false;
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509.PublicKey.Key;

                SHA1 sha1 = new SHA1CryptoServiceProvider();
                regreso = rsa.VerifyHash(sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(cadenaOriginal)),
                                                CryptoConfig.MapNameToOID("SHA1"),
                                                Convert.FromBase64String(sello));

                return regreso;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Verifica el Sello de una Cadena con digestión SHA1.
        /// </summary>
        /// <param name="cadenaOriginal">La cadena.</param>
        /// <param name="sello">El sello en Base64.</param>
        /// <param name="certificadopublico">El certificado publico.</param>
        /// <returns>True si es correcta, False si no coincide.</returns>
        public static bool VerifySHA1Sign(string cadenaOriginal, string sello, MemoryStream certificadopublico)
        {
            try
            {
                certificadopublico.Seek(0, SeekOrigin.Begin);
                byte[] bytesCert = new byte[certificadopublico.Length];
                certificadopublico.Read(bytesCert, 0, bytesCert.Length);
                certificadopublico.Close();

                return VerifySHA1Sign(cadenaOriginal, sello, bytesCert);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Verifica el Sello de una Cadena con digestión SHA256.
        /// </summary>
        /// <param name="cadenaOriginal">La cadena.</param>
        /// <param name="sello">El sello en Base64.</param>
        /// <param name="certificadopublico">El certificado publico.</param>
        /// <returns>True si es correcta, False si no coincide.</returns>
        public static bool VerifySHA256Sign(string cadenaOriginal, string sello, byte[] certificadopublico)
        {
            try
            {
                X509Certificate2 x509 = new X509Certificate2();
                x509.Import(certificadopublico);

                bool regreso = false;
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509.PublicKey.Key;
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(cadenaOriginal));
#if DEBUG
                var hasStr = String.Empty;
                foreach (byte x in hash)
                {
                    hasStr += String.Format("{0:x2}", x);
                }
#endif
                regreso = rsa.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), Convert.FromBase64String(sello));

                return regreso;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Verifica el Sello de una Cadena con digestión SHA256.
        /// </summary>
        /// <param name="cadenaOriginal">La cadena.</param>
        /// <param name="sello">El sello en Base64.</param>
        /// <param name="certificadopublico">El certificado publico.</param>
        /// <returns>True si es correcta, False si no coincide.</returns>
        public static bool VerifySHA256Sign(string cadenaOriginal, string sello, MemoryStream certificadopublico)
        {
            try
            {
                certificadopublico.Seek(0, SeekOrigin.Begin);
                byte[] bytesCert = new byte[certificadopublico.Length];
                certificadopublico.Read(bytesCert, 0, bytesCert.Length);
                certificadopublico.Close();

                return VerifySHA256Sign(cadenaOriginal, sello, bytesCert);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Genera cadena utilizando digestion SHA1.
        /// </summary>
        /// <param name="cadena">Cadena a Sellar</param>
        /// <returns>Sha1 de la Cadena en Base64</returns>
        public static string GenerarSHA1(string cadena)
        {
            try
            {
                byte[] hash;
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                hash = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(cadena));
                var sello = Convert.ToBase64String(hash);
                return sello;
            }
            catch (Exception e)
            {
                throw new Exception("Error al generar el SHA1, ver error interno", e);
            }
        }

    }
}