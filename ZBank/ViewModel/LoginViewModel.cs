using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.Services;
using ZBank.View;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using Windows.ApplicationModel.VoiceCommands;
using ZBankManagement.Domain.UseCase;
using Windows.UI.Core;
using System.Windows.Input;
using ZBank.ViewModel.VMObjects;
using Windows.UI.Popups;
using ZBank.Entities;

namespace ZBank.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private IView View { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand SignupCommand { get; set; }

        public LoginViewModel(IView view) {

            View = view;
            LoginCommand = new RelayCommand(ValidateLogin);
            SignupCommand = new RelayCommand(SignupUser);
        }



        public void Reset()
        {
            SignupCustomer = null;
            Password = null;
            CustomerID = null;
            ErrorText = null;
            SuccessText = null;
        }

        private void SignupUser(object obj)
        {
            SignupUserRequest request = new SignupUserRequest()
            {
                Customer = SignupCustomer,
                Password = Password
            };
            IPresenterCallback<SignupUserResponse> presenterCallback = new SignupUserPresenterCallback(this);
            UseCaseBase<SignupUserResponse> useCase = new SignupUserUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void ValidateLogin(object parameter)
        {
            if(string.IsNullOrEmpty(CustomerID))
            {
                ErrorText = "Customer ID and Password is required";
            }

            if(string.IsNullOrEmpty(ErrorText))
            {
                LoginCustomer();
            }
        }

        private Customer _signupCustomer = new Customer();

        public Customer SignupCustomer
        {
            get { return _signupCustomer; }
            set { Set(ref _signupCustomer, value); }
        }


        private string _customerID = null;

        public string CustomerID
        {
            get { return _customerID; }
            set { Set(ref _customerID, value); }
        }

        private string _errorText = null;

        public string ErrorText
        {
            get { return _errorText; }
            set { Set(ref _errorText, value); }
        }

        private string _successText = null;

        public string SuccessText
        {
            get { return _successText; }
            set { Set(ref _successText, value); }
        }


        private string _password = null;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

      

        private void LoginCustomer()
        {
            LoginCustomerRequest request = new LoginCustomerRequest()
            {
                 CustomerID = CustomerID,
                 Password = Password,
            };

            IPresenterCallback<LoginCustomerResponse> presenterCallback = new LoginCustomerPresenterCallback(this);
            UseCaseBase<LoginCustomerResponse> useCase = new LoginCustomerUseCase(request, presenterCallback);
            useCase.Execute();
        }

        internal void OnLoaded()
        {
            ViewNotifier.Instance.CurrentUserChanged += OnCurrentUserChanged;
            ViewNotifier.Instance.LoginError += OnErrorLoggingIn;
            ViewNotifier.Instance.SignupSuccess += SignupSuccess;
        }

        private void SignupSuccess(Customer customer)
        {
            SuccessText = $"Customer ID of {customer.ID} created successfully";
        }

        private async void OnErrorLoggingIn(string obj)
        {
            ErrorText = obj;
;        }

        private void OnCurrentUserChanged(string obj)
        {
            if(!string.IsNullOrEmpty(obj)) 
            {
                AppSettings.Current.CustomerID = obj;
            }
        }

        internal void OnUnloaded()
        {
            ViewNotifier.Instance.CurrentUserChanged -= OnCurrentUserChanged;
            ViewNotifier.Instance.LoginError -= OnErrorLoggingIn;
            ViewNotifier.Instance.SignupSuccess -= SignupSuccess;
        }

        private class SignupUserPresenterCallback : IPresenterCallback<SignupUserResponse>
        {
            public LoginViewModel ViewModel { get; set; }

            public SignupUserPresenterCallback(LoginViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(SignupUserResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnSignupSuccess(response.InsertedCustomer);
                });
            }

            public async Task OnFailure(ZBankException exception)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnLoginError(exception.Message);
                });
            }
        }

        private class LoginCustomerPresenterCallback : IPresenterCallback<LoginCustomerResponse>
        {
            public LoginViewModel ViewModel { get; set; }

            public LoginCustomerPresenterCallback(LoginViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(LoginCustomerResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnCurrentUserChanged(response.LoggedInCustomer.ID);
                });
            }

            public async Task OnFailure(ZBankException exception)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnLoginError(exception.Message);  
                });
            }
        }
    }


}
