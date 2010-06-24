using System;
using System.ServiceModel;
using System.Threading;

namespace NinjaFerret.Wcf.Sample.BankManager.WcfHost
{
    public class ServiceRunner
    {
        private readonly Type _service;
        private ServiceHost _serviceHost;
        private bool _aborted;
        private readonly object _lockObject = new object();

        public ServiceRunner(Type service)
        {
            _service = service;
        }

        public void Start()
        {
            lock (_lockObject)
            {
                _aborted = false;
            }
            var thread = new Thread(RunService);
            thread.Start();
        }

        public void Stop()
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("{0} service stopping", _service);
            Console.WriteLine("-----------------------------------------");
            lock (_lockObject)
            {
                _aborted = true;
            }
        }

        private void RunService()
        {
            var aborted = false;
            try
            {
                lock (_lockObject)
                {
                    _serviceHost = new ServiceHost(_service);
                    _serviceHost.Open();
                    Console.WriteLine("=========================================");
                    Console.WriteLine("{0} service started", _service);
                    Console.WriteLine("=========================================");
                }
                while (!_aborted)
                {

                }
            }
            catch(System.Exception e)
            {
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("{0} service errored {1}", _service, e);
                Console.WriteLine("-----------------------------------------");
                aborted = true;
                _serviceHost.Abort();
            }
            finally
            {
                Console.WriteLine("=========================================");
                Console.WriteLine("{0} service stopped", _service);
                Console.WriteLine("=========================================");
                if (!aborted)
                {
                    _serviceHost.Close();
                }
            }
        }
    }
}