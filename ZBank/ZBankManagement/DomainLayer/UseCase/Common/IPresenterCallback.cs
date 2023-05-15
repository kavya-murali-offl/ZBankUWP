using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBankManagement.Domain.UseCase
{
    public interface IPresenterCallback<TResponse>
    {
        void OnSuccess(TResponse result);

        void OnFailure(ZBankException exception);

    }
}
