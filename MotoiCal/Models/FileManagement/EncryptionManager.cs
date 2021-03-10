﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MotoiCal.Models.FileManagement
{
    public class EncryptionManager : FileManager
    {
        public string EncryptionKey { get; }

        private byte[] EncryptionIV;

        public EncryptionManager()
        {
            // Create Secret Key
            this.EncryptionKey = "EnterEncryptionPasswordHere";

            // Create Secret IV
            this.EncryptionIV = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

        }

        public string EncryptString(string password, string plainString)
        {
            Aes encryptor = this.ConfigureEncryptor(password);
            byte[] plainBytes = this.ConvertPlainStringToBytes(plainString);
            byte[] cipherBytes = this.Encrypt(encryptor, plainBytes);
            return this.ConvertCipherBytesToString(cipherBytes);
        }

        public string DecryptString(string password, string cipherString)
        {
            Aes encryptor = this.ConfigureEncryptor(password);
            byte[] cipherBytes = this.ConvertCipherStringToBytes(cipherString);
            byte[] plainBytes = this.Decrypt(encryptor, cipherBytes);
            return this.ConvertPlainBytesToString(plainBytes);
        }

        public void EncryptFile(string password, string plainString, string filePath)
        {
            Aes encryptor = this.ConfigureEncryptor(password);
            byte[] plainBytes = this.ConvertPlainStringToBytes(plainString);
            byte[] cipherBytes = this.Encrypt(encryptor, plainBytes);
            string cipherString = this.ConvertCipherBytesToString(cipherBytes);
            this.SaveToFile(filePath, cipherString);
        }

        public void DecryptFile(string password, string filePath)
        {
            Aes encryptor = this.ConfigureEncryptor(password);
            string cipherText = this.ReadFromFile(filePath);
            byte[] cipherBytes = this.ConvertCipherStringToBytes(cipherText);
            byte[] plainBytes = this.Decrypt(encryptor, cipherBytes);
            string plainString = this.ConvertPlainBytesToString(plainBytes);
            this.SaveToFile(filePath, plainString);
        }

        public string GetDecryptedFileContents(string password, string filePath)
        {
            Aes encryptor = this.ConfigureEncryptor(password);
            string cipherText = this.ReadFromFile(filePath);
            byte[] raw = this.ConvertCipherStringToBytes(cipherText);
            byte[] final = this.Decrypt(encryptor, raw);
            return this.ConvertPlainBytesToString(final);
        }

        private Aes ConfigureEncryptor(string password)
        {
            // Create sha256 hash
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(password));

            // Create secret IV
            //byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            //encryptor.KeySize = 256;
            //encryptor.BlockSize = 128;
            //encryptor.Padding = PaddingMode.Zeros;

            // Set key and IV
            encryptor.Key = key;
            encryptor.IV = this.EncryptionIV;

            return encryptor;
        }

        private byte[] Encrypt(Aes encryptor, byte[] plainBytes)
        {
            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            return cipherBytes;
        }

        public byte[] Decrypt(Aes encryptor, byte[] cipherBytes)
        {
            // ConvertCipherStringToBytes, could potentially return null
            // depending on if there is a base-64 string exception.
            if (cipherBytes == null)
            {
                return new byte[0];
            }

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plainbytes
            byte[] plainBytes;

            try
            {
                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                plainBytes = memoryStream.ToArray();
            }
            finally
            {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            // Return the decrypted data as a string
            return plainBytes;
        }

        // Encryption
        private byte[] ConvertPlainStringToBytes(string plainText)
        {
            // Convert the plainText string into a byte array
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
            return plainBytes;
        }

        // Encryption
        private string ConvertCipherBytesToString(byte[] cipherBytes)
        {
            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
            return cipherText;
        }

        // Decryption
        private byte[] ConvertCipherStringToBytes(string cipherText)
        {
            try
            {
                // Convert the ciphertext string into a byte array
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                return cipherBytes;
            }
            catch (FormatException)
            {
                // Catch Invalid character in a base-64 string exception,
                // if cipherText is not a valid base 64 string
            }

            return null;
        }

            // Decryption
            private string ConvertPlainBytesToString(byte[] plainBytes)
            {
                // Convert the decrypted byte array to string
                string plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
                return plainText;
            }
        }
    }
