using IMAP_Client.Services;
using IMAP_Client.UpdateEvents;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
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
        private IRegionManager _regionManager;
        #region Properties
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
        private bool canExecute;
        public bool CanExecute
        {
            get { return canExecute; }
            set { SetProperty(ref canExecute, value); }
        }
        #endregion
        public DelegateCommand StartTLS { get; set; }
        public DelegateCommand Authenticate { get; set; }
        public DelegateCommand Login { get; set; }

        public NoAuthStateViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            StartTLS = new DelegateCommand(ExecuteStartTLS).
                ObservesCanExecute(() => CanExecute);
            Authenticate = new DelegateCommand(ExecuteAuthenticate).
                ObservesCanExecute(() => CanExecute);
            Login = new DelegateCommand(ExecuteLogin).
                ObservesCanExecute(() => CanExecute);
            _eventAggregator.GetEvent<UpdateAuthentificationState>().Subscribe(UpdateCanExecute);
            CanExecute = true;
        }

        private void UpdateCanExecute(bool obj)
        {
            CanExecute = !obj;
        }

        private async void ExecuteLogin()
        {
            try
            {
                string tag = TaggingService.Tag;
                await MainWindowViewModel._connection.SendMessage($"{tag} LOGIN {UserName} {Password}", _eventAggregator);
                await Task.Delay(2000);
                if (!CanExecute)
                {
                    _regionManager.RequestNavigate("ContentRegion", "AuthStateView");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private async void ExecuteAuthenticate()
        {
            try
            {
                string tag = TaggingService.Tag;
                await MainWindowViewModel._connection.SendMessage($"{tag} AUTHENTICATE", _eventAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void ExecuteStartTLS()
        {
            try
            {
                string tag = TaggingService.Tag;
                await MainWindowViewModel._connection.SendMessage($"{tag} STARTTLS", _eventAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
