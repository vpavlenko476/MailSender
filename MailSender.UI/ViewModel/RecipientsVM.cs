using AsyncAwaitBestPractices.MVVM;
using Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Services.Abstract;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MailSender.UI.ViewModel
{
	public class RecipientsVM : ViewModelBase
	{		
		private readonly IRecipientService _recipientService;
		private ObservableCollection<Recipient> _recipients;
		private string _emailToCreate;
		private string _nameToCreate;
		private IAsyncCommand _addRecipient;
		private IAsyncCommand _editRecipient;
		private IAsyncCommand _deleteRecipient;

		public RecipientsVM(IRecipientService service)
		{
			_recipientService = service;
		}

		/// <summary>
		/// Получатели 
		/// </summary>
		public ObservableCollection<Recipient> Recipients
		{
			get
			{
				if (_recipients == null)
				{
					_recipients = new ObservableCollection<Domain.Recipient>(_recipientService.GetAll());
				}
				return _recipients;
			}
			set
			{
				Set(ref _recipients, value);
			}
		}

		/// <summary>
		/// Выбранные в dataGrid получатели
		/// </summary>
		public Recipient SelectedRecipient { get; set; }

		/// <summary>
		/// Почта создаваемого получателя
		/// </summary>
		public string EmailToCreate
		{
			get { return _emailToCreate; }
			set
			{
				if (Regex.IsMatch(value, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
				{
					Set(ref _emailToCreate, value);
				}
				else _emailToCreate = null;
			}
		}

		/// <summary>
		/// Имя создаваемого получателя
		/// </summary>
		public string NameToCreate
		{
			get { return _nameToCreate; }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					Set(ref _nameToCreate, value);
				}
				else _nameToCreate = null;
			}
		}

		/// <summary>
		/// Добавление поьзователя
		/// </summary>
		public IAsyncCommand AddRecipientCommand
		{
			get
			{
				return _addRecipient ?? (_addRecipient = new AsyncCommand(Add));
			}
		}

		/// <summary>
		/// Редиактирование поьзователя
		/// </summary>
		public IAsyncCommand EditRecipientCommand
		{
			get
			{
				return _editRecipient ?? (_editRecipient = new AsyncCommand(Edit));
			}
		}

		/// <summary>
		/// Удаление поьзователя
		/// </summary>
		public IAsyncCommand DeleteRecipientCommand
		{
			get
			{
				return _deleteRecipient ?? (_deleteRecipient = new AsyncCommand(Delete));
			}
		}

		private async Task Add()
		{
			if (EmailToCreate != null && NameToCreate != null)
			{
				var newRecipient = new Recipient() { Email = EmailToCreate, Name = NameToCreate };
				_recipientService.Add(newRecipient);
				Recipients = new ObservableCollection<Recipient>(_recipientService.GetAll());
				Messenger.Default.Send(newRecipient);
			}
		}

		private async Task Edit()
		{
			if (SelectedRecipient != null)
			{
				_recipientService.Edit(SelectedRecipient);
				Messenger.Default.Send(SelectedRecipient);							
				Recipients = new ObservableCollection<Recipient>(_recipientService.GetAll());				
			}
		}

		private async Task Delete()
		{
			if (SelectedRecipient != null)
			{
				_recipientService.Delete(SelectedRecipient);
				Recipients.Remove(Recipients.Where(x => x.Id == SelectedRecipient.Id).FirstOrDefault());
				Messenger.Default.Send(SelectedRecipient);
			}
		}
	}
}
