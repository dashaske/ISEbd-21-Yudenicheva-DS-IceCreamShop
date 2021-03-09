using System;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace IceCreamShopBusinessLogic.BusinessLogics
{
    public class IceCreamLogic
    {
        private readonly IIceCreamStorage _icecreamStorage;
        public IceCreamLogic(IIceCreamStorage icecreamStorage)
        {
            _icecreamStorage = icecreamStorage;
        }
        public List<IceCreamViewModel> Read(IceCreamBindingModel model)
        {
            if (model == null)
            {
                return _icecreamStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<IceCreamViewModel> { _icecreamStorage.GetElement(model) };
            }
            return _icecreamStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(IceCreamBindingModel model)
        {
            var element = _icecreamStorage.GetElement(new IceCreamBindingModel { IceCreamName = model.IceCreamName });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть мороженое с таким названием");
            }
            if (model.Id.HasValue)
            {
                _icecreamStorage.Update(model);
            }
            else
            {
                _icecreamStorage.Insert(model);
            }
        }
        public void Delete(IceCreamBindingModel model)
        {
            var element = _icecreamStorage.GetElement(new IceCreamBindingModel { Id = model.Id });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _icecreamStorage.Delete(model);
        }
    }
}

