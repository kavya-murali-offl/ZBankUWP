using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace ZBankManagement.Domain.UseCase
{
    public abstract class UseCaseBase<TResponse>
    {

        IPresenterCallback<TResponse> _presenterCallback;   

        protected virtual bool GetIfAvailableInCache()
        {
            return false;
        }


        public void Execute()
        {

            if (GetIfAvailableInCache()) return;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(5000);

            Task.Run(() =>
            {
                try
                {
                    Action();
                }
                catch (TaskCanceledException taskCancelledException)
                {

                }
                catch (Exception ex)
                {
                    ZBankError errObj = new ZBankError();
                    _presenterCallback?.OnFailure(errObj);
                }
            }, cancellationTokenSource.Token);
        }

        protected abstract void Action();
    }
}
