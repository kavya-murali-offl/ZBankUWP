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
using ZBank.Services;
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

        private BeneficiaryBObj _selectedBeneficiary = new BeneficiaryBObj();
        
        public BeneficiaryBObj SelectedBeneficiary
        {
            get { return _selectedBeneficiary; }
            set { Set(ref _selectedBeneficiary, value); }
        }

        public  BeneficiaryBObj InitialBeneficiary { get; set; }

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

  
        public AddEditBeneficiaryViewModel(IView view, BeneficiaryBObj beneficiary=null) 
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
            Reset(SelectedBeneficiary.BeneficiaryType);
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

            ValidateObject(FieldErrors, typeof(BeneficiaryBObj), list, SelectedBeneficiary);
            if (!IsAdd)
            {
                if (InitialBeneficiary.BeneficiaryName == SelectedBeneficiary.BeneficiaryName)
                {
                    FieldErrors["BeneficiaryName"] = "Enter a different beneficiary name";
                }
            }

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
                        AccountNumber = SelectedBeneficiary.AccountNumber.Trim(),
                        BeneficiaryName =SelectedBeneficiary.BeneficiaryName.Trim(),
                        UserID = Repository.Current.CurrentUserID,
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

        public void Reset(BeneficiaryType type)
        {
            SelectedBeneficiary = IsAdd ? new BeneficiaryBObj() : InitialBeneficiary;
            SelectedBeneficiary.BeneficiaryType = type;
            FieldErrors["AccountNumber"] = string.Empty;
            FieldErrors["IFSCCode"] = string.Empty;
            FieldErrors["BeneficiaryType"] = string.Empty;
            FieldErrors["BeneficiaryName"] = string.Empty;
        }

        public void SetBeneficiaryType(int index)
        {
            var type = BeneficiaryTypes.ElementAt(index);
            SelectedBeneficiary.BeneficiaryType = type;
            UpdateTemplate(type);
            Reset(type);
        }

        private void UpdateTemplate(BeneficiaryType type)
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
        }

        internal void OnLoaded()
        {
            ViewNotifier.Instance.BeneficiaryAddOrUpdated += BeneficiaryAddedOrUpdated;
            ViewNotifier.Instance.RequestFailed += AddEditRequestFailed;
            Reset(SelectedBeneficiary.BeneficiaryType);
        }

        private void AddEditRequestFailed(bool obj)
        {
            Reset(SelectedBeneficiary.BeneficiaryType);
        }

        internal void OnUnloaded()
        {
            ViewNotifier.Instance.BeneficiaryAddOrUpdated -= BeneficiaryAddedOrUpdated;
            ViewNotifier.Instance.RequestFailed -= AddEditRequestFailed;
        }

        private void BeneficiaryAddedOrUpdated(Beneficiary arg1, bool arg2)
        {
            Reset(SelectedBeneficiary.BeneficiaryType);
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
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnBeneficiaryAddOrUpdated(response.InsertedBeneficiary, true);
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = "Beneficiary Inserted Successfully",
                        Type = NotificationType.SUCCESS
                    });
                    ViewNotifier.Instance.OnCloseDialog();
                });
            }

            public async Task OnFailure(ZBankException exception)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnRequestFailed(true);
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification() { Message = exception.Message, Type = NotificationType.ERROR });
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
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnCloseDialog();
                    ViewNotifier.Instance.OnBeneficiaryAddOrUpdated(response.UpdatedBeneficiary, true);
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = "Beneficiary Updated Successfully",
                        Type = NotificationType.SUCCESS
                    });
                });
            }

            public async Task OnFailure(ZBankException exception)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnCloseDialog();
                    ViewNotifier.Instance.OnRequestFailed(true);
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = exception,
                        Type = NotificationType.ERROR
                    });
                });
            }
        }

    }


}
