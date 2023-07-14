using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.DataStore;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Services;
using ZBank.View;
using ZBank.View.UserControls;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Entities.BusinessObjects;
using ZBankManagement.Entity.EnumerationTypes;

namespace ZBank.ViewModel
{
    public class BeneficiariesViewModel : ViewModelBase
    {
        public BeneficiariesViewModel(IView view)
        {
            View = view;
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
            ViewNotifier.Instance.BeneficiaryListUpdated += UpdateBeneficiaryList;
            ViewNotifier.Instance.BeneficiaryAddOrUpdated += OnBeneficiaryAddedOrUpdated;
            ViewNotifier.Instance.BeneficiaryRemoved += OnBeneficiaryRemoved;
            LoadAllBeneficiaries();

        }

        private void OnBeneficiaryRemoved(Beneficiary obj)
        {
            LoadAllBeneficiaries();
        }

        private void UpdateBeneficiaryList(BeneficiaryListUpdatedArgs args)
        {
            BeneficiariesList = new ObservableCollection<BeneficiaryBObj>(args.BeneficiaryList);
            OtherBankBeneficiaries = new ObservableCollection<BeneficiaryBObj>(args.BeneficiaryList.Where(ben => ben.BeneficiaryType == BeneficiaryType.OTHER_BANK));
            WithinBankBeneficiaries = new ObservableCollection<BeneficiaryBObj>(args.BeneficiaryList.Where(ben => ben.BeneficiaryType == BeneficiaryType.WITHIN_BANK));
            FavouriteBeneficiaries = new ObservableCollection<BeneficiaryBObj>(args.BeneficiaryList.Where(ben => ben.IsFavourite));
        }

        public void OnUnloaded()
        {
            ViewNotifier.Instance.BeneficiaryListUpdated -= UpdateBeneficiaryList;
            ViewNotifier.Instance.BeneficiaryAddOrUpdated -= OnBeneficiaryAddedOrUpdated;
            ViewNotifier.Instance.BeneficiaryRemoved -= OnBeneficiaryRemoved;
        }

        private void OnBeneficiaryAddedOrUpdated(Beneficiary arg1, bool arg2)
        {
            LoadAllBeneficiaries();
        }

        internal void UpdateList(BeneficiaryType type, string input)
        {
            switch(type)
            {
                case BeneficiaryType.WITHIN_BANK:
                    break;
                case BeneficiaryType.OTHER_BANK:
                    break;
            }
        }

        internal void DeleteBeneficiary(BeneficiaryBObj selectedBeneficiary)
        {
            RemoveBeneficiaryRequest request = new RemoveBeneficiaryRequest()
            {
                BeneficiaryToRemove = new Beneficiary()
                {
                    ID = selectedBeneficiary.ID,
                }
            };

            IPresenterCallback<RemoveBeneficiaryResponse> presenterCallback = new RemoveBeneficiaryPresenterCallback(this);
            UseCaseBase<RemoveBeneficiaryResponse> useCase = new RemoveBeneficiaryUseCase(request, presenterCallback);
            useCase.Execute();
        }

        internal void SwitchFavourite(BeneficiaryBObj selectedBeneficiary)
        {
            selectedBeneficiary.IsFavourite = !selectedBeneficiary.IsFavourite;
            UpdateBeneficiary(selectedBeneficiary);
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

        internal async Task OpenDialog(BeneficiaryBObj selectedBeneficiary)
        {
            await DialogService.ShowContentAsync(
                View, 
                new AddEditBeneficiaryView(selectedBeneficiary),
                "Edit Beneficiary", 
                Window.Current.Content.XamlRoot
           );
        }

        private ObservableCollection<BeneficiaryBObj> _beneficiariesList = new ObservableCollection<BeneficiaryBObj>();

        public ObservableCollection<BeneficiaryBObj> BeneficiariesList
        {
            get => _beneficiariesList;
            set => Set(ref _beneficiariesList, value);
        }

        private ObservableCollection<BeneficiaryBObj> _withinBankBeneficiaries = new ObservableCollection<BeneficiaryBObj>();

        public ObservableCollection<BeneficiaryBObj> WithinBankBeneficiaries
        {
            get => _withinBankBeneficiaries; 
            set =>  Set(ref _withinBankBeneficiaries, value);
        }

        private ObservableCollection<BeneficiaryBObj> _favouriteBeneficiaries = new ObservableCollection<BeneficiaryBObj>();

        public ObservableCollection<BeneficiaryBObj> FavouriteBeneficiaries
        {
                get => _favouriteBeneficiaries;
                set => Set(ref _favouriteBeneficiaries, value);
        }

        private ObservableCollection<BeneficiaryBObj> _otherBankBeneficiaries = new ObservableCollection<BeneficiaryBObj>();

        public ObservableCollection<BeneficiaryBObj> OtherBankBeneficiaries
        {
            get => _otherBankBeneficiaries; 
            set => Set(ref _otherBankBeneficiaries, value);
        }

        private class RemoveBeneficiaryPresenterCallback : IPresenterCallback<RemoveBeneficiaryResponse>
        {
            public BeneficiariesViewModel ViewModel { get; set; }

            public RemoveBeneficiaryPresenterCallback(BeneficiariesViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(RemoveBeneficiaryResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnBeneficiaryRemoved(response.RemovedBeneficiary);
                    
                });

                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = $"Beneficiary {response.RemovedBeneficiary.BeneficiaryName} Removed Successfully",
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
            private class UpdateBeneficiaryPresenterCallback : IPresenterCallback<UpdateBeneficiaryResponse>
            {
                public BeneficiariesViewModel ViewModel { get; set; }

                public UpdateBeneficiaryPresenterCallback(BeneficiariesViewModel viewModel)
                {
                    ViewModel = viewModel;
                }

            public async Task OnSuccess(UpdateBeneficiaryResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnCloseDialog();
                    ViewNotifier.Instance.OnBeneficiaryAddOrUpdated(response.UpdatedBeneficiary, false);

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
                    ViewNotifier.Instance.OnRequestFailed(true);

                });
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

            private class GetAllBeneficiariesPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
            {
                public BeneficiariesViewModel ViewModel { get; set; }

                public GetAllBeneficiariesPresenterCallback(BeneficiariesViewModel viewModel)
                {
                    ViewModel = viewModel;
                }

                public async Task OnSuccess(GetAllBeneficiariesResponse response)
                {
                    await ViewModel.View.Dispatcher.CallOnUIThreadAsync(()=>
                    {
                        BeneficiaryListUpdatedArgs args = new BeneficiaryListUpdatedArgs()
                        {
                            BeneficiaryList = response.Beneficiaries
                        };
                        ViewNotifier.Instance.OnBeneficiaryListUpdated(args);
                    });
                }

                public async Task OnFailure(ZBankException exception)
                {
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
