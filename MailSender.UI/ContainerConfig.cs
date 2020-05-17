using Autofac;
using Data.Context;
using Entities;
using MailSender.UI.Views.Services;
using System.Linq;
using System.Reflection;
using Data.Repositories.Abstract;
using MailSender.UI.ViewModel;
using Autofac.Core.Lifetime;

namespace MailSender.UI
{
	public class ContainerConfig
	{
		private static ContainerBuilder _rootScope;
		
		public ContainerConfig()
		{			
			var builder = new ContainerBuilder();
			builder.RegisterType<WindowService>().As<IWindowService>();
			builder.RegisterType<MailSenderContext>();
			builder.RegisterType<BaseRepo<Recipient2MessageEntity>>().As<IBaseRepo<Recipient2MessageEntity>>();
			builder.RegisterType<BaseRepo<RecipientEntity>>().As<IBaseRepo<RecipientEntity>>();
			builder.RegisterType<BaseRepo<HostEntity>>().As<IBaseRepo<HostEntity>>();
			builder.RegisterType<BaseRepo<SenderEntity>>().As<IBaseRepo<SenderEntity>>();
			builder.RegisterType<BaseRepo<MessageEntity>>().As<IBaseRepo<MessageEntity>>();
			builder.RegisterType<MailSenderVM>();
			builder.RegisterType<SendersVM>();
			builder.RegisterType<HostVM>();
			builder.RegisterType<RecipientsVM>();			

			builder.RegisterAssemblyTypes(Assembly.Load(nameof(Services)))
				.Where(c => !(c.Namespace.Contains("Exceptions")))
				.As(c => c.GetInterfaces().FirstOrDefault(i => i.Name == "I" + c.Name));
			_rootScope = builder;			
		}

		public static IContainer Configure()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<WindowService>().As<IWindowService>();
			builder.RegisterType<MailSenderContext>();
			builder.RegisterType<BaseRepo<Recipient2MessageEntity>>().As<IBaseRepo<Recipient2MessageEntity>>();
			builder.RegisterType<BaseRepo<RecipientEntity>>().As<IBaseRepo<RecipientEntity>>();
			builder.RegisterType<BaseRepo<HostEntity>>().As<IBaseRepo<HostEntity>>();
			builder.RegisterType<BaseRepo<SenderEntity>>().As<IBaseRepo<SenderEntity>>();
			builder.RegisterType<BaseRepo<MessageEntity>>().As<IBaseRepo<MessageEntity>>();
			builder.RegisterType<MailSenderVM>();
			builder.RegisterType<SendersVM>();
			builder.RegisterType<HostVM>();
			builder.RegisterType<RecipientsVM>();			

			builder.RegisterAssemblyTypes(Assembly.Load(nameof(Services)))
				.Where(c => !(c.Namespace.Contains("Exceptions")))
				.As(c => c.GetInterfaces().FirstOrDefault(i => i.Name == "I" + c.Name));
			return builder.Build();
			
		}

		public MailSenderVM MailSenderVM
		{
			get 
			{
				var container = Configure().BeginLifetimeScope();
				return container.Resolve<MailSenderVM>(); 
			}
		}

		public SendersVM SendersVM
		{
			get { return _rootScope.Build().Resolve<SendersVM>(); }
		}

		public HostVM SmtpVM
		{
			get { return _rootScope.Build().Resolve<HostVM>(); }
		}
		public RecipientsVM RecipientsVM
		{
			get { return _rootScope.Build().Resolve<RecipientsVM>(); }
		}		
	}
}

					
	