using MotoiCal.Models.FileManagement;

using System;
using System.IO;

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
            this.encryptionManager = new EncryptionManager();
            this.plainTextFilePath = $"{this.projectDirectory}\\motoical\\apikey.txt";
            this.encryptedFilePath = $"{this.projectDirectory}\\motoical\\apikey";
    }

        public string CreateApiKeyFile()
        {
            string apikey = this.encryptionManager.ReadFromFile(this.plainTextFilePath);
            this.encryptionManager.EncryptFile(this.encryptionManager.EncryptionKey, apikey, this.encryptedFilePath);
            return "Key File Created";
        }
    }
}
