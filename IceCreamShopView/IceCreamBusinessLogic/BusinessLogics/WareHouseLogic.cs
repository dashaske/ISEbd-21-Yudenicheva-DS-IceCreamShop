using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopBusinessLogic.BusinessLogics
{
    public class WareHouseLogic
    {
        private readonly IWareHouseStorage _wareHouseStorage;

        private readonly IIngredientStorage _ingredientStorage;

        public WareHouseLogic(IWareHouseStorage wareHouseStorage, IIngredientStorage inredientsStorage)
        {
            _wareHouseStorage = wareHouseStorage;
            _ingredientStorage = inredientsStorage;
        }
        public List<WareHouseViewModel> Read(WareHouseBindingModel model)
        {
            if (model == null)
            {
                return _wareHouseStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<WareHouseViewModel> { _wareHouseStorage.GetElement(model) };
            }
            return _wareHouseStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(WareHouseBindingModel model)
        {
            var element = _wareHouseStorage.GetElement(new WareHouseBindingModel
            {
                WareHouseName = model.WareHouseName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            if (model.Id.HasValue)
            {
                _wareHouseStorage.Update(model);
            }
            else
            {
                _wareHouseStorage.Insert(model);
            }
        }

        public void Delete(WareHouseBindingModel model)
        {
            var element = _wareHouseStorage.GetElement(new WareHouseBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _wareHouseStorage.Delete(model);
        }

        public void Replenishment(WareHouseReplenishmentBindingModel model)
        {
            var wareHouse = _wareHouseStorage.GetElement(new WareHouseBindingModel
            {
                Id = model.WareHouseId
            });

            var ingredient = _ingredientStorage.GetElement(new IngredientBindingModel
            {
                Id = model.IngredientId
            });
            if (wareHouse == null)
            {
                throw new Exception("Не найден склад");
            }
            if (ingredient == null)
            {
                throw new Exception("Не найден компонент");
            }
            if (wareHouse.WareHouseIngredients.ContainsKey(model.IngredientId))
            {
                wareHouse.WareHouseIngredients[model.IngredientId] =
                    (ingredient.IngredientName, wareHouse.WareHouseIngredients[model.IngredientId].Item2 + model.Count);
            }
            else
            {
                wareHouse.WareHouseIngredients.Add(ingredient.Id, (ingredient.IngredientName, model.Count));
            }
            _wareHouseStorage.Update(new WareHouseBindingModel
            {
                Id = wareHouse.Id,
                WareHouseName = wareHouse.WareHouseName,
                ResponsiblePersonFCS = wareHouse.ResponsiblePersonFCS,
                DateCreate = wareHouse.DateCreate,
                WareHouseIngredients = wareHouse.WareHouseIngredients
            });
        }
    }
}
