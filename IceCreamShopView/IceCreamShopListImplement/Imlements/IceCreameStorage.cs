using System;
using System.Linq;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopListImplement.Models;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopListImplement.Imlements
{
    public class IceCreameStorage : IIceCreamStorage
    {
        private readonly DataListSingleton source;
        public IceCreameStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<IceCreamViewModel> GetFullList()
        {
            List<IceCreamViewModel> result = new List<IceCreamViewModel>();
            foreach (var icecream in source.IceCreams)
            {
                result.Add(CreateModel(icecream));
            }
            return result;
        }
        public List<IceCreamViewModel> GetFilteredList(IceCreamBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<IceCreamViewModel> result = new List<IceCreamViewModel>();
            foreach (var icecream in source.IceCreams)
            {
                if (icecream.IceCreamName.Contains(model.IceCreamName))
                {
                    result.Add(CreateModel(icecream));
                }
            }
            return result;
        }
        public IceCreamViewModel GetElement(IceCreamBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var icecream in source.IceCreams)
            {
                if (icecream.Id == model.Id || icecream.IceCreamName ==
                model.IceCreamName)
                {
                    return CreateModel(icecream);
                }
            }
            return null;
        }
        public void Insert(IceCreamBindingModel model)
        {
            IceCream tempIceCream = new IceCream
            {
                Id = 1,
                IceCreamIngredients = new Dictionary<int, int>()
            };
            foreach (var icecream in source.IceCreams)
            {
                if (icecream.Id >= tempIceCream.Id)
                {
                    tempIceCream.Id = icecream.Id + 1;
                }
            }
            source.IceCreams.Add(CreateModel(model, tempIceCream));
        }
        public void Update(IceCreamBindingModel model)
        {
            IceCream tempIceCream = null;
            foreach (var icecream in source.IceCreams)
            {
                if (icecream.Id == model.Id)
                {
                    tempIceCream = icecream;
                }
            }
            if (tempIceCream == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempIceCream);
        }
        public void Delete(IceCreamBindingModel model)
        {
            for (int i = 0; i < source.IceCreams.Count; ++i)
            {
                if (source.IceCreams[i].Id == model.Id)
                {
                    source.IceCreams.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private IceCream CreateModel(IceCreamBindingModel model, IceCream icecream)
        {
            icecream.IceCreamName = model.IceCreamName;
            icecream.Price = model.Price;
            
            foreach (var key in icecream.IceCreamIngredients.Keys.ToList())
            {
                if (!model.IceCreamIngredients.ContainsKey(key))
                {
                    icecream.IceCreamIngredients.Remove(key);
                }
            }
            
            foreach (var icecreams in model.IceCreamIngredients)
            {
                if (icecream.IceCreamIngredients.ContainsKey(icecreams.Key))
                {
                    icecream.IceCreamIngredients[icecreams.Key] =
                    model.IceCreamIngredients[icecreams.Key].Item2;
                }
                else
                {
                    icecream.IceCreamIngredients.Add(icecreams.Key,
                    model.IceCreamIngredients[icecreams.Key].Item2);
                }
            }
            return icecream;
        }
        private IceCreamViewModel CreateModel(IceCream icecream)
        {
        Dictionary<int, (string, int)> IceCreamIngredients = new
        Dictionary<int, (string, int)>();
            foreach (var ic in icecream.IceCreamIngredients)
            {
                string icecreamName = string.Empty;
                foreach (var icecreams in source.IceCreams)
                {
                    if (ic.Key == icecreams.Id)
                    {
                        icecreamName = icecreams.IceCreamName;
                        break;
                    }
                }
                IceCreamIngredients.Add(ic.Key, (icecreamName, ic.Value));
            }
            return new IceCreamViewModel
            {
                Id = icecream.Id,
                IceCreamName = icecream.IceCreamName,
                Price = icecream.Price,
                IceCreamIngredients = IceCreamIngredients
            };
        }
    }
}
