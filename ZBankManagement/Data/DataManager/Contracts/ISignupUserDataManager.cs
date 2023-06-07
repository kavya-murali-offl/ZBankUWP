using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ZBankManagement.DataLayer.DataManager.Contracts
{
    public interface ISignupUserDataManager
    {
        Task SignupUser(InsertCustomerRequest request, IUseCaseCallback<SignupUserResponse> callback);
    }
}
