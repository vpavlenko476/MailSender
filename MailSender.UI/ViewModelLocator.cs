using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MailSender.DAL.Services;
using MailSender.UI.ViewModel;
using MailSender.UI.Views.Services;

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

			SimpleIoc.Default.Register<IEmailSendService, EmailSendService>();
			SimpleIoc.Default.Register<IWindowService, WindowService>();
			SimpleIoc.Default.Register<MailSenderVM>();
			SimpleIoc.Default.Register<SendersVM>();
			SimpleIoc.Default.Register<SmtpVM>();
			SimpleIoc.Default.Register<RecipientsVM>();			
			SimpleIoc.Default.Register<WarningsVW>();

		}

		public MailSenderVM MailSenderVM
		{
			get { return SimpleIoc.Default.GetInstance<MailSenderVM>(); }
		}

		public SendersVM SendersVM
		{
			get { return SimpleIoc.Default.GetInstance<SendersVM>(); }
		}

		public SmtpVM SmtpVM
		{
			get { return SimpleIoc.Default.GetInstance<SmtpVM>(); }
		}
		public RecipientsVM RecipientsVM
		{
			get { return SimpleIoc.Default.GetInstance<RecipientsVM>(); }
		}
		public WarningsVW WarningsVW
		{
			get { return SimpleIoc.Default.GetInstance<WarningsVW>(); }
		}

	}
}
