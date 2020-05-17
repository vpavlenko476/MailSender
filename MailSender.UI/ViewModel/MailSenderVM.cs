using AsyncAwaitBestPractices.MVVM;
using Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MailSender.UI.Views.Behaviors;
using MailSender.UI.Views.Services;
using MaterialDesignThemes.Wpf;
using Services.Abstract;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailSender.UI.ViewModel
{
	public class MailSenderVM : ViewModelBase
	{
		private readonly IRecipientService _recipientService;
		private readonly IHostService _hostService;
		private readonly ISenderService _senderService;
		private readonly IWindowService _windowService;
		private IAsyncCommand _sendEmailCommand;
		private IAsyncCommand _sendShedullerCommand;
		private IMessageSendService _mesageSendService;
		private string _messageBody;
		private string _messageTitle;
		private bool _isBusy;
		private int selectedTabControl = 0;
		private RelayCommand _openScheduler;
		private string _senderEmail;
		private ObservableCollection<string> _smtpServers;
		private Recipient _recipientEmail;
		private string _smtpServer;
		private DateTime _sendTime;
		private DateTime _sendDate;
		private string _searchField;
		private ObservableCollection<Recipient> _recipients;
		private ObservableCollection<Sender> _senders;
		private ObservableCollection<string> _sendersEmails;
		private ObservableCollection<Message> _sheduledMessages;
		private RelayCommand _openSendersEditWindowCommand;
		private RelayCommand _openSmtpEditWindowCommand;
		private RelayCommand _openRecipientEditWindowCommand;
		private SnackbarMessageQueue _snackBarMessageQueue;
		public MailSenderVM
			(IRecipientService recService,
			IHostService hostService,
			ISenderService senderService,
			IMessageSendService emailSendService,
			IWindowService openWindowService)
		{
			_senderService = senderService;
			_hostService = hostService;
			_recipientService = recService;
			_mesageSendService = emailSendService;
			_windowService = openWindowService;
			Messenger.Default.Register<Sender>(this, x => EditSendersList(x));
			Messenger.Default.Register<Host>(this, x => EditHostList(x));
			Messenger.Default.Register<Recipient>(this, x => EditRecipientList(x));
			MyMessageQueue = new SnackbarMessageQueue(new TimeSpan(0, 0, 3));
			_mesageSendService.messageSend += _mesageSendService_messageSend;
			_mesageSendService.messageScheduled += _mesageSendService_messageScheduled;
		}

		private void _mesageSendService_messageScheduled()
		{
			SheduledMessages = new ObservableCollection<Message>(_mesageSendService.GetMessages().Where(x=>x.SendDateTime==null).OrderBy(x=>x.ScheduledSendDateTime));
		}

		private void _mesageSendService_messageSend()
		{
			SheduledMessages = new ObservableCollection<Message>(_mesageSendService.GetMessages().Where(x => x.SendDateTime == null).OrderBy(x => x.ScheduledSendDateTime));
		}

		public SnackbarMessageQueue MyMessageQueue
		{
			get { return _snackBarMessageQueue; }
			set { Set(ref _snackBarMessageQueue, value); }
		}

		/// <summary>
		/// Строка поиска
		/// </summary>
		public string SearchField
		{
			get { return _searchField; }
			set
			{
				Set(ref _searchField, value);
				SerarchRecipient();
			}
		}

		/// <summary>
		/// Сообщение
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
			get
			{
				return _smtpServer;
			}
			set
			{
				Set(ref _smtpServer, value);
			}
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

		/// <summary>
		/// Дата запланированной отправки
		/// </summary>
		public DateTime SendDate
		{
			get { return _sendDate; }
			set
			{
				Set(ref _sendDate, value);
			}
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
		/// Список запланированных писем
		/// </summary>
		public ObservableCollection<Message> SheduledMessages
		{
			get
			{	
				return _sheduledMessages;
			}
			set
			{
				Set(ref _sheduledMessages, value);
			}
		}

		/// <summary>
		/// Список эл.почт отправителей
		/// </summary>
		public ObservableCollection<string> SendersEmails
		{
			get
			{
				if (_sendersEmails == null)
				{
					_sendersEmails = new ObservableCollection<string>(_senderService.GetAll().Select(x => x.Email));
				}
				return _sendersEmails;
			}
			set
			{
				Set(ref _sendersEmails, value);
			}
		}

		/// <summary>
		/// Список получателей
		/// </summary>
		public ObservableCollection<Recipient> Recipients
		{
			get
			{
				if (_recipients == null)
				{
					_recipients = new ObservableCollection<Recipient>(_recipientService.GetAll());
				}
				return _recipients;
			}
			set
			{
				{ Set(ref _recipients, value); }
			}
		}

		/// <summary>
		/// Список Smtp серверов
		/// </summary>
		public ObservableCollection<string> SmtpServers
		{
			get
			{
				if (_smtpServers == null)
				{
					_smtpServers = new ObservableCollection<string>(_hostService.GetAll().Select(x => x.Server));
				}
				return _smtpServers;
			}
			set
			{
				{ Set(ref _smtpServers, value); }
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

		// TODO: Вынести обращение к бд из метода, в методе оставить работу с имеющейся коллекцией
		private void SerarchRecipient()
		{
			if (!string.IsNullOrWhiteSpace(_searchField))
			{
				Recipients = new ObservableCollection<Recipient>(_recipientService.GetAll().Where(x => x.Email.Contains(_searchField)));
			}
		}


		private async Task SendEmailShedullerAsync()
		{
			var messages = GetMessages();
			try
			{
				MyMessageQueue.Enqueue($"Письмо будет отправлено {SendDate.ToShortDateString()} в {SendTime.ToShortTimeString()}");
				await Task.Run(() => _mesageSendService.SendEmailScheduler(
					 SmtpServer,
					 messages,
					 SendDate,
					 SendTime));
			}
			catch (EmailSendServiceException ex)
			{
				MyMessageQueue.Enqueue(ex.Message);
			}
		}

		private async Task SendEmailNowAsync()
		{
			var messages = GetMessages();
			try
			{
				await _mesageSendService.SendEmailNow(SmtpServer, messages);
				MyMessageQueue.Enqueue("Письмо отправлено");
			}
			catch (EmailSendServiceException ex)
			{
				MyMessageQueue.Enqueue(ex.Message);
			}
		}

		private IEnumerable<MailMessage> GetMessages()
		{
			var messages = new List<MailMessage>();
			var recipients = new MultiSelectBehavior();
			foreach (var recipient in recipients.SelectedItems)
			{
				messages.Add(new MailMessage(SenderEmail, recipient)
				{
					Body = _messageBody,
					Subject = _messageTitle,
					IsBodyHtml = false
				});
			}
			return messages;
		}

		public RelayCommand OpenSendersEditWindowCommand => _openSendersEditWindowCommand ?? (_openSendersEditWindowCommand = new RelayCommand
			(() => _windowService.showWindow(new SendersVM(_senderService))));

		public RelayCommand OpenSmtpEditWindowCommand => _openSmtpEditWindowCommand ?? (_openSmtpEditWindowCommand = new RelayCommand
		(() => _windowService.showWindow(new HostVM(_hostService))));

		public RelayCommand OpenRecipientEditWindowCommand => _openRecipientEditWindowCommand ?? (_openRecipientEditWindowCommand = new RelayCommand
		(() => _windowService.showWindow(new RecipientsVM(_recipientService))));

		private void EditSendersList(Sender editedSender)
		{
			SendersEmails = new ObservableCollection<string>(_senderService.GetAll().Select(x => x.Email));
		}

		private void EditHostList(Host editedHost)
		{
			SmtpServers = new ObservableCollection<string>(_hostService.GetAll().Select(x => x.Server));
		}
		private void EditRecipientList(Recipient editedRecipient)
		{
			Recipients = new ObservableCollection<Recipient>(_recipientService.GetAll());
		}
	}
}
