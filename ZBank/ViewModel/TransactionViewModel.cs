using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.AppEvents.AppEventArgs;
using ZBank.View;
using ZBank.AppEvents;
using ZBankManagement.Utility;

namespace ZBank.ViewModel
{
    public class TransactionViewModel : ViewModelBase
    {

        public IView View;

        public void LoadAllTransactionsData()
        {
            GetAllTransactionsRequest request = new GetAllTransactionsRequest()
            {
                CustomerID = "1111"
            };

            IPresenterCallback<GetAllTransactionsResponse> presenterCallback = new GetAllTransactionsPresenterCallback(this);
            UseCaseBase<GetAllTransactionsResponse> useCase = new GetAllTransactionsUseCase(request, presenterCallback);
            useCase.Execute();
        }


        public void OnPageLoaded()
        {
            ViewNotifier.Instance.TransactionListUpdated += UpdateTransactionsData;
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.TransactionListUpdated -= UpdateTransactionsData;
        }

        public TransactionViewModel(IView view)
        {
            View = view;
            LoadAllTransactionsData();

        }

        private void UpdateTransactionsData(TransactionPageDataUpdatedArgs args)
        {
            InViewTransactions = new ObservableCollection<TransactionBObj>(args.TransactionList);
        }


        private ObservableCollection<TransactionBObj> _inViewTransactions { get; set; }

        public ObservableCollection<TransactionBObj> InViewTransactions
        {
            get { return _inViewTransactions; }
            set
            {
                _inViewTransactions = value;
                OnPropertyChanged(nameof(InViewTransactions));
            }
        }

    }
}
