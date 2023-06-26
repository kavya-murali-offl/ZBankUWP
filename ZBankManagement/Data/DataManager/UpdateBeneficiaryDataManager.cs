using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.DatabaseHandler;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Data.DataManager
{
   class UpdateBeneficiaryDataManager
    {
        public UpdateBeneficiaryDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task UpdateBeneficiary(UpdateBeneficiaryRequest request, IUseCaseCallback<UpdateBeneficiaryResponse> callback)
        {
            try
            {
                int rowsModified = await DBHandler.AddBeneficiary(request.BeneficiaryToUpdate);
                if (rowsModified > 0)
                {
                    UpdateBeneficiaryResponse response = new UpdateBeneficiaryResponse
                    {
                        UpdatedBeneficiary = request.BeneficiaryToUpdate
                    };
                    callback.OnSuccess(response);
                }
                else
                {
                    ZBankException error = new ZBankException();
                    error.Message = "Beneficiary not updated";
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
