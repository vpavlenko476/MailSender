using AsyncAwaitBestPractices.MVVM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MailSender.DAL.Models;
using MailSender.DAL.Repos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MailSender.UI.ViewModel
{
	public class RecipientsVM : ViewModelBase
	{
		private ObservableCollection<Recipient> _recipients;
		private string _emailToCreate;
		private string _nameToCreate;		
		private IAsyncCommand _addRecipient;
		private IAsyncCommand _editRecipient;
		private IAsyncCommand _deleteRecipient;

		/// <summary>
		/// Получатели 
		/// </summary>
		public ObservableCollection<Recipient> Recipients
		{
			get
			{
				if (_recipients == null)
				{
					using (var dbRecipient = new BaseRepo<Recipient>())
					{
						_recipients = new ObservableCollection<Recipient>(dbRecipient.GetAll());
					}
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
			if (EmailToCreate != null && NameToCreate!=null)
			{
				using (var dbRecipient = new BaseRepo<Recipient>())
				{
					var newRecipient = new Recipient() { Email = EmailToCreate, Name = NameToCreate };
					dbRecipient.Add(newRecipient);
					Recipients.Add(newRecipient);
					Messenger.Default.Send(newRecipient);
				}
			}			
		}

		private async Task Edit()
		{
			if(SelectedRecipient!=null)
			{
				using (var dbRecipient = new BaseRepo<Recipient>())
				{
					dbRecipient.Save(SelectedRecipient);
					Recipients.Remove(Recipients.Where(x => x.Id == SelectedRecipient.Id).FirstOrDefault());
					Recipients = new ObservableCollection<Recipient>(dbRecipient.GetAll());
					Messenger.Default.Send(SelectedRecipient);
				}
			}			
		}

		private async Task Delete()
		{
			if(SelectedRecipient!=null)
			{
				using (var RecipientHost = new BaseRepo<Recipient>())
				{
					RecipientHost.Delete(SelectedRecipient);
					Recipients.Remove(Recipients.Where(x => x.Id == SelectedRecipient.Id).FirstOrDefault());
					Messenger.Default.Send(SelectedRecipient);
				}
			}			
		}
	}
}
