using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NinjaFerret.Wcf.Sample.BankManager.Service;

namespace NinjaFerret.Wcf.Sample.BankManager.WcfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var runners = new List<ServiceRunner>(2);
            var types = new[] {typeof (TransactionServiceWrapper), typeof (AccountServiceWrapper)};
            runners.AddRange(types.Select(type => new ServiceRunner(type)));
            foreach (var serviceRunner in runners)
            {
                serviceRunner.Start();
            }
            Console.WriteLine("When ready press any key to continue...");
            Console.ReadKey(true);
            foreach (var serviceRunner in runners)
            {
                serviceRunner.Stop();
            }
        }
    }
}
