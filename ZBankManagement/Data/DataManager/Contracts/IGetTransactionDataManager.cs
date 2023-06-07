using ZBank.Entities;
using System.Collections.Generic;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Interface
{
    public interface IGetTransactionDataManager
    {
        void GetTransactionsByAccountNumber(GetAllTransactionsRequest request, IUseCaseCallback<GetAllTransactionsResponse> callback);

        void GetTransactionsByCustomerID(GetAllTransactionsRequest request, IUseCaseCallback<GetAllTransactionsResponse> callback);

    }
}
