using IMAP_Client.Services;
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
    public class SelectedStateViewModel : BindableBase
    {
        #region Properties
        //Search
        private string _searchBox;
        public string SearchBox
        {
            get { return _searchBox; }
            set { SetProperty(ref _searchBox, value); }
        }
        //Fetch
        private string _fetchSequenceSet;
        public string FetchSequenceSet
        {
            get { return _fetchSequenceSet; }
            set { SetProperty(ref _fetchSequenceSet, value); }
        }
        private string _fetchMessageDataItem;
        public string FetchMessageDataItem
        {
            get { return _fetchMessageDataItem; }
            set { SetProperty(ref _fetchMessageDataItem, value); }
        }
        //Store
        private string _storeSequenceSet;
        public string StoreSequenceSet
        {
            get { return _storeSequenceSet; }
            set { SetProperty(ref _storeSequenceSet, value); }
        }
        private string _storeMessageDataItemName;
        public string StoreMessageDataItemName
        {
            get { return _storeMessageDataItemName; }
            set { SetProperty(ref _storeMessageDataItemName, value); }
        }
        private string _valueForMessage;
        public string ValueForMessage
        {
            get { return _valueForMessage; }
            set { SetProperty(ref _valueForMessage, value); }
        }
        //COPY
        private string _copySequenceSet;
        public string CopySequenceSet
        {
            get { return _copySequenceSet; }
            set { SetProperty(ref _copySequenceSet, value); }
        }
        private string _mailBox;
        public string MailBox
        {
            get { return _mailBox; }
            set { SetProperty(ref _mailBox, value); }
        }
        //UID
        private string _commandName;
        public string CommandName
        {
            get { return _commandName; }
            set { SetProperty(ref _commandName, value); }
        }
        private string _commandArguments;
        public string CommandArguments
        {
            get { return _commandArguments; }
            set { SetProperty(ref _commandArguments, value); }
        }

        private IEventAggregator _eventAggregator;
        #endregion
        #region Delegate Commands
        public DelegateCommand Check { get; set; }
        public DelegateCommand Close { get; set; }
        public DelegateCommand Expunge { get; set; }
        public DelegateCommand Search { get; set; }
        public DelegateCommand Fetch { get; set; }
        public DelegateCommand Store { get; set; }
        public DelegateCommand Copy { get; set; }
        public DelegateCommand UID { get; set; }
        #endregion
        public SelectedStateViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Check = new DelegateCommand(ExecuteCheck);
            Close = new DelegateCommand(ExecuteClose);
            Expunge = new DelegateCommand(ExecuteExpunge);
            Search = new DelegateCommand(ExecuteSearch);
            Fetch = new DelegateCommand(ExecuteFetch);
            Store = new DelegateCommand(ExecuteStore);
            Copy = new DelegateCommand(ExecuteCopy);
            UID = new DelegateCommand(ExecuteUID);
        }
        
        #region Functions
        private async void ExecuteUID()
        {
            try
            {      
                await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} UID {CommandName} {CommandArguments}", _eventAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private async void ExecuteCopy()
        {
            try
            {
                await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} COPY {CopySequenceSet} {MailBox}", _eventAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void ExecuteStore()
        {
            try
            {
                await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} STORE {StoreSequenceSet} {StoreMessageDataItemName}", _eventAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void ExecuteFetch()
        {
            try
            {
                await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} FETCH {FetchSequenceSet} {FetchMessageDataItem}", _eventAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void ExecuteSearch()
        {
            try
            {
                await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} SEARCH {SearchBox}", _eventAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void ExecuteExpunge()
        {
            try
            {          
                await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} EXPUNGE", _eventAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void ExecuteClose()
        {
            try
            {               
               await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} CLOSE", _eventAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void ExecuteCheck()
        {
            try
            {
                await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} CHECK", _eventAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
    }
}
