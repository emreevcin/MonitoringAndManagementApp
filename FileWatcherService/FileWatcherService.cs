using System;
using System.IO;
using System.ServiceProcess;
using Serilog;
    using System.Diagnostics;

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
            _watcher = new FileSystemWatcher();
            _watcher.Path = _path;
            _watcher.IncludeSubdirectories = true; 
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _watcher.Filter = "*.*";

            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.Created += new FileSystemEventHandler(OnCreated);
            _watcher.Deleted += new FileSystemEventHandler(OnDeleted);
            _watcher.Renamed += new RenamedEventHandler(OnRenamed);

            _watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            _logCatcher.Information($"File: {e.FullPath} {e.ChangeType}");
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            _logCatcher.Information($"Created: File {e.FullPath} {e.ChangeType}");
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            _logCatcher.Information($"Deleted: File {e.FullPath} {e.ChangeType}");
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            _logCatcher.Information($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }
    }
}
