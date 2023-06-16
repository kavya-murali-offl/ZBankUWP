using ZBankManagement.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBank.ZBankManagement.DataLayer.DataManager.Contracts
{
    interface IInsertBeneficiaryDataManager
    {
        void InsertBeneficiary(InsertBeneficiaryRequest request, IUseCaseCallback<InsertBeneficiaryResponse> callback);
    }
}
