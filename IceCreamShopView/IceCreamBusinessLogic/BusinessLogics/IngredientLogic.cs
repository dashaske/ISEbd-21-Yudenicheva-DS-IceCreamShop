using System;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace IceCreamShopBusinessLogic.BusinessLogics
{
    public class IngredientLogic
    {
        private readonly IIngredientStorage _ingredientStorage;
        public IngredientLogic(IIngredientStorage ingredientStorage)
        {
            _ingredientStorage = ingredientStorage;
        }
        public List<IngredientViewModel> Read(IngredientBindingModel model)
        {
            if (model == null)
            {
                return _ingredientStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<IngredientViewModel> { _ingredientStorage.GetElement(model) };
            }
            return _ingredientStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(IngredientBindingModel model)
        {
            var element = _ingredientStorage.GetElement(new IngredientBindingModel { IngredientName = model.IngredientName });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть ингредиент с таким названием");
            }
            if (model.Id.HasValue)
            {
                _ingredientStorage.Update(model);
            }
            else
            {
                _ingredientStorage.Insert(model);
            }
        }
        public void Delete(IngredientBindingModel model)
        {
            var element = _ingredientStorage.GetElement(new IngredientBindingModel { Id = model.Id });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _ingredientStorage.Delete(model);
        }
    }
}
