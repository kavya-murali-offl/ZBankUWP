using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ZBankManagement.Domain.UseCase.ResetPin;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.DatabaseHandler;
using ZBankManagement.Data.DataManager.Contracts;
using ZBank.Entity.EnumerationTypes;
using System.ServiceModel.Channels;

namespace ZBankManagement.Data.DataManager
{
    class ResetPinDataManager : IResetPinDataManager
    {
        public ResetPinDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }

        public async Task ResetPin(ResetPinRequest request, IUseCaseCallback<ResetPinResponse> callback)
        {
            try
            {
                int rowsModified = await DBHandler.ResetPin(request.CardNumber, request.NewPin).ConfigureAwait(false);

                if (rowsModified > 0)
                {
                    ResetPinResponse response = new ResetPinResponse();
                    response.IsSuccess = rowsModified > 0;
                    callback.OnSuccess(response);
                }
                else
                {
                    ZBankException error = new ZBankException();
                    error.Type = ZBank.Entity.EnumerationTypes.ErrorType.UNKNOWN;
                    callback.OnFailure(error);
                }
            }
            catch (Exception ex)
            {
                ZBankException error = new ZBankException()
                {
                    Type = ErrorType.UNKNOWN,
                    Message = ex.Message
                };
                callback.OnFailure(error);
            }
            
        }

    }
}
