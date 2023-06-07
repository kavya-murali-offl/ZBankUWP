using System;
using System.Text;
using ZBankManagement.Controller;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using ZBankManagement.Data;
using System.Linq;
using ZBank.Entities;

namespace ZBankManagement.Utility
{
    public class Helper
    {
        public static T StringToEnum<T>(string data) => (T)Enum.Parse(typeof(T), data);

        public int CountDays()
        {
           
            DateTime? lastWithdrawDate = GetLastWithdrawDate();
            if (lastWithdrawDate.HasValue)
            {
               int numberOfDays = (int)(DateTime.Now - lastWithdrawDate)?.TotalDays;
               if (numberOfDays > 30) return numberOfDays;
            }
            return 0;
        }

        public DateTime? GetLastWithdrawDate()
        {
            if (Store.TransactionsList.Count() > 0)
            {
                Transaction transaction = Store.TransactionsList.Where(data => data.TransactionType == TransactionType.DEBIT).LastOrDefault();
                if (transaction == null)
                    transaction = Store.TransactionsList.Where(data => data.TransactionType == TransactionType.CREDIT).LastOrDefault();
                return transaction.RecordedOn;
            }
            return null;
        }

    }
}
