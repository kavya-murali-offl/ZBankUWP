using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.AppEvents;
using ZBank.DataStore;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Services;
using ZBank.View;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ViewModel
{
    internal class AddCardViewModel : ViewModelBase
    {

        public AddCardViewModel(IView view) 
        { 
            View = view;
        }

        public IEnumerable<CreditCardProvider> CreditCardProviders
        {
            get => new List<CreditCardProvider>()
            {
                CreditCardProvider.VISA,
                CreditCardProvider.MASTERCARD,
                CreditCardProvider.RUPAY
            };
        }

        private CreditCardProvider _selectedCreditCardProvider = CreditCardProvider.VISA;

        public CreditCardProvider SelectedCreditCardProvider
        {
            get { return _selectedCreditCardProvider; }
            set { Set(ref _selectedCreditCardProvider, value); }
        }

        public void InsertCard()
        {
            InsertCardRequest request = new InsertCardRequest()
            {
                CardType = CardType.CREDIT,
                CustomerID = Repository.Current.CurrentUserID,
                CreditCardProvider = SelectedCreditCardProvider
            };
            IPresenterCallback<InsertCardResponse> presenterCallback = new InsertCardPresenterCallback(this);
            UseCaseBase<InsertCardResponse> useCase = new InsertCardUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private class InsertCardPresenterCallback : IPresenterCallback<InsertCardResponse>
        {
            private AddCardViewModel ViewModel { get; set; }

            public InsertCardPresenterCallback(AddCardViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(InsertCardResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnCardInserted(true, response.InsertedCard);
                    ViewNotifier.Instance.OnCloseDialog();
                });

                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = "Card inserted successfully",
                        Type = NotificationType.SUCCESS
                    });
                });
            }

            public async Task OnFailure(ZBankException exception)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = exception.Message,
                        Type = NotificationType.ERROR
                    });
                });
            }
        }

    }
}
