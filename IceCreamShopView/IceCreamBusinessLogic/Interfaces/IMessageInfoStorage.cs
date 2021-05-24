using System;
using System.Collections.Generic;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.ViewModel;

namespace IceCreamShopBusinessLogic.Interfaces
{
    public interface IMessageInfoStorage
    {
        List<MessageInfoViewModel> GetFullList();
        List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model);
        void Insert(MessageInfoBindingModel model);
        int Count();
        List<MessageInfoViewModel> GetMessagesForPage(MessageInfoBindingModel model);
    }
}
