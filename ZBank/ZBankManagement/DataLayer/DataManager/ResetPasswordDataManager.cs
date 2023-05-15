using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.DatabaseHandler;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCard;

namespace ZBank.ZBankManagement.DataLayer.DataManager
{
    public class ResetPasswordDataManager : IResetPasswordDataManager
    {

        public ResetPasswordDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public void ResetPassword(UpdateCustomerCredentialsRequest request, IUseCaseCallback<ResetPasswordResponse> callback)
        {
            int rowsModified = DBHandler.UpdateCredentials(request.CustomerCredentials).Result;
            if (rowsModified > 0)
            {
                ResetPasswordResponse response = new ResetPasswordResponse();
                response.IsSuccess = true;
                callback.OnSuccess(response);
            }
            else
            {
                ZBankException error = new ZBankException();
                error.Message = "Error in fetching data";
                error.Type = ErrorType.UNKNOWN;
            }
        }
    }
}
