using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Entities.BusinessObjects;
using ZBankManagement.Entity.EnumerationTypes;

namespace ZBankManagement.Data.DataManager
{
    class InsertBeneficiaryDataManager : IInsertBeneficiaryDataManager
    {
        public InsertBeneficiaryDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task InsertBeneficiary(InsertBeneficiaryRequest request, IUseCaseCallback<InsertBeneficiaryResponse> callback)
        {
            try
            {
                bool isIFSCCodeValidated = false;
                if (request.BeneficiaryToInsert.BeneficiaryType == BeneficiaryType.WITHIN_BANK)
                {
                    IEnumerable<Account> accounts = await DBHandler.GetIFSCCodeByAccountNumber(request.BeneficiaryToInsert.AccountNumber).ConfigureAwait(false);
                    if (accounts.Count() > 0)
                    {
                        isIFSCCodeValidated = true;
                    }
                }
                else if (request.BeneficiaryToInsert.BeneficiaryType == BeneficiaryType.OTHER_BANK)
                {
                    IEnumerable<ExternalAccount> accounts = 
                        await DBHandler.ValidateExternalAccount(request.BeneficiaryToInsert.AccountNumber, request.IFSCCode).ConfigureAwait(false);
                    if (accounts?.Count() > 0)
                    {
                        isIFSCCodeValidated = true;
                    }
                }

                if (isIFSCCodeValidated)
                {
                    IEnumerable<BeneficiaryBObj> beneficiaries = await DBHandler.GetBeneficiaries(request.BeneficiaryToInsert.UserID).ConfigureAwait(false);
                    var alreadyAddedBeneficiary = beneficiaries.Where(ben => ben.AccountNumber == request.BeneficiaryToInsert.AccountNumber);
                    if(alreadyAddedBeneficiary?.Count() > 0)
                    {
                        ZBankException error = new ZBankException();
                        error.Message = "Beneficiary already added";
                        error.Type = ErrorType.UNKNOWN;
                        callback.OnFailure(error);
                    }
                    else
                    {
                        int rowsModified = await DBHandler.AddBeneficiary(request.BeneficiaryToInsert).ConfigureAwait(false);
                        if (rowsModified > 0)
                        {
                            InsertBeneficiaryResponse response = new InsertBeneficiaryResponse
                            {
                                InsertedBeneficiary = request.BeneficiaryToInsert
                            };
                            callback.OnSuccess(response);
                        }
                        else
                        {
                            ZBankException error = new ZBankException();
                            error.Message = "Beneficiary not inserted";
                            error.Type = ErrorType.UNKNOWN;
                            callback.OnFailure(error);
                        }
                    }
                    
                }
                else
                {
                    ZBankException error = new ZBankException();
                    error.Message = "Account not found";
                    error.Type = ErrorType.UNKNOWN;
                    callback.OnFailure(error);
                }

            }
            catch (Exception err)
            {
                ZBankException error = new ZBankException();
                error.Message = err.Message;
                error.Type = ErrorType.DATABASE_ERROR;
                callback.OnFailure(error);
            }
        }

    }
}
