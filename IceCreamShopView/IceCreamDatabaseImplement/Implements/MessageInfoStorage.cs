using System;
using System.Collections.Generic;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModel;
using IceCreamDatabaseImplement.Models;
using System.Linq;

namespace IceCreamDatabaseImplement.Implements
{
    public class MessageInfoStorage : IMessageInfoStorage
    {
        private readonly int stringsOnPage = 7;

        public List<MessageInfoViewModel> GetFullList()
        {
            using (var context = new IceCreamDatabase())
            {
                return context.MessageInfoes
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new IceCreamDatabase())
            {
                var messageInfoes = context.MessageInfoes
                    .Where(rec => (model.ClientId.HasValue && rec.ClientId == model.ClientId) ||
                    (!model.ClientId.HasValue && rec.DateDelivery.Date == model.DateDelivery.Date) ||
                    (!model.ClientId.HasValue && model.PageNumber.HasValue) ||
                    (model.ClientId.HasValue && rec.ClientId == model.ClientId && model.PageNumber.HasValue));

                if (model.PageNumber.HasValue)
                {
                    messageInfoes = messageInfoes.Skip(stringsOnPage * (model.PageNumber.Value - 1))
                        .Take(stringsOnPage);
                }

                return messageInfoes.Select(CreateModel).ToList();
            }
        }

        public void Insert(MessageInfoBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                MessageInfo element = context.MessageInfoes.FirstOrDefault(rec => rec.MessageId == model.MessageId);
                if (element != null)
                {
                    return;
                }

                context.MessageInfoes.Add(new MessageInfo
                {
                    MessageId = model.MessageId,
                    ClientId = model.ClientId,
                    SenderName = model.FromMailAddress,
                    DateDelivery = model.DateDelivery,
                    Subject = model.Subject,
                    Body = model.Body
                });
                context.SaveChanges();
            }
        }

        public MessageInfoViewModel CreateModel(MessageInfo messageInfo)
        {
            return new MessageInfoViewModel
            {
                MessageId = messageInfo.MessageId,
                SenderName = messageInfo.SenderName,
                DateDelivery = messageInfo.DateDelivery,
                Subject = messageInfo.Subject,
                Body = messageInfo.Body
            };
        }
    }
}
