using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Data.DataManager.Contracts
{
    class IPayCreditCardDataManager
    {
        internal void PayCreditCard(PayCreditCardRequest request, IUseCaseCallback<PayCreditCardResponse> payCreditCardCallback)
        {
            throw new NotImplementedException();
        }
    }
}
