using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace ZBankManagement.Domain.UseCase
{
    public abstract class UseCaseBase<TResponse>
    {
        public readonly IPresenterCallback<TResponse> PresenterCallback;
        private readonly CancellationToken _cancellationToken;

        protected UseCaseBase(IPresenterCallback<TResponse> callback, CancellationToken token)
        {
            PresenterCallback = callback;
            _cancellationToken = token;
        }

        protected virtual bool GetIfAvailableInCache()
        {
            return false;
        }


        public void Execute()
        {

            if (GetIfAvailableInCache()) return;

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
                    ZBankException errObj = new ZBankException();
                    PresenterCallback?.OnFailure(errObj);
                }
            }, _cancellationToken);
        }

        protected abstract void Action();
    }
}
