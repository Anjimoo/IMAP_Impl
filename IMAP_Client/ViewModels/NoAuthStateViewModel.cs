using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Client.ViewModels
{
    public class NoAuthStateViewModel : BindableBase
    {
        private string _authMechName;
        public string AuthMechName
        {
            get { return _authMechName; }
            set { SetProperty(ref _authMechName, value); }
        }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public DelegateCommand StartTLS { get; set; }
        public DelegateCommand Authenticate { get; set; }
        public DelegateCommand Login { get; set; }

        public NoAuthStateViewModel()
        {
            StartTLS = new DelegateCommand(ExecuteStartTLS);
            Authenticate = new DelegateCommand(ExecuteAuthenticate);
            Login = new DelegateCommand(ExecuteLogin);
        }

        private void ExecuteLogin()
        {
            //TODO
        }

        private void ExecuteAuthenticate()
        {
            //TODO
        }

        private void ExecuteStartTLS()
        {
            //TODO
        }
    }
}
