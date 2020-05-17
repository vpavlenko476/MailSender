using Data.Context;
using Data.Repositories.Abstract;
using Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MailSender.UI.ViewModel;
using MailSender.UI.Views.Services;
using Services;
using Services.Abstract;

namespace MailSender.UI
{
	public class ViewModelLocator
	{
		public ViewModelLocator()
		{

			if (ViewModelBase.IsInDesignModeStatic)
			{

			}
			else
			{

			}

			SimpleIoc.Default.Register<IMessageSendService, MessageService>();
			SimpleIoc.Default.Register<IRecipientService, RecipientService>();
			SimpleIoc.Default.Register<IHostService, HostService>();
			SimpleIoc.Default.Register<ISenderService, SenderService>();
			SimpleIoc.Default.Register<IWindowService, WindowService>();				
			SimpleIoc.Default.Register<MailSenderContext>();
			SimpleIoc.Default.Register<IBaseRepo<Recipient2MessageEntity>, BaseRepo<Recipient2MessageEntity>>();
			SimpleIoc.Default.Register<IBaseRepo<RecipientEntity>, BaseRepo<RecipientEntity>>();
			SimpleIoc.Default.Register<IBaseRepo<HostEntity>, BaseRepo<HostEntity>>();
			SimpleIoc.Default.Register<IBaseRepo<SenderEntity>, BaseRepo<SenderEntity>>();
			SimpleIoc.Default.Register<IBaseRepo<MessageEntity>, BaseRepo<MessageEntity>>();
			SimpleIoc.Default.Register<MailSenderVM>();
			SimpleIoc.Default.Register<SendersVM>();
			SimpleIoc.Default.Register<HostVM>();
			SimpleIoc.Default.Register<RecipientsVM>();	
		}

		public MailSenderVM MailSenderVM
		{
			get { return SimpleIoc.Default.GetInstance<MailSenderVM>(); }
		}

		public SendersVM SendersVM
		{
			get { return SimpleIoc.Default.GetInstance<SendersVM>(); }
		}

		public HostVM SmtpVM
		{
			get { return SimpleIoc.Default.GetInstance<HostVM>(); }
		}
		public RecipientsVM RecipientsVM
		{
			get { return SimpleIoc.Default.GetInstance<RecipientsVM>(); }
		}	
	}
}
