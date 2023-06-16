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
     class GetBranchDetailsDataManager : IGetBranchDetailsDataManager
    {

        public GetBranchDetailsDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task GetBranchList(GetAllBranchesRequest request, IUseCaseCallback<GetAllBranchesResponse> callback)
        {
            try
            {
                IEnumerable<Branch> branchList = await DBHandler.GetBranchDetails();
                GetAllBranchesResponse response = new GetAllBranchesResponse()
                {
                    BranchList = branchList
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
