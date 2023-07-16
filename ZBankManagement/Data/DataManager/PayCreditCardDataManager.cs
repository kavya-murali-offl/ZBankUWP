using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.DatabaseHandler;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Data.DataManager.Contracts;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Data.DataManager
{
    internal class PayCreditCardDataManager : IPayCreditCardDataManager
    {

        public PayCreditCardDataManager(IDBHandler dBHandler)
        {
            _dBHandler = dBHandler;
        }

        private IDBHandler _dBHandler { get; set; }

        public async Task PayCreditCardBill(PayCreditCardRequest request, IUseCaseCallback<PayCreditCardResponse> callback)
        {
            try
            {
                bool validated = false;
                if (request.PaymentAccount.AccountType == AccountType.CURRENT || request.PaymentAccount.AccountType == AccountType.SAVINGS)
                {
                    Account ownerAccount = await _dBHandler.GetAccountByAccountNumber(request.CustomerID, request.PaymentAccount.AccountNumber);
                    if (ownerAccount.Balance > request.PaymentAmount)
                    {
                        IEnumerable<TransactionBObj> transactionsMadeToday = await _dBHandler.FetchAllTodayTransactions(request.PaymentAccount.AccountNumber, request.CustomerID);
                        var amountTransacted = transactionsMadeToday.Sum(x => x.Amount);
                        decimal limit = 0;
                        if (request.PaymentAccount is CurrentAccount)
                        {
                            limit = (request.PaymentAccount as CurrentAccount).TransactionLimit;

                        }
                        else if (request.PaymentAccount is SavingsAccount)
                        {
                            limit = (request.PaymentAccount as SavingsAccount).TransactionLimit;
                        }

                        if (amountTransacted + request.PaymentAmount > limit)
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
                            Message = "Insufficient Balance"
                        };
                        callback.OnFailure(error);
                    }
                }

                if (validated)
                {
                    Transaction transaction = new Transaction()
                    {
                        ReferenceID = Guid.NewGuid().ToString(),
                        Amount = request.PaymentAmount,
                        Description = $"Bill Payment of Credit Card ending with {request.CreditCard.CardNumber.Substring(request.CreditCard.CardNumber.Length - 4)}",
                        RecipientAccountNumber = request.CreditCard.CardNumber,
                        RecordedOn = DateTime.Now,
                        SenderAccountNumber = request.PaymentAccount.AccountNumber,
                        TransactionType = TransactionType.CARD_PAYMENT
                    };
                    var metaData = new TransactionMetaData()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ReferenceID = transaction.ReferenceID,
                        AccountNumber = request.PaymentAccount.AccountNumber,
                        ClosingBalance = request.PaymentAccount.Balance -= request.PaymentAmount,
                    };

                    request.CreditCard.TotalOutstanding -= request.PaymentAmount;

                    await _dBHandler.PayCreditCardBill(transaction, request.PaymentAccount, metaData, request.CreditCard);
                    PayCreditCardResponse response = new PayCreditCardResponse()
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

    }
}
