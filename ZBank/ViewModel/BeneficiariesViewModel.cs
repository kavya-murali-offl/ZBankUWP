using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ViewModel
{
    public class BeneficiariesViewModel : ViewModelBase
    {
        private IView View { get; set; }

        public BeneficiariesViewModel(IView view)
        {
            View = view;
            LoadAllBeneficiaries();
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
        }

        private void UpdateBeneficiaryList(BeneficiaryListUpdatedArgs args)
        {
            BeneficiariesList = new ObservableCollection<Beneficiary>(args.BeneficiaryList);
        }

        public void OnUnloaded()
        {
            ViewNotifier.Instance.BeneficiaryListUpdated -= UpdateBeneficiaryList;
        }

        private ObservableCollection<Beneficiary> _beneficiariesList { get; set; }

        public ObservableCollection<Beneficiary> BeneficiariesList
        {
            get { return _beneficiariesList; }
            set
            {
                _beneficiariesList = value;
                OnPropertyChanged(nameof(BeneficiariesList));
            }
        }

        private class InsertBeneficiaryPresenterCallback : IPresenterCallback<InsertBeneficiaryResponse>
        {
            public BeneficiariesViewModel ViewModel { get; set; }

            public InsertBeneficiaryPresenterCallback(BeneficiariesViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(InsertBeneficiaryResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
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
            public BeneficiariesViewModel ViewModel { get; set; }

            public UpdateBeneficiaryPresenterCallback(BeneficiariesViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(UpdateBeneficiaryResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
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

            public async Task OnFailure(ZBankException response)
            {

            }
        }



    }
}
