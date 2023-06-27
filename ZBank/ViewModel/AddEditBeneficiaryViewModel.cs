using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Entities.BusinessObjects;
using ZBankManagement.Entity.BusinessObjects;
using ZBankManagement.Entity.EnumerationTypes;

namespace ZBank.ViewModel
{
    public class AddEditBeneficiaryViewModel : ViewModelBase
    {
        private IView View { get; set; }

        public bool IsAdd { get; set; }

        public ICommand SubmitCommand { get; set; }

        public BeneficiaryBObj SelectedBeneficiary { get; set; }
        public BeneficiaryBObj InitialBeneficiary { get; set; }
        private ContentDialog Dialog { get; set; }

        public string SubmitText { get; set; }

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

  
        public AddEditBeneficiaryViewModel(IView view, BeneficiaryBObj beneficiary=null, ContentDialog dialog=null) 
        { 
            View = view;
            if(beneficiary == null)
            {
                IsAdd = true;
                SelectedBeneficiary = new BeneficiaryBObj();
                SubmitText = "Add";
            }
            else
            {
                IsAdd = false;
                InitialBeneficiary = beneficiary;
                SelectedBeneficiary = beneficiary;
                SubmitText = "Update";
                Dialog = dialog;
            }
            SubmitCommand = new RelayCommand(ValidateAndSubmit);
            Reset();
        }

        private bool ValidateFields(BeneficiaryType type)
        {
            foreach (var key in FieldValues.Keys)
            {

                if(key != "IFSC Code" && type == BeneficiaryType.WITHIN_BANK)
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
            var beneficiaryType = SelectedBeneficiary.BeneficiaryType;
            if (ValidateFields(beneficiaryType))
            {
               
                if (IsAdd)
                {
                    Beneficiary beneficiary = new Beneficiary()
                    {
                        AccountNumber = FieldValues["Account Number"].ToString(),
                        BeneficiaryName = FieldValues["Beneficiary Name"].ToString(),
                        UserID = "1111",
                        BeneficiaryType = beneficiaryType
                    };
                    AddBeneficiary(beneficiary);
                }
                else
                {
                    UpdateBeneficiary(SelectedBeneficiary);
                }
            }
        }

        private void UpdateBeneficiary(Beneficiary beneficiary)
        {
            UpdateBeneficiaryRequest request = new UpdateBeneficiaryRequest()
            {
                BeneficiaryToUpdate = beneficiary,
            };

            IPresenterCallback<UpdateBeneficiaryResponse> presenterCallback = new UpdateBeneficiaryPresenterCallback(this);
            UseCaseBase<UpdateBeneficiaryResponse> useCase = new UpdateBeneficiaryUseCase(request, presenterCallback);
            useCase.Execute();
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
            SelectedBeneficiary = IsAdd ? new BeneficiaryBObj() : InitialBeneficiary;
            FieldErrors["Account Number"] = string.Empty;
            FieldErrors["IFSC Code"] = string.Empty;
            FieldErrors["Beneficiary Type"] = string.Empty;
            FieldErrors["Beneficiary Name"] = string.Empty;
        }

        public void SetBeneficiaryType(int index)
        {
            FieldValues["Beneficiary Type"] = BeneficiaryTypes.ElementAt(index).ToString();
            UpdateTemplate(BeneficiaryTypes.ElementAt(index));
            Reset();
        }

        internal void OnLoaded()
        {
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


        internal void OnUnloaded()
        {
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
                    ViewNotifier.Instance.OnCloseDialog(true);
                });

                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnBeneficiaryAddOrUpdated(response.InsertedBeneficiary, true);
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
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnBeneficiaryAddOrUpdated(response.UpdatedBeneficiary, false);
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
