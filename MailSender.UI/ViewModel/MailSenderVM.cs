using AsyncAwaitBestPractices.MVVM;
using GalaSoft.MvvmLight;
using MailSender.DAL.Consts;
using MailSender.DAL.Models;
using MailSender.DAL.Services;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailSender.UI.ViewModel
{
	public class MailSenderVM: ViewModelBase
	{
        private IAsyncCommand _sendEmailCommand;
        private EmailSendService _emailSendService;
        private string _messageBody;
        private string _messageTitle;
        private bool _isBusy;
        private string login = ConfigurationManager.AppSettings["login"];
        private string password = ConfigurationManager.AppSettings["password"];
        private string emailFrom = ConfigurationManager.AppSettings["emailFrom"];
        private string emailTo = ConfigurationManager.AppSettings["emailTo"];

        public MailSenderVM()
        {
            _emailSendService = new EmailSendService();
        }

        /// <summary>
        /// Заголовок письма
        /// </summary>
        public string MessageTitle
        {
            get { return _messageTitle; }
            set { Set(ref _messageTitle, value); }
        }

        /// <summary>
        /// Сооьщение письма
        /// </summary>
        public string MessageBody
        {
            get { return _messageBody; }
            set { Set(ref _messageBody, value); }
        }

        /// <summary>
        /// Busy indicator
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        /// <summary>
        /// Отправка письма
        /// </summary>
        public IAsyncCommand SendEmailCommand
        {
            get
            {
                return _sendEmailCommand ?? (_sendEmailCommand = new AsyncCommand(SendEmailAsync));
            }
        }

        public async Task SendEmailAsync()
        {
            IsBusy = true;

            if (!(string.IsNullOrEmpty(_messageBody) || string.IsNullOrEmpty(_messageTitle)))
            {
                await _emailSendService.SendEmail(
                new SmtpClient(Hosts.YandexHost, 25),
                new Message() { Body = _messageBody, From = emailFrom, To = emailTo, Title = _messageTitle },
                login,
                password);
            }

            IsBusy = false;
        }
    }
}
