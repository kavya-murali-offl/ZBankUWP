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
                Account repaymentAccount = await DBHandler.GetAccount(request.CustomerID, request.DepositAccount.RepaymentAccountNumber).ConfigureAwait(false);
                if (repaymentAccount != null)
                {
                    await CloseDeposit(request.DepositAccount, repaymentAccount);

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

        private async Task CloseDeposit(TermDepositAccount depositAccount, Account repaymentAccount)
        {
            decimal totalAmount = depositAccount.CalculateClosingAmount(DateTime.Now);

            Transaction transaction = new Transaction()
            {
                Amount = totalAmount,
                Description = $"FD Account {depositAccount.AccountNumber} Closed",
                RecipientAccountNumber = repaymentAccount.AccountNumber,
                RecordedOn = DateTime.Now,
                ReferenceID = Guid.NewGuid().ToString(),
                SenderAccountNumber = depositAccount.AccountNumber,
                TransactionType = TransactionType.TRANSFER
            };

            depositAccount.IFSCCode = depositAccount.IfscCode;
            depositAccount.AccountStatus = AccountStatus.CLOSED;
            depositAccount.MaturityDate = DateTime.Now;
            depositAccount.MaturityAmount = totalAmount;
            depositAccount.Balance = 0m;

            repaymentAccount.Balance += totalAmount;

            await DBHandler.CloseDeposit(depositAccount, repaymentAccount, transaction).ConfigureAwait(false);
        }

        public async Task CloseAllMaturedDeposits(CloseDepositRequest request, IUseCaseCallback<CloseDepositResponse> callback)
        {
            try
            {
                IEnumerable<TermDepositAccount>  accounts = await DBHandler.GetAllDepositAccounts();
                foreach (var account in accounts)
                {
                    if (account.MaturityDate.Date > DateTime.Now.Date && account.AccountStatus != AccountStatus.CLOSED)
                    {
                        Account repaymentAccount = await DBHandler.GetAccount(null, account.RepaymentAccountNumber).ConfigureAwait(false);
                        if (repaymentAccount != null)
                        {
                            await CloseDeposit(account, repaymentAccount);
                        }
                    }
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
