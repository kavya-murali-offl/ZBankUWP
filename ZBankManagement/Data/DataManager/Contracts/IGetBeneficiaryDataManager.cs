using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ZBankManagement.DataLayer.DataManager.Contracts
{
    public interface IGetBeneficiaryDataManager
    {
       Task GetAllBeneficiariesByUserID(GetAllBeneficiariesRequest request, IUseCaseCallback<GetAllBeneficiariesResponse> callback);

    }
}
