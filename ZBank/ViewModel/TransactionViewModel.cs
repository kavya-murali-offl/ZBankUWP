using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZBank.Entities.BusinessObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.AppEvents.AppEventArgs;
using ZBank.View;
using ZBank.AppEvents;
using Windows.UI.Core;

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
            foreach(var transaction in args.TransactionList)
            {
                transaction.SetDefault();
            }
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

    public class GetAllTransactionsPresenterCallback : IPresenterCallback<GetAllTransactionsResponse>
    {
        public TransactionViewModel ViewModel { get; set; }

        public GetAllTransactionsPresenterCallback(TransactionViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public async void OnSuccess(GetAllTransactionsResponse response)
        {
            await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TransactionPageDataUpdatedArgs args = new TransactionPageDataUpdatedArgs()
                {
                    TransactionList = response.Transactions,
                    BeneficiariesList = response.Beneficiaries
                };

                ViewNotifier.Instance.OnTransactionsListUpdated(args);
            });
        }

        public void OnFailure(ZBankException response)
        {
        }
    }

    public class GetAllBeneficiariesInTransactionsPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
    {
        public TransactionViewModel ViewModel { get; set; }

        public GetAllBeneficiariesInTransactionsPresenterCallback(TransactionViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public async void OnSuccess(GetAllBeneficiariesResponse response)
        {
            await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                BeneficiaryListUpdatedArgs args = new BeneficiaryListUpdatedArgs()
                {
                    BeneficiaryList = response.Beneficiaries
                };
                ViewNotifier.Instance.OnBeneficiaryListUpdated(args);
            });
        }

        public void OnFailure(ZBankException response)
        {
        }
    }
}
