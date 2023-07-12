using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.View;
using ZBankManagement.AppEvents.AppEventArgs;
using static ZBankManagement.Domain.UseCase.ResetPin;
using ZBankManagement.Domain.UseCase;
using ZBank.Services;
using ZBank.AppEvents;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;

namespace ZBank.ViewModel
{
    public class ResetPinViewModel : ViewModelBase
    {
        public ResetPinViewModel(IView view)
        {
            View = view;
        }

        private string _errorText = string.Empty;
        public string ErrorText
        {
            get => _errorText;
            set => Set(ref _errorText, value);
        }

        private string _newPin = string.Empty;
        public string NewPin
        {
            get => _newPin;
            set => Set(ref _newPin, value);
        }

        private string _cardNumber = string.Empty;
        public string CardNumber
        {
            get => _cardNumber;
            set => Set(ref _cardNumber, value);
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(NewPin) || string.IsNullOrWhiteSpace(NewPin))
            {
                ErrorText = "Pin Number is Required";
            }
            else if (NewPin.Length != 4 || !int.TryParse(NewPin, out _))
            {
                ErrorText = "Pin Number should be a number of 4 digits";
            }

            if (ErrorText == null || ErrorText.Length == 0)
            {
                return true;
            }
            return false;
        }

        internal void SubmitForm()
        {
            if (ValidateFields())
            {
                ResetPin();
            }
        }

        private void ResetPin()
        {
            ResetPinRequest request = new ResetPinRequest()
            {
                CardNumber = CardNumber,
                NewPin = NewPin
            };

            IPresenterCallback<ResetPinResponse> presenterCallback = new ResetPinPresenterCallback(this);
            UseCaseBase<ResetPinResponse> useCase = new ResetPinUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private class ResetPinPresenterCallback : IPresenterCallback<ResetPinResponse>
        {
            private ResetPinViewModel ViewModel { get; set; }

            public ResetPinPresenterCallback(ResetPinViewModel cardsViewModel)
            {
                ViewModel = cardsViewModel;
            }

            public async Task OnSuccess(ResetPinResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = "Reset Pin Successful",
                        Type = NotificationType.SUCCESS
                    });
                    ViewNotifier.Instance.OnCloseDialog();
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = response.Message,
                        Type = NotificationType.ERROR
                    });
                });
            }
        }
    }
}
