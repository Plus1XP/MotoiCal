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
            return File.Exists(filePath) ? true : false;
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
            StringBuilder text = new StringBuilder();
            foreach (string line in File.ReadLines(filePath))
            {
                text.AppendLine(line);
            }
            return text.ToString();
        }
    }
}