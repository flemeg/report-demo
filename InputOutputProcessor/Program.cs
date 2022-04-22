using InputOutput.Core.Infra;
using InputOutput.Core.Report;
using InputOutput.Core.Repository;
using System;
using System.Linq;
using System.Threading;

namespace InputOutputProcessor
{
    class Program
    {

        static ManualResetEvent _quitEvent = new ManualResetEvent(false);
        static string _homepath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        static void Main(string[] args)
        {
            Console.WriteLine("Started");
            Console.WriteLine("App will stop if no files added within 10min");
            Console.WriteLine("Ctrl+C to force shutdown");

            Console.CancelKeyPress += (sender, eArgs) =>
            {
                _quitEvent.Set();
                eArgs.Cancel = true;
            };

            // File watcher
            string pathToWatch = @$"{_homepath}\data\in";
            string filter = "*.dat";
            var watcher = new FileWatcher(pathToWatch, filter);
            watcher.FileCreatedReached += Watcher_FileCreatedReached;
            watcher.Start();

            _quitEvent.WaitOne();
        }

        private static void Watcher_FileCreatedReached(object sender, EventArgs e)
        {
            Console.WriteLine($"New files found at {DateTime.Now}");

            // Get sales info
            var content = sender as FileWatcher;
            var models = FileConverter.RawContentAsSaleModel(content.FileContents);

            // Organize as concret Sale Model
            var repository = new SalesRepository(models);

            // Generate Reports
            var builder = new ReportBuilder()
                .AddSales(repository.GetSales())
                .AddCustomers(repository.GetCustomers())
                .AddSalesman(repository.GetSalesman());

            var reports = builder.BuildReport();

            // Write report to the file
            var fileId = DateTime.Now.Millisecond;
            foreach (var r in reports)
            {
                FileWriter.WriteFile(@$"{_homepath}\data\out\report{fileId}.done.dat", r.Title, r.Content);
            }
            if (reports.Any())
                Console.WriteLine($"New report created at {DateTime.Now}");
        }
    }
}
