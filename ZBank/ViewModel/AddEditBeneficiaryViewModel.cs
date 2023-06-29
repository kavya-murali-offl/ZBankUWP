using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Data.Xml.Dom;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using ZBank.AppEvents;
using ZBank.DataStore;
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
                SelectedBeneficiary.BeneficiaryType = BeneficiaryType.WITHIN_BANK;
                SubmitText = "Add";
            }
            else
            {
                IsAdd = false;
                InitialBeneficiary = beneficiary;
                SelectedBeneficiary = beneficiary;
                SubmitText = "Update";
            }
            SubmitCommand = new RelayCommand(ValidateAndSubmit);
            Reset();
        }

        private bool ValidateFields()
        {
            var list = new List<string>()
            {
                "AccountNumber", "BeneficiaryName"
            };

            if (IsOtherBankSelected)
            {
                list.Add("IFSCCode");
            }

            FieldErrors = ValidateField(FieldErrors, typeof(BeneficiaryBObj), list, SelectedBeneficiary);

            if (FieldErrors.Values.Any((val) => val.Length > 0))
                return false;

            return true;
        }

       

        private void ValidateAndSubmit(object obj)
        {
            if (ValidateFields())
            {
               
                if (IsAdd)
                {
                    Beneficiary beneficiary = new Beneficiary()
                    {
                        AccountNumber = SelectedBeneficiary.AccountNumber,
                        BeneficiaryName =SelectedBeneficiary.BeneficiaryName,
                        UserID = Repository.CurrentUserID,
                        BeneficiaryType = IsOtherBankSelected ? BeneficiaryType.OTHER_BANK : BeneficiaryType.WITHIN_BANK
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
                IFSCCode = SelectedBeneficiary.IFSCCode,
                BeneficiaryToInsert = beneficiary,
            };

            IPresenterCallback<InsertBeneficiaryResponse> presenterCallback = new InsertBeneficiaryPresenterCallback(this);
            UseCaseBase<InsertBeneficiaryResponse> useCase = new InsertBeneficiaryUseCase(request, presenterCallback);
            useCase.Execute();
        }


        public ObservableDictionary<string, string> FieldErrors = new ObservableDictionary<string, string>();

        private void Reset()
        {
            SelectedBeneficiary = IsAdd ? new BeneficiaryBObj() : InitialBeneficiary;
            FieldErrors["AccountNumber"] = string.Empty;
            FieldErrors["IFSCCode"] = string.Empty;
            FieldErrors["BeneficiaryType"] = string.Empty;
            FieldErrors["BeneficiaryName"] = string.Empty;
        }

        public void SetBeneficiaryType(int index)
        {
            SelectedBeneficiary.BeneficiaryType = BeneficiaryTypes.ElementAt(index);
            UpdateTemplate();
            Reset();
        }

            
        private void UpdateTemplate()
        {
            switch (SelectedBeneficiary.BeneficiaryType)
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
            Reset();
        }

        internal void OnLoaded()
        {
            ViewNotifier.Instance.BeneficiaryAddOrUpdated += BeneficiaryAddedOrUpdated;
            Reset();
        }

        internal void OnUnloaded()
        {
            ViewNotifier.Instance.BeneficiaryAddOrUpdated -= BeneficiaryAddedOrUpdated;
        }

        private void BeneficiaryAddedOrUpdated(Beneficiary arg1, bool arg2)
        {
            Reset();
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
                    ViewNotifier.Instance.OnCloseDialog(true);
                });
            }

            public async Task OnFailure(ZBankException exception)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnRequestFailed(true);
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = exception.Message,
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
                    ViewNotifier.Instance.OnCloseDialog(true);
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
                    ViewNotifier.Instance.OnRequestFailed(true);
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
