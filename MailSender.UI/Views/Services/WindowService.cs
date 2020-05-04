using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MailSender.UI.Views.Services
{
	class WindowService : IWindowService
	{
		public void showWindow(object viewModel)
		{
			var win = new Window();
			win.Width = 500;
			win.Height = 400;
			win.Content = viewModel;
			win.Show();
		}
	}
}
