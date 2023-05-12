using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBankManagement.Domain.UseCase
{
    public interface IPresenterCallback<T>
    {
        void OnSuccess(T result);

        void OnFailure(ZBankError exception);

    }
}
