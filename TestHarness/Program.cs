using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jones.Utilities;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var live = new LiveConfig<IEnumerable<Config>>(@"C:\users\jonesy\desktop\config.json");
            live.Changed += () => Console.WriteLine(string.Join(", ", live.Configuration.Select(c => c.Test)));
            live.Unavailable += () => Console.WriteLine("Unavailable");
            live.Watch();

            Console.Read();
        }
    }

    public class Config
    {
        public string Test { get; set; }
    }
}
