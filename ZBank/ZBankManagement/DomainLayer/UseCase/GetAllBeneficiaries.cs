using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.ViewModel;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class GetAllBeneficiariesUseCase : UseCaseBase<GetAllBeneficiariesResponse>
    {

        private readonly IGetBeneficiaryDataManager _getBeneficiaryDataManager;
        private readonly GetAllBeneficiariesRequest _request;

        public GetAllBeneficiariesUseCase(GetAllBeneficiariesRequest request, IPresenterCallback<GetAllBeneficiariesResponse> presenterCallback) 
            : base(presenterCallback, request.Token)
        {
            _getBeneficiaryDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetBeneficiaryDataManager>();
            _request = request;
        }


        protected override void Action()
        {
            _getBeneficiaryDataManager.GetAllBeneficiariesByUserID(_request, new GetAllBeneficiariesCallback(this));
        }

        private class GetAllBeneficiariesCallback : IUseCaseCallback<GetAllBeneficiariesResponse>
        {

            private readonly GetAllBeneficiariesUseCase _useCase;

            public GetAllBeneficiariesCallback(GetAllBeneficiariesUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetAllBeneficiariesResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class GetAllBeneficiariesRequest : RequestObjectBase
    {
        public string UserID { get; set; }
    }


    public class GetAllBeneficiariesResponse
    {
        public IEnumerable<Beneficiary> Beneficiaries { get; set; }
    }


    public class GetAllBeneficiariesPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
    {

        public void OnSuccess(GetAllBeneficiariesResponse response)
        {
        }

        public void OnFailure(ZBankException response)
        {
        }
    }

    public class GetAllBeneficiariesInAccountPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
    {
        public AccountInfoViewModel ViewModel { get; set; }

        public GetAllBeneficiariesInAccountPresenterCallback(AccountInfoViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public async void OnSuccess(GetAllBeneficiariesResponse response)
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

        public void OnFailure(ZBankException response)
        {
        }
    }

    public class GetAllBeneficiariesInTransactionsPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
    {
        public TransactionViewModel ViewModel { get; set; }

        public GetAllBeneficiariesInTransactionsPresenterCallback(TransactionViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public async void OnSuccess(GetAllBeneficiariesResponse response)
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

        public void OnFailure(ZBankException response)
        {
        }
    }

    public class GetAllBeneficiariesInDashboardPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
    {

        private readonly DashboardViewModel _dashboardViewModel;

        public void OnSuccess(GetAllBeneficiariesResponse response)
        {
        }

        public void OnFailure(ZBankException response)
        {
        }
    }
}
