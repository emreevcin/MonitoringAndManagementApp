using System.IO;
using System.ServiceProcess;
using Serilog;

namespace FileWatcherService
{
    public partial class FileWatcherService : ServiceBase
    {
        private readonly ILogger _logCatcher;
        private readonly string _path;
        private FileSystemWatcher _watcher;

        public FileWatcherService(ILogger logCatcher, string path)
        {
            InitializeComponent();
            _logCatcher = logCatcher;
            _path = path;
        }

        protected override void OnStart(string[] args)
        {
            _logCatcher.Information("Service started.");

            ConfigureFileSystemWatcher();
        }

        protected override void OnStop()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
            }

            _logCatcher.Information("Service stopped.");
        }

        private void ConfigureFileSystemWatcher()
        {
            _watcher = new FileSystemWatcher
            {
                Path = _path,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*"
            };

            _watcher.Changed += OnFileSystemEvent;
            _watcher.Created += OnFileSystemEvent;
            _watcher.Deleted += OnFileSystemEvent;
            _watcher.Renamed += OnRenamed;

            _watcher.EnableRaisingEvents = true;
        }

        private void OnFileSystemEvent(object sender, FileSystemEventArgs e)
        {
            _logCatcher.Information($"{e.ChangeType}: {e.Name}");
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            _logCatcher.Information($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }
    }
}
