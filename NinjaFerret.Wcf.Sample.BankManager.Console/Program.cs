using System.Linq;
using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using NinjaFerret.Wcf.Sample.BankManager.Client;
using Ninjaferret.Wcf.Sample.BankManager.Interface.Exception;
using Ninjaferret.Wcf.Sample.BankManager.Interface.Model;

namespace NinjaFerret.Wcf.Sample.BankManager.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));

            var bankAdmin = repository.Resolve<IBankAdministrator>();
            
            try
            {
                bankAdmin.Get("00000000");
            }
            catch (AccountDoesNotExistException)
            {
                // Creating a new account
                WriteLine("Creating a current account for Ian Johnson");

                bankAdmin.CreateAccount("Ian Johnson", AccountType.Current);
            }
            var account = bankAdmin.FindAccount("Ian Johnson", AccountType.Current).First();
            WriteLine("Account found number {0}", account.AccountNumber);

            // Make some deposits
            WriteLine("Depositing £1000 to account {0}", account.AccountNumber);
            bankAdmin.Deposit(account.AccountNumber, 1000, "Opening balance");

            WriteLine("Depositing £250 to account {0}", account.AccountNumber);
            bankAdmin.Deposit(account.AccountNumber, 250, "Cheque 11111");

            // Make withdrawals
            WriteLine("Withdrawing £750 from account {0}", account.AccountNumber);
            bankAdmin.Withdraw(account.AccountNumber, 750, "New computer");

            try
            {
                bankAdmin.Withdraw(account.AccountNumber, 1000, "Even bigger computer");
            }
            catch (InsufficientFundsException e)
            {
                WriteLine(e.Message);
            }


            WriteLine("Press any key to continue...");
            System.Console.ReadKey(true);


        }

        private static void WriteLine(string line)
        {
            System.Console.WriteLine(line);
        }

        private static void WriteLine(string lineFormat, params object[] args)
        {
            System.Console.WriteLine(lineFormat, args);
        }
    }

}
