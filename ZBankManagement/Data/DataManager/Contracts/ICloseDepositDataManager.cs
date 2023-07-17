using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Data.DataManager.Contracts
{
    interface ICloseDepositDataManager
    {
        Task CloseAllMaturedDeposits(CloseDepositRequest request, IUseCaseCallback<CloseDepositResponse> callback);

        Task CloseDeposit(CloseDepositRequest request, IUseCaseCallback<CloseDepositResponse> callback);

    }
}
