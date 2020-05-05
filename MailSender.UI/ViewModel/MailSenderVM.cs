using AsyncAwaitBestPractices.MVVM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MailSender.DAL.Exceptions;
using MailSender.DAL.Models;
using MailSender.DAL.Repos;
using MailSender.DAL.Services;
using MailSender.UI.Views.Behaviors;
using MailSender.UI.Views.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailSender.UI.ViewModel
{
	public class MailSenderVM : ViewModelBase
	{
		private IWindowService windowService;
		private IAsyncCommand _sendEmailCommand;
		private IAsyncCommand _sendShedullerCommand;
		private IEmailSendService _emailSendService;
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
		private RelayCommand _openSendersEditWindowCommand;
		private RelayCommand _openSmtpEditWindowCommand;
		private RelayCommand _openRecipientEditWindowCommand;
		private SnackbarMessageQueue _snackBarMessageQueue;
		public MailSenderVM(IEmailSendService service, IWindowService openWindowService)
		{
			_emailSendService = service;
			windowService = openWindowService;
			Messenger.Default.Register<Sender>(this, x => EditSendersList(x));
			Messenger.Default.Register<Host>(this, x => EditHostList(x));
			Messenger.Default.Register<Recipient>(this, x => EditRecipientList(x));
			MyMessageQueue = new SnackbarMessageQueue(new TimeSpan(0, 0, 3));
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

		/// <summary>
		/// Дата запланированной отправки
		/// </summary>
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
					if (_sendersEmails == null)
					{
						_sendersEmails = new ObservableCollection<string>(sender.GetAll().Select(x => x.Email));
					}
					return _sendersEmails;
				}
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
				using (var recipient = new BaseRepo<Recipient>())
				{
					if (_recipients == null)
					{
						_recipients = new ObservableCollection<Recipient>(recipient.GetAll());
					}
					return _recipients;
				}
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
					using (var hosts = new BaseRepo<Host>())
					{
						_smtpServers = new ObservableCollection<string>(hosts.GetAll().Select(x => x.Server));
					}
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
		public void SerarchRecipient()
		{
			if (!string.IsNullOrWhiteSpace(_searchField))
			{
				using (var recipient = new BaseRepo<Recipient>())
				{
					Recipients = new ObservableCollection<Recipient>(recipient.GetAll().Where(x => x.Email.Contains(_searchField)));
				}
			}
		}


		public async Task SendEmailShedullerAsync()
		{
			IsBusy = true;
			var messages = GetMessages();
			try
			{
				await Task.Run(() => _emailSendService.SendEmailScheduler(
					 SmtpServer,
					 messages,
					 SendDate,
					 SendTime));
				MyMessageQueue.Enqueue($"Письмо будет отправлено {SendDate.ToShortDateString()} в {SendTime.ToShortTimeString()}");
			}
			catch (EmailSendServiceException ex)
			{
				MyMessageQueue.Enqueue(ex.Message);
			}
			catch (Exception ex)
			{
				windowService.showWindow(new WarningsVW());
				Messenger.Default.Send(ex);
			}

			IsBusy = false;
		}

		public async Task SendEmailNowAsync()
		{
			IsBusy = true;

			var messages = GetMessages();

			try
			{
				await _emailSendService.SendEmailNow(SmtpServer, messages);
				MyMessageQueue.Enqueue("Письмо отправлено");
			}
			catch (EmailSendServiceException ex)
			{
				MyMessageQueue.Enqueue(ex.Message);
			}
			catch (Exception ex)
			{
				windowService.showWindow(new WarningsVW());
				Messenger.Default.Send(ex);
			}

			IsBusy = false;
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
			(() => windowService.showWindow(new SendersVM())));

		public RelayCommand OpenSmtpEditWindowCommand => _openSmtpEditWindowCommand ?? (_openSmtpEditWindowCommand = new RelayCommand
		(() => windowService.showWindow(new SmtpVM())));

		public RelayCommand OpenRecipientEditWindowCommand => _openRecipientEditWindowCommand ?? (_openRecipientEditWindowCommand = new RelayCommand
		(() => windowService.showWindow(new RecipientsVM())));

		public void EditSendersList(Sender editedSender)
		{
			using (var dbSender = new BaseRepo<Sender>())
			{
				SendersEmails = new ObservableCollection<string>(dbSender.GetAll().Select(x => x.Email));
			}

			//if (_senders == null)
			//{
			//	using (var dbSender = new BaseRepo<Sender>())
			//	{
			//		_senders = new ObservableCollection<Sender>(dbSender.GetAll());
			//	}
			//}

			//var edited = _senders.Where(x => x.Id == editedSender.Id).FirstOrDefault();
			//var deleted = _senders.Where(x => Equals(x, editedSender)).FirstOrDefault();
			//if (edited != null)
			//{
			//	_senders.Remove(edited);
			//	_senders.Add(editedSender);
			//}
			//else if(deleted!=null)
			//{
			//	_senders.Remove(deleted);
			//}
			//else
			//{
			//	_senders.Add(editedSender);
			//}			
			//_sendersEmails = new ObservableCollection<string>(_senders.Select(x => x.Email));
			//SendersEmails = _sendersEmails;			
		}

		public void EditHostList(Host editedHost)
		{
			using (var dbHost = new BaseRepo<Host>())
			{
				SmtpServers = new ObservableCollection<string>(dbHost.GetAll().Select(x => x.Server));
			}
		}
		public void EditRecipientList(Recipient editedRecipient)
		{
			using (var dbRecipient = new BaseRepo<Recipient>())
			{
				Recipients = new ObservableCollection<Recipient>(dbRecipient.GetAll());
			}
		}
	}
}
