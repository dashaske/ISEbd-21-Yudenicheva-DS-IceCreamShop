using System;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopListImplement.Models;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IceCreamShopListImplement.Imlements
{
    public class WareHouseStorage : IWareHouseStorage
    {
        private readonly DataListSingleton source;

        public WareHouseStorage()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<WareHouseViewModel> GetFullList()
        {
            List<WareHouseViewModel> result = new List<WareHouseViewModel>();
            foreach (var wareHouse in source.Warehouses)
            {
                result.Add(CreateModel(wareHouse));
            }
            return result;
        }

        public List<WareHouseViewModel> GetFilteredList(WareHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<WareHouseViewModel> result = new List<WareHouseViewModel>();
            foreach (var wareHouse in source.Warehouses)
            {
                if (wareHouse.WareHouseName.Contains(model.WareHouseName))
                {
                    result.Add(CreateModel(wareHouse));
                }
            }
            return result;
        }

        public WareHouseViewModel GetElement(WareHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var wareHouse in source.Warehouses)
            {
                if (wareHouse.Id == model.Id || wareHouse.WareHouseName ==
                model.WareHouseName)
                {
                    return CreateModel(wareHouse);
                }
            }
            return null;
        }

        public void Insert(WareHouseBindingModel model)
        {
            WareHouse tempWareHouse = new WareHouse
            {
                Id = 1,
                WareHouseIngredients = new Dictionary<int, int>(),
                DateCreate = DateTime.Now
            };
            foreach (var wareHouse in source.Warehouses)
            {
                if (wareHouse.Id >= tempWareHouse.Id)
                {
                    tempWareHouse.Id = wareHouse.Id + 1;
                }
            }
            source.Warehouses.Add(CreateModel(model, tempWareHouse));
        }

        public void Update(WareHouseBindingModel model)
        {
            WareHouse tempWareHouse = null;
            foreach (var wareHouse in source.Warehouses)
            {
                if (wareHouse.Id == model.Id)
                {
                    tempWareHouse = wareHouse;
                }
            }
            if (tempWareHouse == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempWareHouse);
        }

        public void Delete(WareHouseBindingModel model)
        {
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id == model.Id)
                {
                    source.Warehouses.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
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
            foreach (var ingridient in model.WareHouseIngredients)
            {
                if (wareHouse.WareHouseIngredients.ContainsKey(ingridient.Key))
                {
                    wareHouse.WareHouseIngredients[ingridient.Key] =
                    model.WareHouseIngredients[ingridient.Key].Item2;
                }
                else
                {
                    wareHouse.WareHouseIngredients.Add(ingridient.Key,
                    model.WareHouseIngredients[ingridient.Key].Item2);
                }
            }
            return wareHouse;
        }

        private WareHouseViewModel CreateModel(WareHouse wareHouse)
        {
            // требуется дополнительно получить список компонентов для изделия с названиями и их количество
            Dictionary<int, (string, int)> wareHouseComponents = new Dictionary<int, (string, int)>();

            foreach (var wareHouseComponent in wareHouse.WareHouseIngredients)
            {
                string ingridientName = string.Empty;
                foreach (var ingredient in source.Ingredients)
                {
                    if (wareHouseComponent.Key == ingredient.Id)
                    {
                        ingridientName = ingredient.IngredientName;
                        break;
                    }
                }
                wareHouseComponents.Add(wareHouseComponent.Key, (ingridientName, wareHouseComponent.Value));
            }
            return new WareHouseViewModel
            {
                Id = wareHouse.Id,
                WareHouseName = wareHouse.WareHouseName,
                ResponsiblePersonFCS = wareHouse.ResponsiblePersonFCS,
                DateCreate = wareHouse.DateCreate,
                WareHouseIngredients = wareHouseComponents
            };
        }

        public void Print()
        {
            foreach (WareHouse warehouse in source.Warehouses)
            {
                Console.WriteLine(warehouse.WareHouseIngredients + " " + warehouse.ResponsiblePersonFCS + " " + warehouse.DateCreate);
                foreach (KeyValuePair<int, int> keyValue in warehouse.WareHouseIngredients)
                {
                    string ingridientName = source.Ingredients.FirstOrDefault(ingridient => ingridient.Id == keyValue.Key).IngredientName;
                    Console.WriteLine(ingridientName + " " + keyValue.Value);
                }
            }
        }
    }
}
