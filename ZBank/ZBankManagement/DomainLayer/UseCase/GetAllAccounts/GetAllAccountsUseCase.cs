using BankManagementDB.DataManager;
using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;

namespace ZBank.ZBankManagement.UseCase.GetAllAccounts
{
    public class GetAllAccountsUseCase : UseCaseBase<GetAllAccountsRequest, GetAllAccountsResponse>
    {
        private readonly IGetAccountDataManager GetAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetAccountDataManager>();

        protected override void Action(GetAllAccountsRequest request , IPresenterCallback<GetAllAccountsResponse> presenterCallback)
        {
            GetAccountDataManager.GetAllAccounts(request, new GetAllAccountsCallback(presenterCallback));
        }
    }
}
