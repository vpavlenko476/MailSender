using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace MailSender.UI.ViewModel
{
	public class WarningsVW : ViewModelBase
	{
		public WarningsVW()
		{			
			Messenger.Default.Register<Exception>(this, x => 
			{
				WarningTitle = "Необработанная ошибка";
				WarningText = x.Message;
			});
		}
		private string _warningText;
		private string _warningTitle;
		public string WarningText
		{
			get { return _warningText; }
			set { Set(ref _warningText, value); }
		}

		public string WarningTitle
		{
			get { return _warningTitle; }
			set { Set(ref _warningTitle, value); }
		}
	}
}
