using ZBank.Entities;
using System.Collections.Generic;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using System.Threading.Tasks;

namespace ZBankManagement.Interface
{
    public interface IGetTransactionDataManager
    {
        Task GetTransactionsByAccountNumber(GetAllTransactionsRequest request, IUseCaseCallback<GetAllTransactionsResponse> callback);

        Task GetTransactionsByCustomerID(GetAllTransactionsRequest request, IUseCaseCallback<GetAllTransactionsResponse> callback);

    }
}
