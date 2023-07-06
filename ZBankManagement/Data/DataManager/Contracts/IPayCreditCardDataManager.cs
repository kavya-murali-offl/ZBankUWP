using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Data.DataManager.Contracts
{
    interface IPayCreditCardDataManager
    {
        Task PayCreditCardBill(PayCreditCardRequest request, IUseCaseCallback<PayCreditCardResponse> callback);
    }
}
