using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.DatabaseHandler;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Data.DataManager
{
    internal class DeleteBeneficiaryDataManager : IDeleteBeneficiaryDataManager
    {
        public DeleteBeneficiaryDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task DeleteBeneficiary(RemoveBeneficiaryRequest request, IUseCaseCallback<RemoveBeneficiaryResponse> callback)
        {
            try
            {
                int rowsModified = await DBHandler.DeleteBeneficiary(request.BeneficiaryToRemove);
                if(rowsModified > 0)
                {
                    RemoveBeneficiaryResponse response = new RemoveBeneficiaryResponse()
                    {
                        RemovedBeneficiary = request.BeneficiaryToRemove,
                    };
                    callback.OnSuccess(response);
                }
                else
                {
                    ZBankException error = new ZBankException()
                    {
                        Type = ErrorType.UNKNOWN,
                        Message = "Failure in deleting beneficiary",
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
