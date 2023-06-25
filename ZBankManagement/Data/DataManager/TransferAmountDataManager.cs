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
                var metaData = new TransactionMetaData()
                {
                    ID = Guid.NewGuid().ToString(),
                    ReferenceID = request.Transaction.ReferenceID,
                    AccountNumber = request.OwnerAccount.AccountNumber,
                    ClosingBalance = request.OwnerAccount.Balance -= request.Transaction.Amount,
                };
                await _dBHandler.InitiateTransactionExternal(request.Transaction, request.OwnerAccount, metaData);
                TransferAmountResponse response = new TransferAmountResponse()
                {
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

        public async Task GetBeneficiaryAccount(TransferAmountRequest request, IUseCaseCallback<GetBeneficiaryAccountResponse> callback)
        {
            try
            {
                Account account = await _dBHandler.GetAccountByAccountNumber(request.Beneficiary.AccountNumber);
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
                Account beneficiaryAccount = await _dBHandler.GetAccountByAccountNumber(request.Beneficiary.AccountNumber);
                if (beneficiaryAccount != null)
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
                        AccountNumber = beneficiaryAccount.AccountNumber,
                        ClosingBalance = beneficiaryAccount.Balance += request.Transaction.Amount,
                    };

                    await _dBHandler.InitiateTransactionInternal(request.OwnerAccount, beneficiaryAccount, request.Transaction, metaData, otherMetaData);
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
