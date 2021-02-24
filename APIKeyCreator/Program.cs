using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKeyCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyCreator keyCreator = new KeyCreator();
            string result = keyCreator.CreateApiKeyFile();
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
