using AsyncAwaitBestPractices.MVVM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MailSender.DAL.Models;
using MailSender.DAL.Repos;
using MailSender.DAL.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MailSender.UI.ViewModel
{
	public class SendersVM : ViewModelBase
	{
		private ObservableCollection<Sender> _senders;
		private string _senderEmail;
		private string _senderPassword;
		private Sender _sender;
		private IAsyncCommand _addSender;
		private IAsyncCommand _editSender;
		private IAsyncCommand _deleteSender;
		public SendersVM()
		{

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
				if(Regex.IsMatch(value, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
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
				if(!string.IsNullOrEmpty(value))
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
				using (var dbSender = new BaseRepo<Sender>())
				{
					return _senders ?? (_senders = new ObservableCollection<Sender>(dbSender.GetAll().Select(x => x = new Sender()
					{
						Id = x.Id,
						Email = x.Email,
						Password = PasswordCoding.Decrypt(x.Password),
						Messages = x.Messages
					})));
				}
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
			if(SenderEmail!=null && SenderPassword!=null)
			{
				using (var dbSender = new BaseRepo<Sender>())
				{
					var newSender = new Sender() { Email = SenderEmail, Password = PasswordCoding.Encrypt(SenderPassword) };
					dbSender.Add(newSender);
					Senders.Add(new Sender() { Email = SenderEmail, Password = SenderPassword });
					Messenger.Default.Send(newSender);					
				}
			}			
		}

		private async Task EditSender()
		{
			if (SelectedSender!=null)
			{
				using (var dbSender = new BaseRepo<Sender>())
				{
					dbSender.Save(new Sender()
					{
						Id = SelectedSender.Id,
						Email = SelectedSender.Email,
						Password = PasswordCoding.Encrypt(SelectedSender.Password),
						Messages = SelectedSender.Messages,
					});
					Senders = new ObservableCollection<Sender>(dbSender.GetAll().Select(x => new Sender()
					{
						Id = x.Id,
						Email = x.Email,
						Password = PasswordCoding.Decrypt(x.Password),
						Messages = x.Messages
					}));
					Messenger.Default.Send(SelectedSender);
				}
			}			
		}

		private async Task DeleteSender()
		{
			if(SelectedSender != null)
			{
				using (var dbSender = new BaseRepo<Sender>())
				{
					dbSender.Delete(SelectedSender);
					Senders.Remove(SelectedSender);
					Messenger.Default.Send(SelectedSender);
				}
			}			
		}
	}
}
