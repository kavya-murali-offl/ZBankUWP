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
    public class AddEditBeneficiaryViewModel : FormBase<BeneficiaryBObj>
    {

        public ICommand SubmitCommand { get; set; }

        public string SubmitText { get; set; }

        private bool _isOtherBankSelected = false;
        
        public bool IsOtherBankSelected { get
            {
                return _isOtherBankSelected;
            }
            set => Set(ref _isOtherBankSelected, value);
        }
  
        public AddEditBeneficiaryViewModel(IView view, BeneficiaryBObj beneficiary=null)         { 
            View = view;
            if(beneficiary == null)
            {
                IsNew = true;
                Item = new BeneficiaryBObj();
                SubmitText = "Add";
            }
            else
            {
                IsNew = false;
                Item = beneficiary;
                SubmitText = "Update";
            }

           
            SubmitCommand = new RelayCommand(ValidateAndSubmit);
            Reset(Item.BeneficiaryType);
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

            ValidateObject(FieldErrors, typeof(BeneficiaryBObj), list, EditableItem);
            if (!IsNew)
            {
                if (Item.BeneficiaryName == EditableItem.BeneficiaryName?.Trim())
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
                if (IsNew)
                {
                    Beneficiary beneficiary = new Beneficiary()
                    {
                        AccountNumber = EditableItem.AccountNumber.Trim(),
                        BeneficiaryName =EditableItem.BeneficiaryName.Trim(),
                        UserID = Repository.Current.CurrentUserID,
                        BeneficiaryType = IsOtherBankSelected ? BeneficiaryType.OTHER_BANK : BeneficiaryType.WITHIN_BANK
                    };
                    AddBeneficiary(beneficiary);
                }
                else
                {
                    UpdateBeneficiary(EditableItem);
                }
            }
        }

        private void UpdateBeneficiary(BeneficiaryBObj beneficiary)
        {
            UpdateBeneficiaryRequest request = new UpdateBeneficiaryRequest()
            {
                BeneficiaryToUpdate = new Beneficiary()
                {
                    AccountNumber = beneficiary.AccountNumber,  
                    ID = beneficiary.ID,
                    BeneficiaryName = beneficiary.BeneficiaryName.Trim(),
                    UserID = beneficiary.UserID,    
                },
            };

            IPresenterCallback<UpdateBeneficiaryResponse> presenterCallback = new UpdateBeneficiaryPresenterCallback(this);
            UseCaseBase<UpdateBeneficiaryResponse> useCase = new UpdateBeneficiaryUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void AddBeneficiary(Beneficiary beneficiary)
        {
            InsertBeneficiaryRequest request = new InsertBeneficiaryRequest()
            {
                IFSCCode = EditableItem.IFSCCode,
                BeneficiaryToInsert = beneficiary,
            };

            IPresenterCallback<InsertBeneficiaryResponse> presenterCallback = new InsertBeneficiaryPresenterCallback(this);
            UseCaseBase<InsertBeneficiaryResponse> useCase = new InsertBeneficiaryUseCase(request, presenterCallback);
            useCase.Execute();
        }


        public ObservableDictionary<string, string> FieldErrors = new ObservableDictionary<string, string>();

        public void Reset(BeneficiaryType type)
        {
            EditableItem = IsNew ? new BeneficiaryBObj() : (BeneficiaryBObj)Item.Clone();
            EditableItem.BeneficiaryType = type;
            FieldErrors["AccountNumber"] = string.Empty;
            FieldErrors["IFSCCode"] = string.Empty;
            FieldErrors["BeneficiaryType"] = string.Empty;
            FieldErrors["BeneficiaryName"] = string.Empty;
        }

        public void SetBeneficiaryType(int index)
        {
            var type = BeneficiaryTypes.ElementAt(index);
            EditableItem.BeneficiaryType = type;
            UpdateTemplate();
            Reset(type);
        }

        private void UpdateTemplate()
        {
            switch (EditableItem.BeneficiaryType)
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
            Reset(EditableItem.BeneficiaryType);
        }

        private void AddEditRequestFailed(bool obj)
        {
            Reset(EditableItem.BeneficiaryType);
        }

        internal void OnUnloaded()
        {
            ViewNotifier.Instance.BeneficiaryAddOrUpdated -= BeneficiaryAddedOrUpdated;
            ViewNotifier.Instance.RequestFailed -= AddEditRequestFailed;
        }

        private void BeneficiaryAddedOrUpdated(Beneficiary arg1, bool arg2)
        {
            Reset(EditableItem.BeneficiaryType);
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
                    ViewNotifier.Instance.OnCloseDialog();
                   
                });
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = "Beneficiary Inserted Successfully",
                        Type = NotificationType.SUCCESS
                    });
                });
            }


            public async Task OnFailure(ZBankException exception)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnRequestFailed(true);
                });
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
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
                  
                });

                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
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
                   
                });
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
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
