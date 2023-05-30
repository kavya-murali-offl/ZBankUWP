using System;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.ViewModel;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using System.Collections.Generic;
using ZBank.Entity.BusinessObjects;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class GetDashboardDataUseCase : UseCaseBase<GetDashboardDataResponse>
    {

        private readonly IGetDashboardDataDataManager _getDashboardDataDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetDashboardDataDataManager>();
        private readonly GetDashboardDataRequest _request;

        public GetDashboardDataUseCase(GetDashboardDataRequest request, IPresenterCallback<GetDashboardDataResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _request = request;
        }


        protected override void Action()
        {
            _getDashboardDataDataManager.GetDashboardData(_request, new GetDashboardDataCallback(this));
        }

        private class GetDashboardDataCallback : IUseCaseCallback<GetDashboardDataResponse>
        {
            private readonly UseCaseBase<GetDashboardDataResponse> _useCase;

            public GetDashboardDataCallback(GetDashboardDataUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetDashboardDataResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class GetDashboardDataRequest : RequestObjectBase
    {
        public string UserID { get; set; }
    }

    public class GetDashboardDataResponse
    {
        public IEnumerable<CardBObj> AllCards { get; set; }
        public IEnumerable<Beneficiary> Beneficiaries { get; set; }
        public IEnumerable<TransactionBObj> LatestTransactions { get; set; }
        public DashboardCardModel BeneficiariesCard { get; set; }
        public DashboardCardModel DepositCard { get; set; }
        public DashboardCardModel IncomeExpenseCard { get; set; }
        public DashboardCardModel BalanceCard { get; set; }
        public IEnumerable<AccountBObj> Accounts { get; set; }
    }


    public class GetDashboardDataPresenterCallback : IPresenterCallback<GetDashboardDataResponse>
    {
        private DashboardViewModel DashboardViewModel { get; set; }

        public GetDashboardDataPresenterCallback(DashboardViewModel dashboardViewModel)
        {
            DashboardViewModel = dashboardViewModel;
        }

        public async void OnSuccess(GetDashboardDataResponse response)
        {
            await DashboardViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                DashboardDataUpdatedArgs args = new DashboardDataUpdatedArgs()
                {
                    AllCards = response.AllCards,
                    DepositCard = response.DepositCard,
                    AllBeneficiaries = response.Beneficiaries,
                    BeneficiariesCard = response.BeneficiariesCard,
                    AllAccounts = response.Accounts,
                    BalanceCard = response.BalanceCard,
                    IncomeExpenseCard = response.IncomeExpenseCard,
                    LatestTransactions = response.LatestTransactions,
                };

                ViewNotifier.Instance.OnDashboardDataChanged(args);
            });
        }

        public void OnFailure(ZBankException response)
        {

        }
    }
}
