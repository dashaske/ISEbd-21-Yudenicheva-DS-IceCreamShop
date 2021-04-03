using System;
using System.Collections.Generic;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopBusinessLogic.BindingModel;
using System.Linq;
using IceCreamDatabaseImplement.Models;
using System.Text;

namespace IceCreamDatabaseImplement.Implements
{
    public class IngredientStorage : IIngredientStorage
    {
        public List<IngredientViewModel> GetFullList()
        {
            using (var context = new IceCreamDatabase())
            {
                return context.Ingredients.Select(rec => new IngredientViewModel
                {
                    Id = rec.Id,
                    IngredientName = rec.IngredientName
                })
                .ToList();
            }
        }

        public List<IngredientViewModel> GetFilteredList(IngredientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new IceCreamDatabase())
            {
                return context.Ingredients
                .Where(rec => rec.IngredientName.Contains(model.IngredientName))
                .Select(rec => new IngredientViewModel
                {
                    Id = rec.Id,
                    IngredientName = rec.IngredientName
                })
                .ToList();
            }
        }

        public IngredientViewModel GetElement(IngredientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new IceCreamDatabase())
            {
                var ingredient = context.Ingredients
                .FirstOrDefault(rec => rec.IngredientName == model.IngredientName ||
                rec.Id == model.Id);
                return ingredient != null ?
                new IngredientViewModel
                {
                    Id = ingredient.Id,
                    IngredientName = ingredient.IngredientName
                } :
                null;
            }
        }

        public void Insert(IngredientBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                context.Ingredients.Add(CreateModel(model, new Ingredient()));
                context.SaveChanges();
            }
        }

        public void Update(IngredientBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                var element = context.Ingredients.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }

        public void Delete(IngredientBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                Ingredient element = context.Ingredients.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Ingredients.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Ingredient CreateModel(IngredientBindingModel model, Ingredient ingredient)
        {
            ingredient.IngredientName = model.IngredientName;
            return ingredient;
        }
    }
}
