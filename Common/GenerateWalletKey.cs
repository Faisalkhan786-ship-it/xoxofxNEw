using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using QRCoder;
using System;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using ViewModel;
using System;
using System.IO;

using System;

using System.IO;

namespace Common
{
    public static class GenerateWalletKey
    {
        public static string GenerateQRCodeBase64(string inputText)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(inputText, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(20); // 20 = pixels per module
            return "data:image/png;base64," + Convert.ToBase64String(qrCodeBytes);
        }

        //public static AddAddressResponseModelViewModel CreateAccount()
        //{
        //    var model = new AddAddressResponseModelViewModel();

        //    try
        //    {
        //        var ecKey = EthECKey.GenerateKey();
        //        string privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();

        //        var account = new Account(privateKey);
        //        model.WalletAddress = account.Address;
        //        model.PrivateKey = account.PrivateKey;
        //    }
        //    catch
        //    {
        //        model.WalletAddress = null;
        //        model.PrivateKey = null;
        //    }

        //    return model;
        //}

        public static string Encrypt(string encryptString)
        {
            const string key = "EthereumHGW^&@^&@^#@%%%^$^%$$#$#$^%*&JKJKJKJHJHG%^^%$%^$%^$%^$%%%%^$ETH";

            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using var aes = Aes.Create();

            var pdb = new Rfc2898DeriveBytes(key, new byte[]
            {
                0x49, 0x76, 0x61, 0x6e, 0x20,
                0x4d, 0x65, 0x64, 0x76, 0x65,
                0x64, 0x65, 0x76
            });

            aes.Key = pdb.GetBytes(32);
            aes.IV = pdb.GetBytes(16);

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            const string key = "EthereumHGW^&@^&@^#@%%%^$^%$$#$#$^%*&JKJKJKJHJHG%^^%$%^$%^$%^$%%%%^$ETH";

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();

            var pdb = new Rfc2898DeriveBytes(key, new byte[]
            {
                0x49, 0x76, 0x61, 0x6e, 0x20,
                0x4d, 0x65, 0x64, 0x76, 0x65,
                0x64, 0x65, 0x76
            });

            aes.Key = pdb.GetBytes(32);
            aes.IV = pdb.GetBytes(16);

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();

            return Encoding.Unicode.GetString(ms.ToArray());
        }

    }
}

