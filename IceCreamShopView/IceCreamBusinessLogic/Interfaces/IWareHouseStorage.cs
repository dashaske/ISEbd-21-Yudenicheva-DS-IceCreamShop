using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopBusinessLogic.Interfaces
{
    public interface IWareHouseStorage
    {
        List<WareHouseViewModel> GetFullList();
        List<WareHouseViewModel> GetFilteredList(WareHouseBindingModel model);
        WareHouseViewModel GetElement(WareHouseBindingModel model);
        void Insert(WareHouseBindingModel model);
        void Update(WareHouseBindingModel model);
        void Delete(WareHouseBindingModel model);
        bool CheckAndTake(int IceCreamId, int Count);
    }
}
