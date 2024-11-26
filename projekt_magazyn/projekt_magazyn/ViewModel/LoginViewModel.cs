using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace projekt_magazyn.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _inViewVisible = true;

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public SecureString Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }

        }
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }

        }
        public bool InViewVisible
        {
            get
            {
                return _inViewVisible;
            }

            set
            {
                _inViewVisible = value;
                OnPropertyChanged("ErrorMessage");
            }

        }

        public ICommand LoginCommand { get; }
        public ICommand PasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);


        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || Password == null || Password.Length < 3)
                validData = false;
            else 
                validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
