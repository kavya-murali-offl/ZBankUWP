using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Model;

namespace ZBank.ViewModel
{
    public class TransactionViewModel
    {
        private ObservableCollection<Transaction> _transactions { get; set; }

        public TransactionViewModel()
        {

            _transactions = new ObservableCollection<Model.Transaction>
            {
                new Transaction(TransactionType.INCOME, ModeOfPayment.DIRECT, 11000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new Transaction(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Paytm", "Shopping"),
                new Transaction(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 1000, DateTime.Now, 3000, "Income", "Shopping"),
                new Transaction(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 4000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new Transaction(TransactionType.INCOME, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Flipkart", "Shopping")
            };
        }

        private ObservableCollection<Transaction> AllTransactions
        {
            get { return _transactions; }
            set { _transactions = value; OnPropertyChanged("_transactions"); }
        }

        public IEnumerable<Transaction> GetLatestTransaction()
        {
            return AllTransactions.Reverse().Take(5);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
