using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Info.Utils
{
    internal class Security
    {
        private readonly Loggers _logger = new();
        private string _methodName = string.Empty;

        public string EncryptPass(string? message, string phrase)
        {
            var passPhrase = phrase.ToLower();

            byte[] results;
            var utf8 = new System.Text.UTF8Encoding();

            //step 1 we Hash the passPhrase using MD5
            //we use the MD5 generator as the result in 128 bit byte array
            //which is a valid length for the TripleDES encoder we use below
            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passPhrase));

            //step 2 create a new TripleDESCryptoServiceProvider object
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                //step 3 set up the encoder
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            //step 4 convert the input string to a byte[]
            var dataToEncrypt = utf8.GetBytes(message);

            //step 5 attempt to encrypt the the string
            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            catch (Exception)
            {
                results = new byte[1];
            }
            finally
            {
                //Clear the TripleDes and Hash provider services of any sensitive data
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            return Convert.ToBase64String(results);
        }

        public string DecryptPass(string message, string phrase)
        {
            var passPhrase = phrase.ToLower();

            byte[] results;
            var utf8 = new System.Text.UTF8Encoding();

            //step 1 we Hash the passPhrase using MD5
            //we use the MD5 generator as the result in 128 bit byte array
            //which is a valid lenght for the TripleDES encoder we use below
            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passPhrase));

            //step 2 create a new TripleDESCryptoServiceProvider object
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            //step 4 convert the input string to a byte[]
            var dataToDecrypt = Convert.FromBase64String(message);

            //step 5 attempt to encrypt the the string
            try
            {
                var decryptor = tdesAlgorithm.CreateDecryptor();

                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            catch (Exception)
            {
                results = new byte[1];
            }
            finally
            {
                //Clear the TripleDes and Hash provider services of any sensitive data
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            //step 6 return the decrypted string in UTF8 format
            return utf8.GetString(results);
        }

        public async Task<string> ConfigureConn(string dbConn, string encrSv, string encrDb, string encrUi, string encrPw)
        {
            var connectionString = string.Empty;

            try
            {
                connectionString = dbConn;

                if (connectionString.Trim().Length == 0 || string.IsNullOrEmpty(connectionString))
                {
                    _logger.CreateLogs(_methodName + "-> Connection String Missing");
                    return connectionString;
                }

                var connSrv = encrSv;
                var connDb = encrDb;
                var connUi = encrUi;
                var connPass = encrPw;

                connSrv = connSrv == "" ? "" : DecryptPass(connSrv, "Fintech");
                connDb = connDb == "" ? "" : DecryptPass(connDb, "Fintech");
                connUi = connUi == "" ? "" : DecryptPass(encrUi, "Fintech");
                connPass = connPass == "" ? "" : DecryptPass(encrPw, "Fintech");

                //------- Server Name
                connectionString = connectionString.Contains("[{SV}]") && connSrv != ""
                    ? connectionString.Replace("[{SV}]", connSrv)
                    : "";

                //------- Database Name
                connectionString = connectionString.Contains("[{DB}]") && connDb != ""
                    ? connectionString.Replace("[{DB}]", connDb)
                    : "";

                //------- User Name
                connectionString = connectionString.Contains("[{UI}]") && connUi != ""
                    ? connectionString.Replace("[{UI}]", connUi)
                    : "";

                //------- User password
                connectionString = connectionString.Contains("[{PW}]") && connPass != ""
                    ? connectionString.Replace("[{PW}]", connPass)
                    : "";

                return connectionString;
            }
            catch (Exception ex)
            {
                _methodName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name;
                _logger.CreateLogs(_methodName + "->" + ex.Message);
                return connectionString;
            }
        }
    }
}
