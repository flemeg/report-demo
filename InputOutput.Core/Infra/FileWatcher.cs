using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace InputOutput.Core.Infra
{
    public class FileWatcher
    {
        FileSystemWatcher watcher;

        private const int timeout = 10000;

        public event EventHandler FileCreatedReached;

        public string FileContents { get; private set; }

        public FileWatcher(string path, string filter)
        {
            TryCreateAndOpenDir(path);
            watcher = new FileSystemWatcher(path);
            watcher.Filter = filter;
        }

        public void Start()
        {

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Created += OnCreated;
            watcher.Error += OnError;
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;

            WaitForChangedResult result;
            do
            {
                result = watcher.WaitForChanged(WatcherChangeTypes.All, timeout);
            } while (!result.TimedOut);
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
            RaiseFileAvaibleEvent(e.FullPath);
        }

        private void RaiseFileAvaibleEvent(string path)
        {
            try
            {
                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var sr = new StreamReader(file))
                    {
                        FileContents = sr.ReadToEnd();
                        sr.Dispose();
                    }
                    file.Dispose();
                }

                var handler = FileCreatedReached;
                EventArgs e = new EventArgs();
                handler?.Invoke(this, e);
            }
            catch (IOException)
            {
                // Some anti-virus may detect file creation without a 
                // signed .exe as a virus and block file read/write 
                // so let try again do this quietly
                Thread.Sleep(5000);
                RaiseFileAvaibleEvent(path);
            }
            catch (Exception)
            {
                Console.WriteLine("File corruped or invalid");
            }

        }

        private void TryCreateAndOpenDir(string path)
        {
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                Process.Start("explorer.exe", @path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
