using System;
using System.IO;

namespace PConsole
{
    public static class ErrorHandler
    {
        public static void Logs(Exception e)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter("Log.txt", true))
                {
                    outputFile.WriteLine("<------------------------------------------------->");
                    outputFile.WriteLine($"Message: {e.Message}");
                    outputFile.WriteLine($"Source: {e.Source}");
                    outputFile.WriteLine($"StackTrace: {e.StackTrace}");
                    outputFile.WriteLine($"TargetSite: {e.TargetSite}");
                    outputFile.WriteLine($"InnerException: {e.InnerException}");
                    outputFile.WriteLine($"Data: {DateTime.Now}");
                    outputFile.WriteLine("<------------------------------------------------->");
                }
            }
            catch (Exception)
            {
                throw new FileLoadException("Logging info error");
            }
        }
    }
}