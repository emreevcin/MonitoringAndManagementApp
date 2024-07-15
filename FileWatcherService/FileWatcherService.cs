using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using Serilog;
using Serilog.Core;

namespace FileWatcherService
{
    public partial class FileWatcherService : ServiceBase
    {
        private readonly ILogger _logCatcher;
        private readonly string _path;
        public FileSystemWatcher _watcher;

        public FileWatcherService(ILogger logCatcher, string path)
        {
            InitializeComponent();
            _logCatcher = logCatcher;
            _path = path;
        }

        protected override void OnStart(string[] args)
        {
            _logCatcher.Information("Service started.");

            if (!Directory.Exists(_path))
            {
                _logCatcher.Error($"Path {_path} does not exist.");
                return;
            }

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

        public void OnFileSystemEvent(object sender, FileSystemEventArgs e)
        {
            _logCatcher.Information($"{e.ChangeType}: {e.Name}");
        }

        public void OnRenamed(object sender, RenamedEventArgs e)
        {
            _logCatcher.Information($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }

        public void StartService(string[] args)
        {
            OnStart(args);
        }

        public void StopService()
        {
            OnStop();
        }
    }
}
