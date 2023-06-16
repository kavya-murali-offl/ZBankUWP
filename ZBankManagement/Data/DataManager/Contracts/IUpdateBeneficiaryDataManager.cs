using ZBankManagement.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBank.ZBankManagement.DataLayer.DataManager.Contracts
{
    interface IUpdateBeneficiaryDataManager
    {
        void UpdateBeneficiary(UpdateBeneficiaryRequest request, IUseCaseCallback<UpdateBeneficiaryResponse> callback);
    }
}
