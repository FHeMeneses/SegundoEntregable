using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace appEntergable2
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hidenContador.Value = "0";
                hidenFechaBloqueo.Value = "";
            }

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                string tdcInput = txtTarjeta.Text;
                string tdcMasked = maskCreditCard(tdcInput);
                var sha256 = convertToSha256Hsah(tdcInput);
                string msgHASH = "TDC Tecleado: " + tdcMasked + Environment.NewLine + " SHA-256 HASH: " + sha256;


                byte[] key = new byte[32]; // 256-bit key
                byte[] iv = new byte[16]; // 128-bit IV
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(key);
                    rng.GetBytes(iv);
                }
                // Encrypt
                byte[] ciphertext = Encrypt(tdcInput, key, iv);
                string encryptedText = Convert.ToBase64String(ciphertext);
                string msgEncrypted = "TDC encriptado: " + encryptedText;
                // Decrypt
                byte[] bytes = Convert.FromBase64String(encryptedText);
                string decryptedText = Decrypt(bytes, key, iv);
                string msgDecrypted = "TCD Desencriptado: " + maskCreditCard(decryptedText);

                txtResult.Text= msgHASH + Environment.NewLine + msgEncrypted + Environment.NewLine + msgDecrypted;

                mostrarMensaje("Este es la tarjeta Tecleada: " + tdcMasked);

            }
            catch (Exception ex)
            {
                mostrarMensaje("AppEntregable2|Default|btnAceptar" + ex.Message);
            }
        }

        private string maskCreditCard(string tdc)
        {
            string firstDigits = tdc.Substring(0,4);
            string lastDigits = tdc.Substring(tdc.Length -4, 4);
            string maskecNumber = firstDigits + new string('*', tdc.Length-8) + lastDigits;
            return maskecNumber;
        }

        private string convertToSha256Hsah(string value)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(value);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public byte[] Encrypt(string plaintext, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }
                return encryptedBytes;
            }
        }

        public string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes;
                using (var msDecrypt = new System.IO.MemoryStream(ciphertext))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var msPlain = new System.IO.MemoryStream())
                        {
                            csDecrypt.CopyTo(msPlain);
                            decryptedBytes = msPlain.ToArray();
                        }
                    }
                }
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }


        protected virtual void mostrarMensaje(string mensaje)
        {
            string mensajeFormateado = mensaje.Replace("\r", "").Replace("\n", "").Replace("'", "");
            string script = "<script language='javascript'>alert('" + mensajeFormateado + "');</script>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "CERRAR", script, false);
        }
    }
}