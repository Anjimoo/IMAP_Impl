﻿using IMAP_Client.Services;
using Prism.Commands;
using Prism.Events;
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

        private void ExecuteLogin()
        {
            string response;
            response = MainWindowViewModel._connection.SendMessage($"LOGIN {UserName} {Password}");
            _eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
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
