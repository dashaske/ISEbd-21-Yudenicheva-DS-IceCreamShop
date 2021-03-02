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
    public class IngredientStorage : IIngredientStorage
    {
        private readonly FileDataListSingleton source;

        private Ingredient CreateModel(IngredientBindingModel model, Ingredient ingredient)
        {
            ingredient.IngredientName = model.IngredientName;
            return ingredient;
        }

        private IngredientViewModel CreateModel(Ingredient ingredient)
        {
            return new IngredientViewModel
            {
                Id = ingredient.Id,
                IngredientName = ingredient.IngredientName
            };
        }

        public IngredientStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<IngredientViewModel> GetFullList()
        {
            return source.Ingredients.Select(CreateModel).ToList();
        }

        public List<IngredientViewModel> GetFilteredList(IngredientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Ingredients.Where(ingredient => ingredient.IngredientName.Contains(model.IngredientName)).Select(CreateModel).ToList();
        }

        public IngredientViewModel GetElement(IngredientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var ingredient = source.Ingredients.FirstOrDefault(ingr => ingr.IngredientName == model.IngredientName || ingr.Id == model.Id);
            return ingredient != null ? CreateModel(ingredient) : null;
        }

        public void Insert(IngredientBindingModel model)
        {
            int maxId = source.Ingredients.Count > 0 ? source.Ingredients.Max(comp => comp.Id) : 0;
            var ingredient = new Ingredient { Id = maxId + 1 };
            source.Ingredients.Add(CreateModel(model, ingredient));
        }

        public void Update(IngredientBindingModel model)
        {
            var component = source.Ingredients.FirstOrDefault(comp => comp.Id == model.Id);

            if (component == null)
            {
                throw new Exception("Компонент не найден");
            }
            CreateModel(model, component);
        }

        public void Delete(IngredientBindingModel model)
        {
            var component = source.Ingredients.FirstOrDefault(comp => comp.Id == model.Id);

            if (component != null)
            {
                source.Ingredients.Remove(component);
            }
            else
            {
                throw new Exception("Компонент не найден");
            }
        }
    }
}
