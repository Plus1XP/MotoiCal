using MotoiCal.Models.FileManagement;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKeyCreator
{
    class KeyCreator
    {
        private EncryptionManager encryptionManager;

        // This will get the current PROJECT directory
        private string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        private string plainTextFilePath;
        private string encryptedFilePath;
        public KeyCreator()
        {
            encryptionManager = new EncryptionManager();
            plainTextFilePath = $"{projectDirectory}\\motoical\\apikey.txt";
            encryptedFilePath = $"{projectDirectory}\\motoical\\apikey";
    }

        public string CreateApiKeyFile()
        {
            string apikey = encryptionManager.ReadFromFile(plainTextFilePath);
            encryptionManager.EncryptFile(encryptionManager.Password, apikey, encryptedFilePath);
            return "Key File Created";
        }
    }
}
