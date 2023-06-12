using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.EnumerationType;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.Interface
{
    public interface IGetAccountDataManager
    {
        Task GetAllAccounts(GetAllAccountsRequest request, IUseCaseCallback<GetAllAccountsResponse> callback);
    }
}
