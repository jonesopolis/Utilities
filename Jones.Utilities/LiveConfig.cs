using System;
using System.IO;

namespace Jones.Utilities
{
    public sealed class LiveConfig<T>
    {
        private readonly string _configPath;

        public LiveConfig(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            _configPath = filePath;

            if (!File.Exists(_configPath))
            {
                throw new FileNotFoundException(_configPath);
            }
        }

        public T Configuration { get; private set; }
    }
}