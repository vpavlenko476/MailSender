using AsyncAwaitBestPractices.MVVM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MailSender.DAL.Models;
using MailSender.DAL.Repos;
using MailSender.DAL.Services;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailSender.UI.ViewModel
{
	public class MailSenderVM : ViewModelBase
	{
		private IAsyncCommand _sendEmailCommand;
		private IAsyncCommand _sendShedullerCommand;
		private EmailSendService _emailSendService;
		private string _messageBody;
		private string _messageTitle;
		private bool _isBusy;		
		private int selectedTabControl = 0;
		private RelayCommand _openScheduler;
		private string _senderEmail;
		private Recipient _recipientEmail;
		private string _smtpServer;
		private DateTime _sendTime;
		private DateTime _sendDate;

		public MailSenderVM()
		{
			_emailSendService = new EmailSendService();
		}

		/// <summary>
		/// Текст письма
		/// </summary>
		public string Message
		{
			get { return _messageBody; }
			set { Set(ref _messageBody, value); }
		}

		/// <summary>
		/// Заголовок письма
		/// </summary>
		public string Title
		{
			get { return _messageTitle; }
			set { Set(ref _messageTitle, value); }
		}

		/// <summary>
		/// Smtp сервер
		/// </summary>
		public string SmtpServer
		{
			get { return _smtpServer; }
			set { Set(ref _smtpServer, value); }
		}

		/// <summary>
		/// Адрес отправителя
		/// </summary>
		public string SenderEmail
		{
			get { return _senderEmail; }
			set { Set(ref _senderEmail, value); }
		}

		/// <summary>
		/// Адрес получателя
		/// </summary>
		public Recipient RecipientEmail
		{
			get { return _recipientEmail; }
			set { Set(ref _recipientEmail, value); }
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
		/// Сообщение письма
		/// </summary>
		public string MessageBody
		{
			get { return _messageBody; }
			set { Set(ref _messageBody, value); }
		}

		public DateTime SendTime
		{
			get { return _sendTime; }
			set { Set(ref _sendTime, value); }
		}

		public DateTime SendDate
		{
			get { return _sendDate; }
			set { Set(ref _sendDate, value); }
		}

		/// <summary>
		/// Активная вкладка TabControl
		/// </summary>
		public int SelectedTabControl
		{
			get { return selectedTabControl; }
			set { Set(ref selectedTabControl, value); }
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
		/// Список эл.почт отправителей
		/// </summary>
		public ObservableCollection<string> SendersEmails
		{
			get
			{
				using (var sender = new BaseRepo<Sender>())
				{
					return new ObservableCollection<string>(sender.GetAll().Select(x => x.Email));
				}
			}
		}

		/// <summary>
		/// Список эл.почт получателей
		/// </summary>
		public ObservableCollection<Recipient> RecipientsEmails
		{
			get
			{
				using (var recipient = new BaseRepo<Recipient>())
				{
					return new ObservableCollection<Recipient>(recipient.GetAll());
				}
			}
		}

		/// <summary>
		/// Список Smtp серверов
		/// </summary>
		public ObservableCollection<string> SmtpServers
		{
			get
			{
				using (var hosts = new BaseRepo<Host>())
				{
					return new ObservableCollection<string>(hosts.GetAll().Select(x => x.Server));
				}
			}
		}

		/// <summary>
		/// Отправка письма сейчас
		/// </summary>
		public IAsyncCommand SendEmailNowCommand
		{
			get
			{
				return _sendEmailCommand ?? (_sendEmailCommand = new AsyncCommand(SendEmailNowAsync));
			}
		}

		/// <summary>
		/// Отправка письма запланированно
		/// </summary>
		public IAsyncCommand SendEmailShedullerCommand
		{
			get
			{
				return _sendShedullerCommand ?? (_sendShedullerCommand = new AsyncCommand(SendEmailShedullerAsync));
			}
		}

		/// <summary>
		/// Открытие вкладки планировщика
		/// </summary>
		public RelayCommand OpenScheduler
		{
			get
			{
				return _openScheduler ?? (_openScheduler = new RelayCommand(() =>
				{
					SelectedTabControl = 1;
				}));
			}
		}
		
		public async Task SendEmailShedullerAsync()
		{
			IsBusy = true;
			if (!(string.IsNullOrEmpty(_messageBody) || string.IsNullOrEmpty(_messageTitle)))
			{
				await Task.Run(() => _emailSendService.SendEmailScheduler(
					 SmtpServer,
					 new MailMessage(_senderEmail, _recipientEmail.Email) { Body = _messageBody, Subject = _messageTitle, IsBodyHtml = false },
					 _sendDate,
					 _sendTime));
			}
			IsBusy = false;
		}
		
		public async Task SendEmailNowAsync()
		{
			IsBusy = true;

			if (!(string.IsNullOrEmpty(_messageBody) || string.IsNullOrEmpty(_messageTitle)))
			{
				await _emailSendService.SendEmailNow(
					   SmtpServer,
					   new MailMessage(_senderEmail, _recipientEmail.Email) { Body = _messageBody, Subject = _messageTitle, IsBodyHtml = false });
			}
			IsBusy = false;
		}
	}
}
