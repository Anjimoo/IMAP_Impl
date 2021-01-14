using IMAP_Client.Services;
using IMAP_Client.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows.Navigation;
using Unity;

namespace IMAP_Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private ServerConnection _connection;
        private IRegionManager _regionManager;
        #region Properties
        private string _title = "IMAP Client Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string _ipAddress;
        public string IPAddress
        {
            get { return _ipAddress; }
            set { SetProperty(ref _ipAddress, value); }
        }
        private int _port;
        public int Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }
        #endregion

        #region Delegate Commands
        public DelegateCommand Capability { get; set; }
        public DelegateCommand Noop { get; set; }
        public DelegateCommand Logout { get; set; }
        public DelegateCommand Disconnect { get; set; }
        public DelegateCommand Connect { get; set; }
        public DelegateCommand<string> NavigateCommand { get; set; }
        #endregion
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            Capability = new DelegateCommand(ExecuteCapability);
            Noop = new DelegateCommand(ExecuteNoop);
            Logout = new DelegateCommand(ExecuteLogout);
            Disconnect = new DelegateCommand(ExecuteDisconnect);
            Connect = new DelegateCommand(ExecuteConnect);
        }


        #region Buttons Functions
        private void ExecuteConnect()
        {
            _connection = new ServerConnection(IPAddress, Port);
            
        }
        private void ExecuteDisconnect()
        {
            _connection.Disconnect();
        }

        private void ExecuteLogout()
        {
            //TODO
            //check connection to server
            _connection.Connect("Hello world");
        }

        private void ExecuteNoop()
        {
            //TODO
        }

        private void ExecuteCapability()
        {
            //TODO
        }
        #endregion

        private void Navigate(string navigationParams)
        {
            _regionManager.RequestNavigate("ContentRegion", navigationParams);  
        }
    }
}
