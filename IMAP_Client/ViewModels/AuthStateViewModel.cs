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
    public class AuthStateViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
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
        public AuthStateViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Select = new DelegateCommand(ExecuteSelect);
            Examine = new DelegateCommand(ExecuteExamine);
            Create = new DelegateCommand(ExecuteCreate);
            Delete = new DelegateCommand(ExecuteDelete);
            Subscribe = new DelegateCommand(ExecuteSubscribe);
            Unsubscribe = new DelegateCommand(ExecuteUnsubscribe);
            Rename = new DelegateCommand(ExecuteRename);
            LIST = new DelegateCommand(ExecuteLIST);
            LSUB = new DelegateCommand(ExecuteLSUB);
            Status = new DelegateCommand(ExecuteStatus);
            Append = new DelegateCommand(ExecuteAppend);
        }

        #region Execute Functions
        private async void ExecuteAppend()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} APPEND {MailBox} {Flag} {DateTime} {MessageLiteral}",_eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            

        }

        private async void ExecuteStatus()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} STATUS {MailBox} {MailboxNameAndWildCard} ", _eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private async void ExecuteLSUB()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} LSUB {ReferenceName} {MailboxNameAndWildCard} ", _eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private async void ExecuteLIST()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} LIST {ReferenceName} {MailboxNameAndWildCard} ", _eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private async void ExecuteRename()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} RENAME {MailBox} {NewMailBox}", _eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private async void ExecuteUnsubscribe()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} UNSUBSCRIBE {MailBox}", _eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private async void ExecuteSubscribe()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} SUBSCRIBE {MailBox}", _eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private async void ExecuteDelete()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} DELETE {MailBox}", _eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
              
        }

        private async void ExecuteCreate()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} CREATE {MailBox}", _eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private async void ExecuteExamine()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} EXAMINE {MailBox}", _eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private async void ExecuteSelect()
        {
            try
            {
                string response;
                response = await MainWindowViewModel._connection.SendMessage($"{TaggingService.Tag} SELECT {MailBox}", _eventAggregator);
                //_eventAggregator.GetEvent<UpdateUserConsole>().Publish(response);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
        #endregion
    }
}
