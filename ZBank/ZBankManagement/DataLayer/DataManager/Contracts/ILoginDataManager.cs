using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagementDB.Interface
{
    public interface ILoginDataManager
    {
        event Action<string> UserChanged;

        bool ValidatePassword(string phoneNumber, string password);

        void LoginCustomer(Customer customer);

        void LogoutCustomer();
    }
}
