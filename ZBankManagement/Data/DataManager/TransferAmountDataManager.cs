using ZBankManagement.Interface;
using System;
using System.Collections.Generic;
using ZBank.DatabaseHandler;
using System.Threading.Tasks;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.EnumerationTypes;
using ZBank.Entities;
using System.Linq;
using ZBank.Entities.EnumerationType;

namespace ZBankManagement.DataManager
{
    class TransferAmountDataManager : ITransferAmountDataManager
    {
        public TransferAmountDataManager(IDBHandler dBHandler)
        {
            _dBHandler = dBHandler;
        }

        private IDBHandler _dBHandler { get; set; }

        public async Task InitiateOtherBankTransaction(TransferAmountRequest request, IUseCaseCallback<TransferAmountResponse> callback)
        {
            try
            {
                bool validated = false;
                if (request.OwnerAccount.AccountType == AccountType.CURRENT || request.OwnerAccount.AccountType == AccountType.SAVINGS)
                {
                    Account ownerAccount = await _dBHandler.GetAccount(request.CustomerID, request.OwnerAccount.AccountNumber).ConfigureAwait(false);
                    if(ownerAccount != null)
                    {
                        if(ownerAccount.Balance < request.Transaction.Amount)
                        {
                            IEnumerable<TransactionBObj> transactionsMadeToday = await _dBHandler.FetchAllTodayTransactions(request.OwnerAccount.AccountNumber, request.CustomerID).ConfigureAwait(false);
                            var amountTransacted = transactionsMadeToday.Sum(x => x.Amount);
                            decimal limit = 0;
                            if (request.OwnerAccount is CurrentAccount)
                            {
                                limit = ( request.OwnerAccount as CurrentAccount ).TransactionLimit;

                            }
                            else if (request.OwnerAccount is SavingsAccount)
                            {
                                limit = ( request.OwnerAccount as SavingsAccount ).TransactionLimit;
                            }

                            if (amountTransacted + request.Transaction.Amount > limit)
                            {
                                ZBankException error = new ZBankException()
                                {
                                    Type = ErrorType.UNKNOWN,
                                    Message = "Daily Transaction Limit Exceeded. Try again later",
                                };
                                callback.OnFailure(error);
                            }
                            else
                            {
                                validated = true;
                            }
                        }
                        else
                        {
                            ZBankException error = new ZBankException()
                            {
                                Type = ErrorType.UNKNOWN,
                                Message = "Insufficient Balance",
                            };
                            callback.OnFailure(error);
                        }
                    }
                }
                else
                {
                    validated = true;
                }
                if (validated)
                {
                    var metaData = new TransactionMetaData()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ReferenceID = request.Transaction.ReferenceID,
                        AccountNumber = request.OwnerAccount.AccountNumber,
                        ClosingBalance = request.OwnerAccount.Balance -= request.Transaction.Amount,
                    };

                    await _dBHandler.InitiateTransactionExternal(request.Transaction, request.OwnerAccount, metaData).ConfigureAwait(false);
                    TransferAmountResponse response = new TransferAmountResponse()
                    {
                    };
                    callback.OnSuccess(response);
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

        public async Task GetBeneficiaryAccount(TransferAmountRequest request, IUseCaseCallback<GetBeneficiaryAccountResponse> callback)
        {
            try
            {
                Account account = await _dBHandler.GetAccount(request.CustomerID, request.Beneficiary.AccountNumber).ConfigureAwait(false);
                GetBeneficiaryAccountResponse response = new GetBeneficiaryAccountResponse()
                {
                    Account = account
                };
                callback.OnSuccess(response);
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

        public async Task InitiateWithinBankTransaction(TransferAmountRequest request, IUseCaseCallback<TransferAmountResponse> callback)
        {
            try
            {
                Account otherAccount =
                    request.OtherAccount != null ? request.OtherAccount : 
                    await _dBHandler.GetAccount(request.CustomerID, request.Beneficiary.AccountNumber).ConfigureAwait(false);
                
                if (otherAccount != null)
                {
                    var metaData = new TransactionMetaData()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ReferenceID = request.Transaction.ReferenceID,
                        AccountNumber = request.OwnerAccount.AccountNumber,
                        ClosingBalance = request.OwnerAccount.Balance -= request.Transaction.Amount,
                    };

                    var otherMetaData = new TransactionMetaData()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ReferenceID = request.Transaction.ReferenceID,
                        AccountNumber = otherAccount.AccountNumber,
                        ClosingBalance = otherAccount.Balance += request.Transaction.Amount,
                    };
                    await _dBHandler.InitiateTransactionInternal(request.OwnerAccount, otherAccount, request.Transaction, metaData, otherMetaData).ConfigureAwait(false);
                    TransferAmountResponse response = new TransferAmountResponse()
                    {
                        Transaction = request.Transaction,
                    };
                    callback.OnSuccess(response);
                }
                else
                {
                    ZBankException error = new ZBankException()
                    {
                        Type = ErrorType.UNKNOWN,
                        Message = "Account Not Found",
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
