using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Interface
{
    interface ITransferAmountDataManager
    {
        Task InitiateWithinBankTransaction(TransferAmountRequest request, IUseCaseCallback<TransferAmountResponse> callback);
        Task InitiateOtherBankTransaction(TransferAmountRequest request, IUseCaseCallback<TransferAmountResponse> callback);
        Task GetBeneficiaryAccount(TransferAmountRequest request, IUseCaseCallback<GetBeneficiaryAccountResponse> callback);
    }
}
