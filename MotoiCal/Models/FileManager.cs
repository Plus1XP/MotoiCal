using System.IO;
using System.Text;

namespace MotoiCal.Models
{
    public class FileManager
    {
        public FileManager()
        {
        }

        public bool IsFileCreated(string filePath)
        {
            return File.Exists(filePath);
        }

        public void CreateFile(string filePath)
        {
            File.Create(filePath).Close();
        }

        public void ClearFile(string filePath)
        {
            File.WriteAllText(filePath, string.Empty);
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public void SaveToFile(string filePath, string Value)
        {
            StreamWriter writeFile = new StreamWriter(filePath);
            writeFile.Write(Value);
            writeFile.Close();
        }

        public string ReadFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
    }
}