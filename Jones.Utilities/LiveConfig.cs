using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Jones.Utilities
{
    public sealed class LiveConfig<T>
    {
        private readonly string _configPath;
        readonly FileSystemWatcher _watcher;

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

            var dir = Path.GetDirectoryName(filePath);

            _watcher = new FileSystemWatcher(dir);
            _watcher.Filter = Path.GetFileName(filePath);

            _watcher.Deleted += (s, e) => Unavailable?.Invoke();
            _watcher.Created += (s, e) => tryLoadConfig();
            _watcher.Changed += (s, e) => tryLoadConfig();
        }

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
                Changed();
            }
            catch (Exception ex)
            {
                Unavailable();
            }
        }

        public T Configuration { get; private set; }

        public Action Unavailable { get; set; }
        public Action Changed { get; set; }
    }
}