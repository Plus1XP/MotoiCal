using System;

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
