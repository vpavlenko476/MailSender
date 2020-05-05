namespace MailSender.DAL.Migrations
{
    using Models= MailSender.DAL.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MailSender.DAL.Models.Context.MailSenderContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MailSender.DAL.Models.Context.MailSenderContext context)
        {
            var senders = new List<Models.Sender>()
            {
                new Models.Sender(){Email="test@yandex.ru", Password="test"}
            };
            senders.ForEach(x => context.Senders.AddOrUpdate(c => new { c.Email, c.Password }, x));

            var recipients = new List<Models.Recipient>()
            {
                new Models.Recipient(){Name = "test", Email="test@yandex.ru"}
            };
            recipients.ForEach(x => context.Recipients.AddOrUpdate(c => new { c.Name, c.Email}, x));

            var hosts = new List<Models.Host>()
            {
                new Models.Host(){Server="smtp.yandex.ru",Port=25},
                new Models.Host(){Server="smtp.gmail.ru",Port=25},
            };
            hosts.ForEach(x => context.Hosts.AddOrUpdate(c => new { c.Server, c.Port }, x));
        }
    }
}
