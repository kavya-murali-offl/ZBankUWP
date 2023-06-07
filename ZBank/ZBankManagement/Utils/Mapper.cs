using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;

namespace ZBankManagement.Utility
{
    public class Mapper
    { 
       public static TEntity Map<TBusiness, TEntity>(TBusiness businessObject) where TBusiness : new() where TEntity : new()  
        {
            TEntity entityObject = new TEntity();

            PropertyInfo[] businessProperties = typeof(TBusiness).GetProperties();
            PropertyInfo[] entityProperties = typeof(TEntity).GetProperties();

            foreach (PropertyInfo businessProperty in businessProperties)
            {
                foreach (PropertyInfo entityProperty in entityProperties)
                {
                    if (entityProperty.Name == businessProperty.Name)
                    {
                        entityProperty.SetValue(entityObject, businessProperty.GetValue(businessObject));
                        break;
                    }
                }
            }

            return entityObject;
        }

        public static TransactionBObj GetTransactionBObj(Transaction transaction) 
        {
            TransactionBObj transactionBObj = Map<Transaction, TransactionBObj>(transaction);
            if (transaction.TransactionType == TransactionType.DEBIT)
            {
                transactionBObj.BorderColor = "#be3232";
                transactionBObj.BackgroundColor = "#f5e1dd";
                transactionBObj.ArrowIcon = "\uEDDC";
            }
            else if (transaction.TransactionType == TransactionType.CREDIT)
            {
                transactionBObj.BackgroundColor = "#eafde8";
                transactionBObj.BorderColor = "#058365";
                transactionBObj.ArrowIcon = "\uEDDB";
            }
            return transactionBObj;
        }

    }
}
