using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamFileImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IceCreamFileImplement.Implements
{
    public class WareHouseStorage : IWareHouseStorage
    {
        private readonly FileDataListSingleton source;

        public WareHouseStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<WareHouseViewModel> GetFullList()
        {
            return source.WareHouses
                .Select(CreateModel)
                .ToList();
        }

        public List<WareHouseViewModel> GetFilteredList(WareHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.WareHouses
                .Where(rec => rec.WareHouseName.Contains(model.WareHouseName))
                .Select(CreateModel)
                .ToList();
        }

        public WareHouseViewModel GetElement(WareHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var wareHouse = source.WareHouses.FirstOrDefault(rec => rec.WareHouseName == model.WareHouseName ||
            rec.Id == model.Id);
            return wareHouse != null ? CreateModel(wareHouse) : null;
        }

        public void Insert(WareHouseBindingModel model)
        {
            int maxId = source.WareHouses.Count > 0 ? source.WareHouses.Max(rec => rec.Id) : 0;
            var wareHouse = new WareHouse
            {
                Id = maxId + 1,
                WareHouseIngredients = new Dictionary<int, int>(),
                DateCreate = DateTime.Now
            };
            source.WareHouses.Add(CreateModel(model, wareHouse));
        }

        public void Update(WareHouseBindingModel model)
        {
            var wareHouse = source.WareHouses.FirstOrDefault(rec => rec.Id == model.Id);

            if (wareHouse == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, wareHouse);
        }

        public void Delete(WareHouseBindingModel model)
        {
            var wareHouse = source.WareHouses.FirstOrDefault(rec => rec.Id == model.Id);

            if (wareHouse == null)
            {
                throw new Exception("Элемент не найден");
            }
            source.WareHouses.Remove(wareHouse);
        }

        private WareHouse CreateModel(WareHouseBindingModel model, WareHouse wareHouse)
        {
            wareHouse.WareHouseName = model.WareHouseName;
            wareHouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;
            // удаляем убранные
            foreach (var key in wareHouse.WareHouseIngredients.Keys.ToList())
            {
                if (!model.WareHouseIngredients.ContainsKey(key))
                {
                    wareHouse.WareHouseIngredients.Remove(key);
                }
            }
            // обновляем существуюущие и добавляем новые
            foreach (var ingredient in model.WareHouseIngredients)
            {
                if (wareHouse.WareHouseIngredients.ContainsKey(ingredient.Key))
                {
                    wareHouse.WareHouseIngredients[ingredient.Key] =
                    model.WareHouseIngredients[ingredient.Key].Item2;
                }
                else
                {
                    wareHouse.WareHouseIngredients.Add(ingredient.Key,
                    model.WareHouseIngredients[ingredient.Key].Item2);
                }
            }
            return wareHouse;
        }

        private WareHouseViewModel CreateModel(WareHouse wareHouse)
        {
            // требуется дополнительно получить список компонентов для изделия с названиями и их количество
            Dictionary<int, (string, int)> wareHouseIngredients = new Dictionary<int, (string, int)>();

            foreach (var wareHouseIngredient in wareHouse.WareHouseIngredients)
            {
                string ingredientName = string.Empty;
                foreach (var ingredient in source.Ingredients)
                {
                    if (wareHouseIngredient.Key == ingredient.Id)
                    {
                        ingredientName = ingredient.IngredientName;
                        break;
                    }
                }
                wareHouseIngredients.Add(wareHouseIngredient.Key, (ingredientName, wareHouseIngredient.Value));
            }
            return new WareHouseViewModel
            {
                Id = wareHouse.Id,
                WareHouseName = wareHouse.WareHouseName,
                ResponsiblePersonFCS = wareHouse.ResponsiblePersonFCS,
                DateCreate = wareHouse.DateCreate,
                WareHouseIngredients = wareHouseIngredients
            };
        }

        public bool CheckAndTake(int count, Dictionary<int, (string, int)> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                int requiredCount = ingredient.Value.Item2 * count;
                int availableCount = source.WareHouses
                    .Where(rec => rec.WareHouseIngredients.ContainsKey(ingredient.Key))
                    .Sum(rec => rec.WareHouseIngredients[ingredient.Key]);
                if (availableCount < requiredCount)
                {
                    return false;
                }
            }
            foreach (var ingredient in ingredients)
            {
                int requiredCount = ingredient.Value.Item2 * count;
                List<WareHouse> availableStoreHouses = source.WareHouses
                    .Where(rec => rec.WareHouseIngredients.ContainsKey(ingredient.Key))
                    .ToList();
                foreach (var storeHouse in availableStoreHouses)
                {
                    int availableCount = storeHouse.WareHouseIngredients[ingredient.Key];
                    if (availableCount <= requiredCount)
                    {
                        requiredCount = requiredCount - availableCount;
                        storeHouse.WareHouseIngredients.Remove(ingredient.Key);
                    }
                    else
                    {
                        storeHouse.WareHouseIngredients[ingredient.Key] -= requiredCount;
                        requiredCount = 0;
                    }
                    if (requiredCount == 0)
                    {
                        break;
                    }
                }
            }
            return true;
        }
    }
}
