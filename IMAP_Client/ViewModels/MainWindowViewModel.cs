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
        private IRegionManager _regionManager;
        private string _title = "IMAP Client Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand Capability { get; set; }
        public DelegateCommand Noop { get; set; }
        public DelegateCommand Logout { get; set; }

        public DelegateCommand<string> NavigateCommand { get; set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            Capability = new DelegateCommand(ExecuteCapability);
            Noop = new DelegateCommand(ExecuteNoop);
            Logout = new DelegateCommand(ExecuteLogout);
        }

        private void ExecuteLogout()
        {
            //TODO
        }

        private void ExecuteNoop()
        {
            //TODO
        }

        private void ExecuteCapability()
        {
            //TODO
        }

        private void Navigate(string navigationParams)
        {
            _regionManager.RequestNavigate("ContentRegion", navigationParams);  
        }
    }
}
