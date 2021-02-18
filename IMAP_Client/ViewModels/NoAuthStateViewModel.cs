using IMAP_Client.Services;
using IMAP_Client.UpdateEvents;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMAP_Client.ViewModels
{
    public class NoAuthStateViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;

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

        public NoAuthStateViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            StartTLS = new DelegateCommand(ExecuteStartTLS);
            Authenticate = new DelegateCommand(ExecuteAuthenticate);
            Login = new DelegateCommand(ExecuteLogin);
        }

        private async void ExecuteLogin()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} LOGIN {UserName} {Password}");
                _eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
                if (response.Split()[1] == "OK")
                {
                    _eventAggregator.GetEvent<UpdateAuthentificationState>().Publish(true);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
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
