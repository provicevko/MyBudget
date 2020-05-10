using System;
using System.IO;

namespace PConsole
{
    public static class ErrorHandler
    {
        public static void Logs(string message)
        {
            string docPath = "logs/";
            
            using (StreamWriter outputFile = new StreamWriter("Log.txt", true))
            {
                outputFile.WriteLine(message);
            }
        }
    }
}