using System;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace IceCreamShopBusinessLogic.Interfaces
{
    public interface IIceCreamStorage
    {
        List<IceCreamViewModel> GetFullList();
        List<IceCreamViewModel> GetFilteredList(IceCreamBindingModel model);
        IceCreamViewModel GetElement(IceCreamBindingModel model);
        void Insert(IceCreamBindingModel model);
        void Update(IceCreamBindingModel model);
        void Delete(IceCreamBindingModel model);
    }
}
