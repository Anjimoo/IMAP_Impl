using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public SelectedStateViewModel()
        {
            Check = new DelegateCommand(ExecuteCheck);
            Check = new DelegateCommand(ExecuteClose);
            Check = new DelegateCommand(ExecuteExpunge);
            Check = new DelegateCommand(ExecuteSearch);
            Check = new DelegateCommand(ExecuteFetch);
            Check = new DelegateCommand(ExecuteStore);
            Check = new DelegateCommand(ExecuteCopy);
            Check = new DelegateCommand(ExecuteUID);
        }
        #region Functions
        private void ExecuteUID()
        {
            //TODO
        }

        private void ExecuteCopy()
        {
            //TODO
        }

        private void ExecuteStore()
        {
            //TODO
        }

        private void ExecuteFetch()
        {
            //TODO
        }

        private void ExecuteSearch()
        {
            //TODO
        }

        private void ExecuteExpunge()
        {
            //TODO
        }

        private void ExecuteClose()
        {
            //TODO
        }

        private void ExecuteCheck()
        {
            //TODO
        }
        #endregion
    }
}
