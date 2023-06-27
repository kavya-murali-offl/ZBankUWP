using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
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
                UserID = "1111"
            };

            IPresenterCallback<GetAllBeneficiariesResponse> presenterCallback = new GetAllBeneficiariesPresenterCallback(this);
            UseCaseBase<GetAllBeneficiariesResponse> useCase = new GetAllBeneficiariesUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public void OnLoaded()
        {
            ViewNotifier.Instance.BeneficiaryListUpdated += UpdateBeneficiaryList;
            ViewNotifier.Instance.BeneficiaryAddOrUpdated += OnBeneficiaryAddedOrUpdated;
            LoadAllBeneficiaries();

        }

        private void UpdateBeneficiaryList(BeneficiaryListUpdatedArgs args)
        {
            BeneficiariesList = new ObservableCollection<BeneficiaryBObj>(args.BeneficiaryList);
            OtherBankBeneficiaries = new ObservableCollection<BeneficiaryBObj>(args.BeneficiaryList.Where(ben => ben.BeneficiaryType == BeneficiaryType.OTHER_BANK));
            WithinBankBeneficiaries = new ObservableCollection<BeneficiaryBObj>(args.BeneficiaryList.Where(ben => ben.BeneficiaryType == BeneficiaryType.WITHIN_BANK));
        }

        public void OnUnloaded()
        {
            ViewNotifier.Instance.BeneficiaryListUpdated -= UpdateBeneficiaryList;
            ViewNotifier.Instance.BeneficiaryAddOrUpdated -= OnBeneficiaryAddedOrUpdated;
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
