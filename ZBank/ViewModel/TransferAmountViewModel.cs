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
using static Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ExpressionValues;
using ZBankManagement.Entities.BusinessObjects;
using ZBank.View.Modals;

namespace ZBank.ViewModel
{
    public class TransferAmountViewModel : ViewModelBase
    {
        private IView View { get; set; }

        public ObservableDictionary<string, string> FieldErrors = new ObservableDictionary<string, string>();
        public ObservableDictionary<string, object> FieldValues = new ObservableDictionary<string, object>();

        public TransferAmountViewModel(IView view, ContentDialog dialog = null) {
            View = view;
            ContentDialog = dialog;
            InitializeSteps();
            Reset();
        }

        private ContentDialog ContentDialog { get; set; }   

        public BeneficiaryBObj SelectedBeneficiary
        {
            get { 
                return AllBeneficiaries.FirstOrDefault(ben =>
                ben.ToString().Equals(FieldValues["Beneficiary"].ToString())
            ); }
        }

        public AccountBObj SelectedAccount
        {
            get
            {
                return UserAccounts.FirstOrDefault(ben =>
                 ben.ToString().Equals(FieldValues["Account"].ToString())
            );
            }
        }

        private void ProceedToPay(object parameter) 
        {
            var ReferenceID = Guid.NewGuid().ToString();
            var userAccount = UserAccounts.FirstOrDefault(acc => acc.ToString().Equals(FieldValues["Account"]));
            TransferAmountRequest request = new TransferAmountRequest()
            {
                Transaction = new Transaction()
                {
                    Amount = decimal.Parse(FieldValues["Amount"].ToString()),
                    RecipientAccountNumber = AllBeneficiaries.FirstOrDefault(ben => ben.ToString().Equals(FieldValues["Beneficiary"])).AccountNumber,
                    SenderAccountNumber = UserAccounts.FirstOrDefault(ben => ben.ToString().Equals(FieldValues["Account"])).AccountNumber,
                    Description = FieldValues["Description"].ToString(),
                    RecordedOn = DateTime.Now,
                    ReferenceID  = ReferenceID,
                },
                OwnerAccount = userAccount,
                Beneficiary = AllBeneficiaries.FirstOrDefault(acc => acc.ToString().Equals(FieldValues["Beneficiary"])),
            };

            IPresenterCallback<TransferAmountResponse> presenterCallback = new TransferAmountPresenterCallback(this);
            UseCaseBase<TransferAmountResponse> useCase = new TransferAmountUseCase(request, presenterCallback);
            useCase.Execute();

        }

        public void ValidateField(string fieldName)
        {
            if (!FieldValues.TryGetValue(fieldName, out object val) || string.IsNullOrEmpty(FieldValues[fieldName]?.ToString()))
            {
                FieldErrors[fieldName] = $"{fieldName} is required.";
            }
            else
            {
                FieldErrors[fieldName] = string.Empty;
            }

            if (fieldName == "Amount")
            {
                var inText = FieldValues["Amount"].ToString();
                if (decimal.TryParse(inText, out decimal amountInDecimal))
                {
                    if (amountInDecimal <= 0)
                    {
                        FieldErrors["Amount"] = "Amount should be greater than zero";
                    }
                    else
                    {
                        FieldErrors["Amount"] = string.Empty;
                    }
                }
                else
                {
                    FieldErrors["Amount"] = "Please enter a valid Amount";
                }
            }
        }

        private void Reset(object parameter=null) 
        {
            FieldValues["Amount"] = string.Empty;
            FieldValues["Description"] = string.Empty;
            FieldValues["Account"] = null;
            FieldValues["Beneficiary"] = null;
            FieldValues["Available Balance"] = 0.0m;
            FieldErrors["Amount"] = string.Empty;
            FieldErrors["Description"] = string.Empty;
            FieldErrors["Account"] = string.Empty;
            FieldErrors["Beneficiary"] = string.Empty;
            FieldErrors["Available Balance"] = string.Empty;
        }

        private void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
                AccountType = null,
                UserID = "1111"
            };

            IPresenterCallback<GetAllAccountsResponse> presenterCallback = new GetAllAccountsPresenterCallback(this);
            UseCaseBase<GetAllAccountsResponse> useCase = new GetAllAccountsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void LoadAllBeneficiaries()
        {
            GetAllBeneficiariesRequest request = new GetAllBeneficiariesRequest()
            {
                UserID = "1111"
            };

            IPresenterCallback<GetAllBeneficiariesResponse> presenterCallback = new GetAllBeneficiariesPresenterCallback(this);
            UseCaseBase<GetAllBeneficiariesResponse> useCase = new GetAllBeneficiariesUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public void UpdateBeneficiary(int index)
        {
            FieldValues["Beneficiary"] = AllBeneficiaries[index].ToString();
            ValidateField("Beneficiary");

        }

        public void UpdateUserAccount(int index)
        {
            FieldValues["Account"] = UserAccounts[index].ToString();
            FieldValues["Available Balance"] = UserAccounts[index].Balance;
            ValidateField("Account");
        }

        public string UpdateAndGetAmount(string text)
        {
            text = new string(text.Where(c => char.IsDigit(c) || c == '.').ToArray());
            //sender.SelectionStart = newText.Length;
            FieldValues["Amount"] = text;
            ValidateField("Amount");
            return text;

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
        }

        public void OnUnloaded()
        {
            ViewNotifier.Instance.AccountsListUpdated -= UpdateAccountsList;
            ViewNotifier.Instance.BeneficiaryListUpdated -= UpdateBeneficiariesList;
            ViewNotifier.Instance.CancelPaymentRequested += CancelPaymentRequested;

        }

        private bool ValidateFields()
        {
            foreach (var key in FieldValues.Keys)
            {
                ValidateField(key);
            }

            if (FieldErrors.Values.Any((val) => val.Length > 0))
                return false;
            return true;
        }


        private async void GoToNextStep(object parameter = null)
        {
            int previousIndex = Steps.IndexOf(CurrentStep);
            if (Steps.Count > previousIndex + 1)
            {
                CurrentStep = Steps[previousIndex + 1];
            }
            switch (previousIndex)
            {
                case 0:
                    if (ValidateFields() && CheckBalance())
                    {
                        if (ContentDialog == null)
                        {
                            ContentDialog dialog = new ContentDialog();
                            dialog.Content = new NewPaymentView(dialog, this);
                            ContentDialog = dialog;
                            await dialog.ShowAsync();
                        }
                    }
                    break;
                case 1:
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
            decimal availableBalance = decimal.Parse(FieldValues["Available Balance"].ToString());
            decimal amount = decimal.Parse(FieldValues["Amount"].ToString());
            if (availableBalance < amount)
            {
                FieldErrors["Amount"] = $"Insufficient Balance. Amount should be less than Rs. {availableBalance.ToString("C")}";
                return false;
            }
            return true;
        }

        internal void CloseDialog(object args = null)
        {
            ContentDialog.Hide();
            Reset();
        }

        private ObservableCollection<AccountBObj> _accounts { get; set; }

        public ObservableCollection<AccountBObj> UserAccounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(UserAccounts));
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
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnCancelPaymentRequested(true);
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = response.Message,
                            Type = NotificationType.ERROR,
                        }
                    });
                });
            }
        }
    }
}
