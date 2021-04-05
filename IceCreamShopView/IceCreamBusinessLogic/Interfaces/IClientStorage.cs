using System;
using System.Collections.Generic;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.ViewModel;
using System.Text;

namespace IceCreamShopBusinessLogic.Interfaces
{
    public interface IClientStorage
    {
        List<ClientViewModel> GetFullList();

        List<ClientViewModel> GetFilteredList(ClientBindingModel model);

        ClientViewModel GetElement(ClientBindingModel model);

        void Insert(ClientBindingModel model);

        void Update(ClientBindingModel model);

        void Delete(ClientBindingModel model);
    }
}
