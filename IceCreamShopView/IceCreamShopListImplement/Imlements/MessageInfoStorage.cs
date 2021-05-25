using System;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModel;
using IceCreamShopListImplement.Models;
using IceCreamShopListImplement.Imlements;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopListImplement.Imlements
{
    public class MessageInfoStorage : IMessageInfoStorage
    {
        private readonly DataListSingleton source;

        public MessageInfoStorage()
        {
            source = DataListSingleton.GetInstance();
        }

        private MessageInfo CreateModel(MessageInfoBindingModel model, MessageInfo messageInfo)
        {
            messageInfo.ClientId = model.ClientId;
            messageInfo.Subject = model.Subject;
            messageInfo.Body = model.Body;
            messageInfo.DateDelivery = model.DateDelivery;

            return messageInfo;
        }

        private MessageInfoViewModel CreateModel(MessageInfo messageInfo)
        {
            return new MessageInfoViewModel
            {
                MessageId = messageInfo.MessageId,
                SenderName = messageInfo.SenderName,
                Subject = messageInfo.Subject,
                Body = messageInfo.Body,
                DateDelivery = messageInfo.DateDelivery
            };
        }

        public List<MessageInfoViewModel> GetFullList()
        {
            List<MessageInfoViewModel> result = new List<MessageInfoViewModel>();
            foreach (var messageInfo in source.MessageInfoes)
            {
                result.Add(CreateModel(messageInfo));
            }
            return result;
        }

        public List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            List<MessageInfoViewModel> result = new List<MessageInfoViewModel>();
            foreach (var messageInfo in source.MessageInfoes)
            {
                if (messageInfo.Subject.Contains(model.Subject))
                {
                    result.Add(CreateModel(messageInfo));
                }
            }
            return result;
        }

        public void Insert(MessageInfoBindingModel model)
        {
            var tempMessageInfo = new MessageInfo
            {
                MessageId = "1"
            };
            foreach (var messageInfo in source.MessageInfoes)
            {
                if (Convert.ToInt32(messageInfo.MessageId) >= Convert.ToInt32(tempMessageInfo.MessageId))
                {
                    tempMessageInfo.MessageId = messageInfo.MessageId + 1;
                }
            }
            source.MessageInfoes.Add(CreateModel(model, tempMessageInfo));
        }
    }
}
