using AsyncAwaitBestPractices.MVVM;
using Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Services;
using Services.Abstract;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MailSender.UI.ViewModel
{
	public class SendersVM : ViewModelBase
	{
		private ISenderService _senderService;
		private ObservableCollection<Sender> _senders;
		private string _senderEmail;
		private string _senderPassword;
		private Sender _sender;
		private IAsyncCommand _addSender;
		private IAsyncCommand _editSender;
		private IAsyncCommand _deleteSender;
		public SendersVM(ISenderService service)
		{
			_senderService = service;
		}

		/// <summary>
		/// Выбранные в dataGrid отправитель 
		/// </summary>
		public Sender SelectedSender
		{
			get { return _sender; }
			set { Set(ref _sender, value); }
		}
		/// <summary>
		/// Почта отправителя
		/// </summary>
		public string SenderEmail
		{
			get { return _senderEmail; }
			set
			{
				if (Regex.IsMatch(value, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
				{
					Set(ref _senderEmail, value);
				}
				else _senderPassword = null;
			}
		}

		/// <summary>
		/// Пароль отправителя
		/// </summary>
		public string SenderPassword
		{
			get { return _senderPassword; }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					Set(ref _senderPassword, value);
				}
				else _senderPassword = null;
			}
		}

		public ObservableCollection<Sender> Senders
		{
			get
			{
				if (_senders == null)
				{
					_senders = new ObservableCollection<Sender>(_senderService.GetAll().Select(x => x = new Sender()
					{
						Id = x.Id,
						Email = x.Email,
						Password = PasswordCoding.Decrypt(x.Password)
					}));
				}
				return _senders;
			}
			set
			{
				Set(ref _senders, value);
			}
		}

		/// <summary>
		/// Добавление отправителя
		/// </summary>
		public IAsyncCommand AddSenderCommand
		{
			get
			{
				return _addSender ?? (_addSender = new AsyncCommand(AddSender));
			}
		}

		/// <summary>
		/// Редактирование отправителя
		/// </summary>
		public IAsyncCommand EditSenderCommand
		{
			get
			{
				return _editSender ?? (_editSender = new AsyncCommand(EditSender));
			}
		}

		/// <summary>
		/// Удаление отправителя
		/// </summary>
		public IAsyncCommand DeleteSenderCommand
		{
			get
			{
				return _deleteSender ?? (_deleteSender = new AsyncCommand(DeleteSender));
			}
		}

		private async Task AddSender()
		{
			if (SenderEmail != null && SenderPassword != null)
			{
				var newSender = new Sender() { Email = SenderEmail, Password = PasswordCoding.Encrypt(SenderPassword) };
				_senderService.Add(newSender);
				Senders = new ObservableCollection<Sender>(new ObservableCollection<Sender>(_senderService.GetAll().Select(x => x = new Sender()
				{
					Id = x.Id,
					Email = x.Email,
					Password = PasswordCoding.Decrypt(x.Password)
				})));
				Messenger.Default.Send(newSender);
			}
		}

		private async Task EditSender()
		{
			if (SelectedSender != null)
			{
				_senderService.Edit(new Sender()
				{
					Id = SelectedSender.Id,
					Email = SelectedSender.Email,
					Password = PasswordCoding.Encrypt(SelectedSender.Password)
				});
				Messenger.Default.Send(SelectedSender);
				Senders = new ObservableCollection<Sender>(_senderService.GetAll().Select(x => new Sender()
				{
					Id = x.Id,
					Email = x.Email,
					Password = PasswordCoding.Decrypt(x.Password)
				}));

			}
		}

		private async Task DeleteSender()
		{
			if (SelectedSender != null)
			{
				_senderService.Delete(SelectedSender);
				Messenger.Default.Send(SelectedSender);
				Senders.Remove(SelectedSender);
			}
		}
	}
}
