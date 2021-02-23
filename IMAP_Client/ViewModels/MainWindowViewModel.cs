using IMAP.Shared;
using IMAP_Client.Services;
using IMAP_Client.UpdateEvents;
using IMAP_Client.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Navigation;
using Unity;

namespace IMAP_Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public static ServerConnection _connection;

        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        #region Properties
        private string _title = "IMAP Client Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string _console;
        public string Console
        {
            get { return _console; }
            set { SetProperty(ref _console, value); }
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
        private bool connected;
        public bool Connected
        {
            get { return connected; }
            set 
            { 
                SetProperty(ref connected, value);  
            }
        }
        private bool authentificated;
        public bool Authentificated
        {
            get { return authentificated; }
            set { SetProperty(ref authentificated, value); }
        }
        private bool selectedMailBox;
        public bool SelectedMailBox
        {
            get { return selectedMailBox; }
            set { SetProperty(ref selectedMailBox, value); }
        }
        private bool notConnected;
        public bool NotConnected
        {
            get { return notConnected; }
            set { SetProperty(ref notConnected, value); }
        }
        #endregion

        #region Delegate Commands
        public DelegateCommand Capability { get; set; }
        public DelegateCommand Noop { get; set; }
        public DelegateCommand Logout { get; set; }
        public DelegateCommand Disconnect { get; set; }
        public DelegateCommand Connect { get; set; }
        public DelegateCommand<string> NavigateNoAuthState { get; set; }
        public DelegateCommand<string> NavigateToAuthState { get; set; }
        public DelegateCommand<string> NavigateToSelecState { get; set; }
        #endregion
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
           // SelectedMailBox = true;
            NotConnected = true;
            NavigateNoAuthState = new DelegateCommand<string>(Navigate)
                .ObservesCanExecute(() => Connected);
            NavigateToAuthState = new DelegateCommand<string>(ExecuteToAuthState)
                .ObservesCanExecute(() => Authentificated);
            NavigateToSelecState = new DelegateCommand<string>(ExecuteToSelectState)
                .ObservesCanExecute(() => SelectedMailBox);
            Capability = new DelegateCommand(ExecuteCapability)
                .ObservesCanExecute(() => Connected);
            Noop = new DelegateCommand(ExecuteNoop)
                .ObservesCanExecute(() => Connected);
            Logout = new DelegateCommand(ExecuteLogout)
                .ObservesCanExecute(() => Authentificated);
            Disconnect = new DelegateCommand(ExecuteDisconnect)
                .ObservesCanExecute(() => Connected);
            Connect = new DelegateCommand(ExecuteConnect)
                .ObservesCanExecute(() => NotConnected);
            _eventAggregator.GetEvent<UpdateUserConsole>().Subscribe(UpdateConsole);
            _eventAggregator.GetEvent<UpdateAuthentificationState>().Subscribe(UpdateAuthentification);
            _eventAggregator.GetEvent<UpdateSelectedState>().Subscribe(UpdateSelection);
            _eventAggregator.GetEvent<UpdateServerConnectionState>().Subscribe(UpdateConnection);
            Port = 143;
            IPAddress = "127.0.0.1";
        }

        #region UpdateEvents    
        private void UpdateConnection(bool isConnected)
        {
            Connected = isConnected;
            NotConnected = !isConnected; 
        }
        private void UpdateSelection(bool state)
        {
            SelectedMailBox = state;
        }

        private void UpdateConsole(string obj)
        {
            Console += $"{obj}\n";
        }
        private void UpdateAuthentification(bool state)
        {
            Authentificated = state;
        }
        #endregion

        #region Buttons Functions
        private async void ExecuteConnect()
        {
            try
            {
                if (_connection != null)
                {
                    ExecuteDisconnect();
                }

                _connection = new ServerConnection(IPAddress, Port);
                _connection.outgoingTag = "*";
                _connection.outgoingCommand = "CONNECT";
                _connection.GetMessages(_eventAggregator);
                await Task.Delay(2500); //Waiting 2.5 seconds before canceling the connection.
                if (Connected)
                {
                    Navigate("NoAuthStateView");
                }
                else
                {
                    _connection.Disconnect();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
        

        private void ExecuteDisconnect()
        {
            if (Connected)
            {
                _connection.Disconnect();
                UpdateConsole("Disconnected from server");
                _connection = null;
                Connected = false;
                Authentificated = false;
                SelectedMailBox = false;
                NotConnected = true;
                _regionManager.Regions["ContentRegion"].RemoveAll();
            }      
        }

        private async void ExecuteLogout()
        {
            try
            {
                if (Authentificated)
                {
                    await _connection.SendMessage($"{TaggingService.Tag} LOGOUT", _eventAggregator);
                    Authentificated = false;
                    SelectedMailBox = false;
                    _regionManager.Regions["ContentRegion"].RemoveAll();
                }                        
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private async void ExecuteNoop()
        {  
            string tag = TaggingService.Tag;
            await _connection.SendMessage($"{tag} NOOP", _eventAggregator);
        }

        private async void ExecuteCapability()
        {
            try
            {
                string tag = TaggingService.Tag;
                await _connection.SendMessage($"{tag} CAPABILITY", _eventAggregator);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
        #endregion

        #region Navigation
        private void Navigate(string navigationParam)
        {
            _regionManager.RequestNavigate("ContentRegion", navigationParam);  
        }
        private void ExecuteToAuthState(string navigationParam)
        {
            _regionManager.RequestNavigate("ContentRegion", navigationParam);
        }

        private void ExecuteToSelectState(string navigationParam)
        {
            _regionManager.RequestNavigate("ContentRegion", navigationParam);
        }
        #endregion
    }
}
