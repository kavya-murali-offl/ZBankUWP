using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBank.ViewModel.VMObjects;
using System.Windows.Input;
using Windows.UI.Xaml;
using ZBank.View.DataTemplates.NewPaymentTemplates;
using Windows.UI.Xaml.Controls;
using ZBankManagement.Entity.BusinessObjects;
using ZBankManagement.Entities.BusinessObjects;
using ZBank.View.Modals;
using ZBank.Config;
using ZBank.DataStore;
using ZBank.Services;

namespace ZBank.ViewModel
{
    public class TransferAmountViewModel : ViewModelBase
    {
        private IView View { get; set; }

        public ObservableDictionary<string, string> FieldErrors = new ObservableDictionary<string, string>();
        public bool IsConfirmed { get; set; }
        public TransferAmountViewModel(IView view, ContentDialog dialog = null) {
            View = view;
            ContentDialog = dialog;
            InitializeSteps();
            Reset();
            ViewNotifier.Instance.PaymentInProgress = true;
        }

        private decimal _availableBalance;
        public decimal AvailableBalance
        {
            get { return _availableBalance; }
            set { Set(ref _availableBalance, value); }  
        }

        public AccountBObj SelectedAccount
        {
            get => UserAccounts?.FirstOrDefault(acc => acc.AccountNumber == CurrentTransaction.SenderAccountNumber);
        }

        public AccountBObj SelectedOtherAccount
        {
            get => UserAccounts?.FirstOrDefault(acc => acc.AccountNumber == CurrentTransaction.RecipientAccountNumber);
        }

        public BeneficiaryBObj SelectedBeneficiary
        {
            get => AllBeneficiaries?.FirstOrDefault(acc => acc.AccountNumber == CurrentTransaction.RecipientAccountNumber);
        }

        private ContentDialog ContentDialog { get; set; }

        private void ProceedToPay(object parameter)
        {
            var ReferenceID = Guid.NewGuid().ToString();
            CurrentTransaction.ReferenceID = ReferenceID;   
            CurrentTransaction.RecordedOn = DateTime.Now;

            TransferAmountRequest request = new TransferAmountRequest()
            {
                CustomerID = Repository.Current.CurrentUserID,
                Transaction = CurrentTransaction,
                OwnerAccount = UserAccounts.FirstOrDefault(acc => acc.AccountNumber == CurrentTransaction.SenderAccountNumber),
                Beneficiary = AllBeneficiaries.FirstOrDefault(ben => ben.AccountNumber == CurrentTransaction.RecipientAccountNumber),
                OtherAccount = UserAccounts.FirstOrDefault(acc => acc.AccountNumber == CurrentTransaction.RecipientAccountNumber)
            };

            IPresenterCallback<TransferAmountResponse> presenterCallback = new TransferAmountPresenterCallback(this);
            UseCaseBase<TransferAmountResponse> useCase = new TransferAmountUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public void Reset(object parameter=null) 
        {
            ViewNotifier.Instance.OnPaymentResetRequested();
            CurrentTransaction = new Transaction();
            AvailableBalance = 0m;
            FieldErrors["Amount"] = string.Empty;
            FieldErrors["Description"] = string.Empty;
            FieldErrors["Account"] = string.Empty;
            FieldErrors["Beneficiary"] = string.Empty;
            FieldErrors["Available Balance"] = string.Empty;
            FieldErrors["RecipientAccountNumber"] = string.Empty;
            FieldErrors["SenderAccountNumber"] = string.Empty;
        }

        private void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
                AccountType = null,
                IsTransactionAccounts = true,
                UserID = Repository.Current.CurrentUserID
            };

            IPresenterCallback<GetAllAccountsResponse> presenterCallback = new GetAllAccountsPresenterCallback(this);
            UseCaseBase<GetAllAccountsResponse> useCase = new GetAllAccountsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void LoadAllBeneficiaries()
        {
            GetAllBeneficiariesRequest request = new GetAllBeneficiariesRequest()
            {
                UserID = Repository.Current.CurrentUserID
            };

            IPresenterCallback<GetAllBeneficiariesResponse> presenterCallback = new GetAllBeneficiariesPresenterCallback(this);
            UseCaseBase<GetAllBeneficiariesResponse> useCase = new GetAllBeneficiariesUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public void OnLoaded()
        {
            ViewNotifier.Instance.AccountsListUpdated += UpdateAccountsList;
            ViewNotifier.Instance.BeneficiaryListUpdated += UpdateBeneficiariesList;
            ViewNotifier.Instance.CancelPaymentRequested += CancelPaymentRequested;
            LoadAllBeneficiaries();
            LoadAllAccounts();
        }
        private StepModel ResumeAtStep { get; set; }    

        public void ResumeAtCurrentStep()
        {
            ResumeAtStep = CurrentStep;
        }

        private void CancelPaymentRequested(bool isPaymentCompleted)
        {
            if(isPaymentCompleted)
            {
                Reset();
                GoToNextStep();
            }
            else
            {
                CurrentStep = new StepModel
                {
                    IsPaymentInProgress = false,
                    StepNumber = 4,
                    PrimaryCommandText = "Continue Payment",
                    PrimaryCommand = new RelayCommand(ResumePayment),
                    SecondaryCommandText = "Exit",
                    SecondaryCommand = new RelayCommand(CloseDialog),
                    Content = new CancelTransferTemplate()
                };
            }
        }

        private void UpdateBeneficiariesList(BeneficiaryListUpdatedArgs args)
        {
            AllBeneficiaries = new ObservableCollection<BeneficiaryBObj>(args.BeneficiaryList);
        }

        private void UpdateAccountsList(AccountsListUpdatedArgs args)
        {
            UserAccounts = new ObservableCollection<AccountBObj>(args.AccountsList);
            OtherAccounts = new ObservableCollection<AccountBObj>(args.AccountsList);
        }

        public void ResetOtherAccounts()
        {
            OtherAccounts = UserAccounts;
        }


        public void OnUnloaded()
        {
            ViewNotifier.Instance.AccountsListUpdated -= UpdateAccountsList;
            ViewNotifier.Instance.BeneficiaryListUpdated -= UpdateBeneficiariesList;
            ViewNotifier.Instance.CancelPaymentRequested -= CancelPaymentRequested;
            ViewNotifier.Instance.CloseDialog -= ClosePaymentDialog;
        }

        private bool ValidateFields()
        {

            var list = new List<string>()
            {
                "Amount", "SenderAccountNumber", "RecipientAccountNumber"
            };

            ValidateObject(FieldErrors, typeof(Transaction), list, CurrentTransaction);
            FieldErrors["Beneficiary"] = FieldErrors["RecipientAccountNumber"].Replace("RecipientAccountNumber", "To Account");
            FieldErrors["Account"] = FieldErrors["SenderAccountNumber"].Replace("SenderAccountNumber", "From Account");
            
            if (FieldErrors.Values.Any((val) => val.Length > 0))
                return false;
            return true;
        }


        private async void GoToNextStep(object parameter = null)
        {
            int previousIndex = Steps.IndexOf(CurrentStep);
            switch (previousIndex)
            {
                case 0:
                    if (ValidateFields() && CheckBalance())
                    {
                        IsConfirmed = true;
                        CurrentStep = Steps[previousIndex + 1];
                        if (ContentDialog == null)
                        {
                            ContentDialog dialog = new ContentDialog();
                            dialog.RequestedTheme = ThemeService.Theme;
                            dialog.Content = new NewPaymentView(dialog, this);
                            ContentDialog = dialog;
                            await dialog.ShowAsync();
                        }
                    }
                    else
                    {
                        CurrentStep = Steps[previousIndex]; 
                        IsConfirmed = false;
                    }
                    break;
                case 1:
                    CurrentStep = Steps[previousIndex + 1];
                    break;
                default:
                    break;
            }
        }


        public ObservableCollection<StepModel> Steps { get;  set; }  

        private StepModel _currentStep;
        public StepModel CurrentStep
        {
            get { return _currentStep; }
            set
            {
                _currentStep = value;
                OnPropertyChanged(nameof(CurrentStep));
            }
        }

        private void InitializeSteps()
        {
            Steps = new ObservableCollection<StepModel>
        {
            new StepModel
            {
                IsPaymentInProgress = true,
                StepNumber = 1,
                PrimaryCommandText = "Next",
                SecondaryCommandText = "Reset",
                PrimaryCommand = new RelayCommand(GoToNextStep),
                SecondaryCommand = new RelayCommand(Reset),
                Content = new PaymentDetails()
            },
            new StepModel
            {
                IsPaymentInProgress = true,

                StepNumber = 2,
                PrimaryCommandText = "Proceed To Pay",
                SecondaryCommandText = "Edit",
                PrimaryCommand = new RelayCommand(ProceedToPay),
                SecondaryCommand = new RelayCommand(GoToPreviousStep),
                Content = new PaymentConfirmation()
            },
            new StepModel
            {
                IsPaymentInProgress = false,
                StepNumber = 3,
                SecondaryCommandText="View Bill",
                SecondaryCommand=new RelayCommand(DownloadStatement),
                PrimaryCommandText = "Finish",
                PrimaryCommand = new RelayCommand(CloseDialog),
                Content = new PaymentAcknowledgement()
            },

            };

            // Set the initial current step
            CurrentStep = Steps.FirstOrDefault();
        }

        private void DownloadStatement(object obj)
        {
        }

        private void ResumePayment(object obj)
        {
            CurrentStep = ResumeAtStep;
        }

        private void GoToPreviousStep(object parameter = null)
        {
            int currentIndex = Steps.IndexOf(CurrentStep);
            if(currentIndex != 0) { 
                CurrentStep = Steps[currentIndex - 1]; 
            }
        }

        private bool CheckBalance()
        {
            decimal availableBalance = UserAccounts.FirstOrDefault(acc => acc.AccountNumber == CurrentTransaction.SenderAccountNumber).Balance;
            if (availableBalance < CurrentTransaction.Amount)
            {
                FieldErrors["Amount"] = $"Insufficient Balance. Amount should be less than Rs. {availableBalance.ToString("C")}";
                return false;
            }
            return true;
        }

        internal void CloseDialog(object args = null)
        {
            ClosePaymentDialog();
        }

        private void ClosePaymentDialog()
        {
            ContentDialog.Hide();
            Reset();
        }

        private ObservableCollection<AccountBObj> _accounts = null;

        public ObservableCollection<AccountBObj> UserAccounts
        {
            get { return _accounts; }
            set
            {
                Set(ref _accounts, value);
            }
        }

        private ObservableCollection<AccountBObj> _otherAccounts { get; set; }

        public ObservableCollection<AccountBObj> OtherAccounts
        {
            get { return _otherAccounts; }
            set
            {
                _otherAccounts = value;
                OnPropertyChanged(nameof(OtherAccounts));
            }
        }

        private ObservableCollection<BeneficiaryBObj> _beneficiaries { get; set; }

        public ObservableCollection<BeneficiaryBObj> AllBeneficiaries
        {
            get { return _beneficiaries; }
            set
            {
                _beneficiaries = value;
                OnPropertyChanged(nameof(AllBeneficiaries));
            }
        }

        private Transaction _currentTransaction { get; set; }

        public Transaction CurrentTransaction
        {
            get { return _currentTransaction; }
            set
            {
                _currentTransaction = value;
                OnPropertyChanged(nameof(CurrentTransaction));
            }
        }

        private class GetAllAccountsPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
        {
            private TransferAmountViewModel ViewModel { get; set; }

            public GetAllAccountsPresenterCallback(TransferAmountViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllAccountsResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    AccountsListUpdatedArgs args = new AccountsListUpdatedArgs()
                    {
                        AccountsList = new ObservableCollection<AccountBObj>(response.Accounts)
                    };
                    ViewNotifier.Instance.OnAccountsListUpdated(args);

                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = response.Message,
                            Duration = 3000,
                            Type = NotificationType.ERROR
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });

            }
        }

        private class GetAllBeneficiariesPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
        {
            public TransferAmountViewModel ViewModel { get; set; }

            public GetAllBeneficiariesPresenterCallback(TransferAmountViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllBeneficiariesResponse response)
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

            public async Task OnFailure(ZBankException response)
            {
            }
        }

        private class TransferAmountPresenterCallback : IPresenterCallback<TransferAmountResponse>
        {
            private TransferAmountViewModel ViewModel { get; set; }

            public TransferAmountPresenterCallback(TransferAmountViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(TransferAmountResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnCancelPaymentRequested(true);
                    ViewNotifier.Instance.OnCloseDialog();
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = response.Message,
                            Type = NotificationType.ERROR,
                        }
                    });
                    ViewNotifier.Instance.OnCloseDialog();
                });
            }
        }
    }
}
