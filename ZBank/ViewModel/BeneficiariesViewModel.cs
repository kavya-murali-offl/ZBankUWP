using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.DataStore;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Entities.BusinessObjects;
using ZBankManagement.Entity.EnumerationTypes;

namespace ZBank.ViewModel
{
    public class BeneficiariesViewModel : ViewModelBase
    {
        private IView View { get; set; }

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

        private ObservableCollection<BeneficiaryBObj> _beneficiariesList { get; set; }

        public ObservableCollection<BeneficiaryBObj> BeneficiariesList
        {
            get { return _beneficiariesList; }
            set
            {
                _beneficiariesList = value;
                OnPropertyChanged(nameof(BeneficiariesList));
            }
        }

        private ObservableCollection<BeneficiaryBObj> _withinBankBeneficiaries { get; set; }

        public ObservableCollection<BeneficiaryBObj> WithinBankBeneficiaries
        {
            get { return _withinBankBeneficiaries; }
            set
            {
                _withinBankBeneficiaries = value;
                OnPropertyChanged(nameof(WithinBankBeneficiaries));
            }
        }

        private ObservableCollection<BeneficiaryBObj> _favouriteBeneficiaries { get; set; }

        public ObservableCollection<BeneficiaryBObj> FavouriteBeneficiaries
        {
            get { return _favouriteBeneficiaries; }
            set
            {
                _favouriteBeneficiaries = value;
                OnPropertyChanged(nameof(FavouriteBeneficiaries));
            }
        }

        private ObservableCollection<BeneficiaryBObj> _otherBankBeneficiaries { get; set; }

        public ObservableCollection<BeneficiaryBObj> OtherBankBeneficiaries
        {
            get { return _otherBankBeneficiaries; }
            set
            {
                _otherBankBeneficiaries = value;
                OnPropertyChanged(nameof(OtherBankBeneficiaries));
            }
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
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnBeneficiaryRemoved(response.RemovedBeneficiary);
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = $"Beneficiary {response.RemovedBeneficiary.BeneficiaryName} Removed Successfully",
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
                public BeneficiariesViewModel ViewModel { get; set; }

                public UpdateBeneficiaryPresenterCallback(BeneficiariesViewModel viewModel)
                {
                    ViewModel = viewModel;
                }

                public async Task OnSuccess(UpdateBeneficiaryResponse response)
                {
                    await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        ViewNotifier.Instance.OnCloseDialog(true);
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

            private class GetAllBeneficiariesPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
            {
                public BeneficiariesViewModel ViewModel { get; set; }

                public GetAllBeneficiariesPresenterCallback(BeneficiariesViewModel viewModel)
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
