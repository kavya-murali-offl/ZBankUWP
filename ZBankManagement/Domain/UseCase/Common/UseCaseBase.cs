using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using ZBank.Entity.EnumerationTypes;

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
                    ZBankException errObj = new ZBankException();
                    errObj.Message = taskCancelledException.Message;
                    errObj.Type = ErrorType.ABORTED;
                    PresenterCallback?.OnFailure(errObj);
                }
                catch (Exception ex)
                {
                    ZBankException errObj = new ZBankException();
                    errObj.Message = ex.Message;
                    errObj.Type = ErrorType.UNKNOWN;
                    PresenterCallback?.OnFailure(errObj);
                }
            }, _cancellationToken);
        }

        protected abstract void Action();
    }
}
