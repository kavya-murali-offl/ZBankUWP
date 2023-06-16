using ZBankManagement.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBank.ZBankManagement.DataLayer.DataManager.Contracts
{
    interface IDeleteBeneficiaryDataManager
    {
        void DeleteBeneficiary(RemoveBeneficiaryRequest request, IUseCaseCallback<RemoveBeneficiaryResponse> callback);

    }
}
