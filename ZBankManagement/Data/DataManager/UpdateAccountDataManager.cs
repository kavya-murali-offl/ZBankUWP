using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;
using System;
using ZBank.Entity.EnumerationTypes;

namespace ZBankManagement.DataManager
{
    class UpdateAccountDataManager : IUpdateAccountDataManager
    {
        public UpdateAccountDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }


        public async Task UpdateAccount(UpdateAccountRequest request, IUseCaseCallback<UpdateAccountResponse> callback)
        {
            try
            {
                if(request.UpdatedAccount is TermDepositAccount)
                {
                    Account account = await DBHandler.GetAccountByAccountNumber(request.CustomerID, (request.UpdatedAccount as TermDepositAccount).RepaymentAccountNumber);
                    if(account != null)
                    {
                        await DBHandler.UpdateAccount(request.UpdatedAccount as TermDepositAccount);
                        UpdateAccountResponse response = new UpdateAccountResponse()
                        {
                            UpdatedAccount = request.UpdatedAccount
                        };
                        callback.OnSuccess(response);
                    }
                    else
                    {
                        ZBankException error = new ZBankException()
                        {
                            Type = ErrorType.UNKNOWN,
                            Message = "Please enter a valid Repayment Account Number",
                        };
                        callback.OnFailure(error);
                    }
                }
                else
                {
                    ZBankException error = new ZBankException()
                    {
                        Type = ErrorType.UNKNOWN,
                        Message = "Update denied",
                    };
                    callback.OnFailure(error);
                }
             
            }
            catch(Exception ex)
            {
                ZBankException error = new ZBankException()
                {
                    Type = ErrorType.UNKNOWN,
                    Message = ex.Message,
                };
                callback.OnFailure(error);
            }
        }
    }
}
