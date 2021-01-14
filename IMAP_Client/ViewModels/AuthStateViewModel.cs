using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Client.ViewModels
{
    public class AuthStateViewModel : BindableBase
    {
        #region Properties
        private string _mailBox;
        public string MailBox
        {
            get { return _mailBox; }
            set { SetProperty(ref _mailBox, value); }
        }
        private string _oldMailBox;
        public string OldMailBox
        {
            get { return _oldMailBox; }
            set { SetProperty(ref _oldMailBox, value); }
        }
        private string _newMailBox;
        public string NewMailBox
        {
            get { return _newMailBox; }
            set { SetProperty(ref _newMailBox, value); }
        }
        private string _referenceName;
        public string ReferenceName
        {
            get { return _referenceName; }
            set { SetProperty(ref _referenceName, value); }
        }
        private string _mailboxNameAndWildCard;
        public string MailboxNameAndWildCard
        {
            get { return _mailboxNameAndWildCard; }
            set { SetProperty(ref _mailboxNameAndWildCard, value); }
        }
        private string _statusMailBox;
        public string StatusMailBox
        {
            get { return _statusMailBox; }
            set { SetProperty(ref _statusMailBox, value); }
        }
        private string _statusDateNames;
        public string StatusDateNames
        {
            get { return _statusDateNames; }
            set { SetProperty(ref _statusDateNames, value); }
        }
        private string _appendMailBox;
        public string AppendMailBox
        {
            get { return _appendMailBox; }
            set { SetProperty(ref _appendMailBox, value); }
        }
        private string _flag;
        public string Flag
        {
            get { return _flag; }
            set { SetProperty(ref _flag, value); }
        }
        private string _dateTime;
        public string DateTime
        {
            get { return _dateTime; }
            set { SetProperty(ref _dateTime, value); }
        }
        private string _messageLiteral;
        public string MessageLiteral
        {
            get { return _messageLiteral; }
            set { SetProperty(ref _messageLiteral, value); }
        }
        #endregion
        #region DelegateCommands
        public DelegateCommand Select { get; set; }
        public DelegateCommand Examine { get; set; }
        public DelegateCommand Create { get; set; }
        public DelegateCommand Delete { get; set; }
        public DelegateCommand Subscribe { get; set; }
        public DelegateCommand Unsubscribe { get; set; }
        public DelegateCommand Rename { get; set; }
        public DelegateCommand LIST { get; set; }
        public DelegateCommand LSUB { get; set; }
        public DelegateCommand Status { get; set; }
        public DelegateCommand Append { get; set; }
        #endregion
        public AuthStateViewModel()
        {
            Select = new DelegateCommand(ExecuteSelect);
            Select = new DelegateCommand(ExecuteExamine);
            Select = new DelegateCommand(ExecuteCreate);
            Select = new DelegateCommand(ExecuteDelete);
            Select = new DelegateCommand(ExecuteSubscribe);
            Select = new DelegateCommand(ExecuteUnsubscribe);
            Select = new DelegateCommand(ExecuteRename);
            Select = new DelegateCommand(ExecuteLIST);
            Select = new DelegateCommand(ExecuteLSUB);
            Select = new DelegateCommand(ExecuteStatus);
            Select = new DelegateCommand(ExecuteAppend);
        }
        #region Execute Functions
        private void ExecuteAppend()
        {
            //TODO
        }

        private void ExecuteStatus()
        {
            //TODO
        }

        private void ExecuteLSUB()
        {
            //TODO
        }

        private void ExecuteLIST()
        {
            //TODO
        }

        private void ExecuteRename()
        {
            //TODO
        }

        private void ExecuteUnsubscribe()
        {
            //TODO
        }

        private void ExecuteSubscribe()
        {
            //TODO
        }

        private void ExecuteDelete()
        {
            //TODO
        }

        private void ExecuteCreate()
        {
            //TODO
        }

        private void ExecuteExamine()
        {
            //TODO
        }

        private void ExecuteSelect()
        {
            //TODO
        }
        #endregion
    }
}
