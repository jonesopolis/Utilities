using System;
using System.IO;
using System.Timers;
using Newtonsoft.Json;

namespace Jones.Utilities
{
    public sealed class LiveConfig<T>
    {
        public event Action Changed;
        public event Action Unavailable;

        private readonly string _configPath;
        private readonly Timer _timer;
        private readonly FileSystemWatcher _watcher;

        private bool _lastUpdateSuccessful;
        private bool _recentlyUpdated;

        public LiveConfig(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!Path.IsPathRooted(filePath))
            {
                filePath = Path.Combine(Environment.CurrentDirectory, filePath);
            }

            _configPath = filePath;

            if (!File.Exists(_configPath))
            {
                throw new FileNotFoundException(_configPath);
            }

            _timer = new Timer();
            _timer.Elapsed += (s, e) =>
            {
                _recentlyUpdated = false;
                _timer.Stop();
            };
            _timer.Interval = 500;

            var dir = Path.GetDirectoryName(_configPath);

            _watcher = new FileSystemWatcher(dir);
            _watcher.NotifyFilter = _watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;
            _watcher.Filter = Path.GetFileName(_configPath);

            _watcher.Deleted += (s, e) => Unavailable?.Invoke();
            _watcher.Created += (s, e) => tryLoadConfig();
            _watcher.Changed += (s, e) => tryLoadConfig();
            _watcher.Renamed += (s, e) => tryLoadConfig();
        }

        public T Configuration { get; private set; }

        public void Watch()
        {
            _watcher.EnableRaisingEvents = true;
            tryLoadConfig();
        }

        private void tryLoadConfig()
        {
            try
            {
                Configuration = JsonConvert.DeserializeObject<T>(File.ReadAllText(_configPath));
                Changed?.Invoke();
                _lastUpdateSuccessful = true;
                _recentlyUpdated = true;
            }
            catch (Exception ex)
            {
                Unavailable?.Invoke();
            }
        }
    }
}