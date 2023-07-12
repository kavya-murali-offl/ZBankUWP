using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Services;
using ZBank.Config;
using ZBank.View;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI;
using static ZBankManagement.Domain.UseCase.InitializeApp;
using Windows.UI.Core;
using ZBankManagement.Domain.UseCase;
using Microsoft.Toolkit.Uwp;

namespace ZBank.ViewModel
{
    public class EntryPageViewModel :ViewModelBase
    {
        public EntryPageViewModel(IView view) 
        { 
            View = view; 
            ThemeService.Initialize();
        }

        internal void OnNavigatedTo()
        {
            InitializeAppData();
            EnterApplication();
        }

        

        private void EnterApplication()
        {
            string customerID = AppSettings.Current.CustomerID;
            if (!string.IsNullOrEmpty(customerID))
            {
                FrameContentChangedArgs args = new FrameContentChangedArgs()
                {
                    PageType = typeof(MainPage),
                    Params = new MainPageArgs()
                    {
                        CustomerID = customerID,
                    },
                    Title = "Dashboard".GetLocalized()
                };
                ViewNotifier.Instance.OnFrameContentChanged(args);
            }
        }

        private void InitializeAppData()
        {
            InitializeAppRequest request = new InitializeAppRequest();

            IPresenterCallback<InitializeAppResponse> presenterCallback = new InitializeAppPresenterCallback(this);
            UseCaseBase<InitializeAppResponse> useCase = new InitializeAppUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private class InitializeAppPresenterCallback : IPresenterCallback<InitializeAppResponse>
        {
            public EntryPageViewModel ViewModel { get; set; }

            public InitializeAppPresenterCallback(EntryPageViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(InitializeAppResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnLoadApp();
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                // Message dialog
            }
        }

    }
}
