using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities;

namespace ZBank.ViewModel
{
    public class TransactionViewModel
    {
        private ObservableCollection<TransactionBObj> _transactions { get; set; }

        public TransactionViewModel()
        {
            _transactions = new ObservableCollection<TransactionBObj>
            {
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 11000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Paytm", "Shopping"),
                new TransactionBObj(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 1000, DateTime.Now, 3000, "Income", "Shopping"),
                new TransactionBObj(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 4000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Flipkart", "Shopping")
            };
        }

        private ObservableCollection<TransactionBObj> AllTransactions
        {
            get { return _transactions; }
            set { _transactions = value; OnPropertyChanged("_transactions"); }
        }

        public ObservableCollection<TransactionBObj> LatestTransaction()
        {
            return (ObservableCollection<TransactionBObj>)AllTransactions.Reverse().Take(5);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
