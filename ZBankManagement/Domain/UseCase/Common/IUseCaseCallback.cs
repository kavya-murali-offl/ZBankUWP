
namespace ZBankManagement.Domain.UseCase
{
    public interface IUseCaseCallback<TResponse>
    {
        void OnSuccess(TResponse result);

        void OnFailure(ZBankException error);
    }
}
