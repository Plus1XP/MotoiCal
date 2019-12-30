using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MotoiCal.Models
{
    public class FileManager
    {
        public FileManager()
        {

        }

        public bool CheckFileExists(string filePath)
        {
            return File.Exists(filePath) ? true : false;
        }
    }
}
