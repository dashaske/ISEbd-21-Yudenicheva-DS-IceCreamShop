using System;
using System.Collections.Generic;
using IceCreamFileImplement.Models;
using IceCreamShopBusinessLogic.BindingModel;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using System.Linq;
using System.Text;

namespace IceCreamFileImplement.Implements
{
    public class IceCreamStorage : IIceCreamStorage
    {
        private readonly FileDataListSingleton source;

        private IceCream CreateModel(IceCreamBindingModel model, IceCream iceCream)
        {
            iceCream.IceCreamName = model.IceCreamName;
            iceCream.Price = model.Price;

            foreach (var key in iceCream.IceCreamIngredients.Keys.ToList())
            {
                if (!model.IceCreamIngredients.ContainsKey(key))
                {
                    iceCream.IceCreamIngredients.Remove(key);
                }
            }

            foreach (var ingredient in model.IceCreamIngredients)
            {
                if (iceCream.IceCreamIngredients.ContainsKey(ingredient.Key))
                {
                    iceCream.IceCreamIngredients[ingredient.Key] = model.IceCreamIngredients[ingredient.Key].Item2;
                }
                else
                {
                    iceCream.IceCreamIngredients.Add(ingredient.Key, model.IceCreamIngredients[ingredient.Key].Item2);
                }
            }
            return iceCream;
        }

        private IceCreamViewModel CreateModel(IceCream iceCream)
        {
            return new IceCreamViewModel
            {
                Id = iceCream.Id,
                IceCreamName = iceCream.IceCreamName,
                Price = iceCream.Price,
                IceCreamIngredients = iceCream.IceCreamIngredients.ToDictionary(icecreamIngredient => icecreamIngredient.Key, icecreamIngredient =>
                (source.Ingredients.FirstOrDefault(Ingredient => Ingredient.Id == icecreamIngredient.Key)?.IngredientName, icecreamIngredient.Value))
            };
        }

        public IceCreamStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<IceCreamViewModel> GetFullList()
        {
            return source.IceCreams.Select(CreateModel).ToList();
        }

        public List<IceCreamViewModel> GetFilteredList(IceCreamBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.IceCreams.Where(recPizza => recPizza.IceCreamName.Contains(model.IceCreamName)).Select(CreateModel).ToList();
        }

        public IceCreamViewModel GetElement(IceCreamBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var iceCream = source.IceCreams.FirstOrDefault(recIceCream => recIceCream.IceCreamName == model.IceCreamName || recIceCream.Id == model.Id);

            return iceCream != null ? CreateModel(iceCream) : null;
        }

        public void Insert(IceCreamBindingModel model)
        {
            int maxId = source.IceCreams.Count > 0 ? source.IceCreams.Max(recPizza => recPizza.Id) : 0;
            var iceCream = new IceCream { Id = maxId + 1, IceCreamIngredients = new Dictionary<int, int>() };
            source.IceCreams.Add(CreateModel(model, iceCream));
        }

        public void Update(IceCreamBindingModel model)
        {
            var iceCream = source.IceCreams.FirstOrDefault(recIceCream => recIceCream.Id == model.Id);

            if (iceCream == null)
            {
                throw new Exception("Мороженое не найден");
            }
            CreateModel(model, iceCream);
        }

        public void Delete(IceCreamBindingModel model)
        {
            var iceCream = source.IceCreams.FirstOrDefault(recIceCream => recIceCream.Id == model.Id);

            if (iceCream != null)
            {
                source.IceCreams.Remove(iceCream);
            }
            else
            {
                throw new Exception("Мороженое не найдено");
            }
        }
    }
}
