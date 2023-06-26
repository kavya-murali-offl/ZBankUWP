using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using static ZBankManagement.Domain.UseCase.ResetPin;

namespace ZBankManagement.Data.DataManager.Contracts
{
    interface IResetPinDataManager
    {
        Task ResetPin(ResetPinRequest request, IUseCaseCallback<ResetPinResponse> callback);
    }
}
