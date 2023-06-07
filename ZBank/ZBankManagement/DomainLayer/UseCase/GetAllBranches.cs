using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using ZBank.ViewModel;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class GetAllBranchesUseCase : UseCaseBase<GetAllBranchesResponse>
    {

        private readonly IGetBranchDetailsDataManager _getBranchDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetBranchDetailsDataManager>();
        private readonly GetAllBranchesRequest _request;

        public GetAllBranchesUseCase(GetAllBranchesRequest request, IPresenterCallback<GetAllBranchesResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _request = request;
        }


        protected override void Action()
        {
            _getBranchDataManager.GetBranchList(_request, new GetAllBranchesCallback(this));
        }

        private class GetAllBranchesCallback : IUseCaseCallback<GetAllBranchesResponse>
        {
            private readonly UseCaseBase<GetAllBranchesResponse> _useCase;

            public GetAllBranchesCallback(GetAllBranchesUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetAllBranchesResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class GetAllBranchesRequest : RequestObjectBase
    {
    }


    public class GetAllBranchesResponse
    {
        public IEnumerable<Branch> BranchList { get; set; }
    }


    public class GetAllBranchesPresenterCallback : IPresenterCallback<GetAllBranchesResponse>
    {
        private AddOrEditAccountViewModel ViewModel { get; set; }

        public GetAllBranchesPresenterCallback(AddOrEditAccountViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public async void OnSuccess(GetAllBranchesResponse response)
        {
            await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                BranchListUpdatedArgs args = new BranchListUpdatedArgs()
                {
                    BranchList = response.BranchList
                };
                ViewNotifier.Instance.OnBranchListUpdated(args);
            });
        }

        public void OnFailure(ZBankException response)
        {
        }

    }
}
