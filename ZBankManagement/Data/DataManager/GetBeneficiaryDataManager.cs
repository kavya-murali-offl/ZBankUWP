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

namespace ZBank.ZBankManagement.DataLayer.DataManager
{
    public class GetBeneficiaryDataManager : IGetBeneficiaryDataManager
    {
        private IDBHandler DBHandler { get; set; }

        public GetBeneficiaryDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        public async Task GetAllBeneficiariesByUserID(GetAllBeneficiariesRequest request, IUseCaseCallback<GetAllBeneficiariesResponse> callback)
        {
            try
            {
                IEnumerable<Beneficiary> beneficiaries = await DBHandler.GetBeneficiaries(request.UserID);
                GetAllBeneficiariesResponse response = new GetAllBeneficiariesResponse()
                {
                    Beneficiaries = beneficiaries
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
    }
}
