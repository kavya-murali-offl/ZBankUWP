using BankManagementDB.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.ZBankManagement.UseCase.GetAllAccounts
{
    public class GetAllAccountsCallback
    {

        IPresenterCallback<GetAllAccountsResponse> _presenterCallback;  

        public GetAllAccountsCallback(IPresenterCallback<GetAllAccountsResponse> presenterCallback) {

            _presenterCallback = presenterCallback;
        }

        public void OnSuccess(GetAllAccountsResponse response)
        {
            _presenterCallback.OnSuccess(response);
        }

        public void OnFailure(ZBankError error)
        {
            _presenterCallback.OnFailure(error);
        }
    }
}
