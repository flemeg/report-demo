using System;
using System.IO;

namespace InputOutput.Core.Infra
{
    public class FileWriter
    {
        public static void WriteFile(string path, string report, string content)
        {
            TryCreateDir(Path.GetDirectoryName(path));         

            using (FileStream file = new FileStream(path, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(file))
            {            
                sw.WriteLine($"{report}: {content}");
            }
        }

        private static void TryCreateDir(string path)
        {
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
