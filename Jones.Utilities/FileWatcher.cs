using System;
using System.IO;

namespace Jones.Utilities
{
    public class FileWatcher
    {
        readonly FileSystemWatcher _watcher;

        public FileWatcher(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var dir = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException(dir);
            }
            
            _watcher = new FileSystemWatcher(dir);
            _watcher.Filter = Path.GetFileName(filePath);
            
            _watcher.Deleted += (s, e) => Deleted?.Invoke();
            _watcher.Created += (s, e) => Created?.Invoke();
            _watcher.Changed += (s, e) => Changed?.Invoke();
            _watcher.EnableRaisingEvents = true;
        }

        public Action Created { get; set; }
        public Action Changed { get; set; }
        public Action Deleted { get; set; }
        
    }
}