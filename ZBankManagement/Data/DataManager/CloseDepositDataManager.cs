using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.Entities.EnumerationType;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Data.DataManager.Contracts;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Data.DataManager
{
    class CloseDepositDataManager : ICloseDepositDataManager
    {
        public CloseDepositDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task CloseDeposit(CloseDepositRequest request, IUseCaseCallback<CloseDepositResponse> callback)
        {
            try
            {
                Account repaymentAccount = await DBHandler.GetAccountByAccountNumber(request.CustomerID, request.DepositAccount.RepaymentAccountNumber).ConfigureAwait(false);
                if(repaymentAccount != null)
                {
                    decimal totalAmount = request.DepositAccount.CalculateClosingAmount(DateTime.Now);
                    Transaction transaction = new Transaction()
                    {
                        Amount = totalAmount,
                        Description = $"FD Account {request.DepositAccount.AccountNumber} Closed",
                        RecipientAccountNumber = repaymentAccount.AccountNumber,
                        RecordedOn = DateTime.Now,
                        ReferenceID = Guid.NewGuid().ToString(),
                        SenderAccountNumber = request.DepositAccount.AccountNumber,
                        TransactionType = TransactionType.TRANSFER
                    };

                    request.DepositAccount.IFSCCode = request.DepositAccount.IfscCode;
                    request.DepositAccount.AccountStatus = AccountStatus.CLOSED;
                    request.DepositAccount.MaturityDate = DateTime.Now;
                    request.DepositAccount.Balance = totalAmount;

                    repaymentAccount.Balance -= totalAmount;
                    await DBHandler.CloseDeposit(request.DepositAccount, repaymentAccount, transaction).ConfigureAwait(false);
                    CloseDepositResponse response = new CloseDepositResponse()
                    {
                        DepositAccount = request.DepositAccount,
                    };
                    callback.OnSuccess(response);
                }
                else
                {
                    ZBankException error = new ZBankException()
                    {
                        Type = ErrorType.UNKNOWN,
                        Message = "No account is associated with the provided Repayment Account Number",
                    };
                    callback.OnFailure(error);
                }
            }
            catch (Exception ex)
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
