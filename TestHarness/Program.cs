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
            Config c = new Config();
            c.PropertyChanged += (s, e) => Console.WriteLine($"{e.PropertyName} : {c.Test}");


            while (true)
            {
                c.Test = Console.ReadLine();
            }
        }
    }

    public class Config : NotifyBase
    {
        private string _test;

        public string Test
        {
            get { return _test; }
            set
            {
                SetProperty(ref _test, value);
            }
        }
    }
}
