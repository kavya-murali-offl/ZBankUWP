using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Entity.BusinessObjects;
using ZBankManagement.Entity.EnumerationTypes;

namespace ZBank.ViewModel
{
    public class AddEditBeneficiaryViewModel : ViewModelBase
    {
        private IView View { get; set; }

        public ICommand SubmitCommand { get; set; }
    private bool _isOtherBankSelected {  get; set; }    
        public bool IsOtherBankSelected { get
            {
                return _isOtherBankSelected;
            }
            set
            {
                _isOtherBankSelected = value;
                OnPropertyChanged(nameof(IsOtherBankSelected));
            }
        }

  
        public AddEditBeneficiaryViewModel(IView view) 
        { 
            View = view;
            SubmitCommand = new RelayCommand(ValidateAndSubmit);
            Reset();
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
        }

        private void ValidateAndSubmit(object obj)
        {
            if (ValidateFields())
            {
                Beneficiary beneficiary = new Beneficiary()
                {
                    AccountNumber = FieldValues["Account Number"].ToString(),
                    BeneficiaryName = FieldValues["Beneficiary Name"].ToString(),
                    UserID = "1111",
                    BeneficiaryType = BeneficiaryTypes.FirstOrDefault(type => FieldValues["Beneficiary Type"].ToString() == type.ToString())
                };

                AddBeneficiary(beneficiary);
            }
        }

        private void AddBeneficiary(Beneficiary beneficiary)
        {
            InsertBeneficiaryRequest request = new InsertBeneficiaryRequest()
            {
                BeneficiaryToInsert = beneficiary,
            };

            IPresenterCallback<InsertBeneficiaryResponse> presenterCallback = new InsertBeneficiaryPresenterCallback(this);
            UseCaseBase<InsertBeneficiaryResponse> useCase = new InsertBeneficiaryUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public ObservableDictionary<string, string> FieldErrors = new ObservableDictionary<string, string>();
        public ObservableDictionary<string, object> FieldValues = new ObservableDictionary<string, object>();

        private void Reset()
        {
            FieldValues["Beneficiary Name"] = null;
            FieldValues["Account Number"] = null;
            FieldValues["IFSC Code"] = null;
            FieldValues["Beneficiary Type"] = BeneficiaryTypes.ElementAt(0).ToString();
            FieldErrors["Account Number"] = string.Empty;
            FieldErrors["IFSC Code"] = string.Empty;
            FieldErrors["Beneficiary Type"] = string.Empty;
            FieldErrors["Beneficiary Name"] = string.Empty;
        }

        public void SetBeneficiaryType(int index)
        {
            FieldValues["Beneficiary Type"] = BeneficiaryTypes.ElementAt(index).ToString();
            UpdateTemplate(BeneficiaryTypes.ElementAt(index));
        }

        internal void OnLoaded()
        {
            ViewNotifier.Instance.BeneficiaryAddOrInserted += OnBeneficiaryAddedOrUpdated;
        }

        private void UpdateTemplate(BeneficiaryType type)
        {
            switch (type)
            {
                case BeneficiaryType.WITHIN_BANK:
                    IsOtherBankSelected = false;
                    break;
                case BeneficiaryType.OTHER_BANK:
                    IsOtherBankSelected = true;
                    break;
                default: 
                    break; 
            }
        }

        private void OnBeneficiaryAddedOrUpdated(Beneficiary beneficiary, bool isAdded)
        {
            Reset();
        }

        internal void OnUnloaded()
        {
            ViewNotifier.Instance.BeneficiaryAddOrInserted -= OnBeneficiaryAddedOrUpdated;
        }

        public IEnumerable<BeneficiaryType> BeneficiaryTypes
        {
            get
            {
                return new List<BeneficiaryType>()
                {
                    BeneficiaryType.WITHIN_BANK,
                    BeneficiaryType.OTHER_BANK
                };
            }
        }
        private class InsertBeneficiaryPresenterCallback : IPresenterCallback<InsertBeneficiaryResponse>
        {
            public AddEditBeneficiaryViewModel ViewModel { get; set; }

            public InsertBeneficiaryPresenterCallback(AddEditBeneficiaryViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(InsertBeneficiaryResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnBeneficiaryAddOrInserted(response.InsertedBeneficiary, true);
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = "Beneficiary Inserted Successfully",
                            Type = NotificationType.SUCCESS
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });
            }

            public async Task OnFailure(ZBankException exception)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = exception,
                            Type = NotificationType.ERROR
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });
            }
        }

        private class UpdateBeneficiaryPresenterCallback : IPresenterCallback<UpdateBeneficiaryResponse>
        {
            public AddEditBeneficiaryViewModel ViewModel { get; set; }

            public UpdateBeneficiaryPresenterCallback(AddEditBeneficiaryViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(UpdateBeneficiaryResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnBeneficiaryAddOrInserted(response.UpdatedBeneficiary, false);
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = "Beneficiary Updated Successfully",
                            Type = NotificationType.SUCCESS
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });
            }

            public async Task OnFailure(ZBankException exception)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = exception,
                            Type = NotificationType.ERROR
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });
            }
        }

    }


}
