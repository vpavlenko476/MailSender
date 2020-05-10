namespace Data.Migrations
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.Context.MailSenderContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Data.Context.MailSenderContext context)
        {
            var senders = new List<SenderEntity>()
            {
                new SenderEntity(){Email="test@yandex.ru", Password="test"}
            };
            senders.ForEach(x => context.Senders.AddOrUpdate(c => new { c.Email, c.Password }, x));

            var recipients = new List<RecipientEntity>()
            {
                new RecipientEntity(){Name = "test", Email="test@yandex.ru"}
            };
            recipients.ForEach(x => context.Recipients.AddOrUpdate(c => new { c.Name, c.Email }, x));

            var hosts = new List<HostEntity>()
            {
                new HostEntity(){Server="smtp.yandex.ru",Port=25},
                new HostEntity(){Server="smtp.gmail.ru",Port=25},
            };
            hosts.ForEach(x => context.Hosts.AddOrUpdate(c => new { c.Server, c.Port }, x));
        }
    }
}
