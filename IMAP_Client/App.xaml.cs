using IMAP_Client.Views;
using Prism.Ioc;
using System.Windows;

namespace IMAP_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AnyStateView>();
            containerRegistry.RegisterForNavigation<NoAuthStateView>();
            containerRegistry.RegisterForNavigation<AuthStateView>();
            containerRegistry.RegisterForNavigation<SelectedStateView>();
        }
    }
}
