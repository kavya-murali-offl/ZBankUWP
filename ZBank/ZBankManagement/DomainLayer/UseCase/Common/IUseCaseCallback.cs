
namespace BankManagementDB.Domain.UseCase
{
    public interface IUseCaseCallback<T>
    {
        void OnSuccess(T result);

        void OnFailure(ZBankError error);
    }
}
