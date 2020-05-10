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
	public class HostVM: ViewModelBase
	{
		private IHostService _hostService;
		private ObservableCollection<Host> _hosts;		
		private string _smtpToCreate;
		private string _portToCreate;
		private IAsyncCommand _addHost;
		private IAsyncCommand _editHost;
		private IAsyncCommand _deleteHost;

		public HostVM(IHostService service)
		{
			_hostService = service;
		}
		/// <summary>
		/// Хосты 
		/// </summary>
		public ObservableCollection<Host> Hosts 
		{
			get
			{
				if(_hosts==null)
				{
					_hosts = new ObservableCollection<Host>(_hostService.GetAll());
				}
				return _hosts;
				
			}
			set
			{
				Set(ref _hosts, value);
			}			
		}		

		/// <summary>
		/// Выбранный в dataGrid хост
		/// </summary>
		public Host SelectedHost { get; set; }

		/// <summary>
		/// Сервер создаваемого хоста
		/// </summary>
		public string SmtpToCreate 
		{
			get { return _smtpToCreate; }
			set 
			{
				if (!string.IsNullOrEmpty(value))
				{
					Set(ref _smtpToCreate, value);
				}
				else _smtpToCreate = null; 
			}
		}

		/// <summary>
		/// Порт создаваемого хоста
		/// </summary>
		public string PortToCreate
		{
			get { return _portToCreate; }
			set 
			{
				if (Regex.IsMatch(value, @"^\d+$"))
				{
					Set(ref _portToCreate, value);
				}
				else _portToCreate = null; 
			}
		}

		/// <summary>
		/// Добавление хоста
		/// </summary>
		public IAsyncCommand AddHostCommand
		{
			get
			{
				return _addHost ?? (_addHost = new AsyncCommand(Add));
			}
		}

		/// <summary>
		/// Редактирование хоста
		/// </summary>
		public IAsyncCommand EditHostCommand
		{
			get
			{
				return _editHost ?? (_editHost = new AsyncCommand(Edit));
			}
		}

		/// <summary>
		/// Удаление зоста
		/// </summary>
		public IAsyncCommand DeleteHostCommand
		{
			get
			{
				return _deleteHost ?? (_deleteHost = new AsyncCommand(Delete));
			}
		}

		private async Task Add()
		{
			if (PortToCreate != null && SmtpToCreate!=null)
			{
				var newHost = new Host() { Server = SmtpToCreate, Port = int.Parse(PortToCreate) };
				_hostService.Add(newHost);
				Hosts = new ObservableCollection<Host>(_hostService.GetAll());
				Messenger.Default.Send(newHost);
			}			
		}

		private async Task Edit()
		{
			if(SelectedHost!=null)
			{
				_hostService.Edit(SelectedHost);
				Messenger.Default.Send(SelectedHost);
				Hosts.Remove(SelectedHost);
				Hosts = new ObservableCollection<Host>(_hostService.GetAll());
				
			}			
		}

		private async Task Delete()
		{
			if(SelectedHost!=null)
			{
				_hostService.Delete(SelectedHost);
				Hosts.Remove(Hosts.Where(x => x.Id == SelectedHost.Id).FirstOrDefault());
				Messenger.Default.Send(SelectedHost);
			}			
		}		
	}
}
