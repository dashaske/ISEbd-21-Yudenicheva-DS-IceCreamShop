using System;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopBusinessLogic.BindingModels;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using IceCreamDatabaseImplement.Models;
using System.Windows.Forms;

namespace IceCreamDatabaseImplement.Implements
{
    public class IceCreamStorage : IIceCreamStorage
    {
        private IceCream CreateModel(IceCreamBindingModel model, IceCream iceCream, IceCreamDatabase context)
        {
            iceCream.IceCreamName = model.IceCreamName;
            iceCream.Price = model.Price;
            if (iceCream.Id == 0)
            {
                context.IceCreams.Add(iceCream);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var IceCreamIngredients = context.IceCreamIngredients
                    .Where(rec => rec.IceCreamId == model.Id.Value)
                    .ToList();

                context.IceCreamIngredients.RemoveRange(IceCreamIngredients
                    .Where(rec => !model.IceCreamIngredients.ContainsKey(rec.IngredientId))
                    .ToList());
                context.SaveChanges();

                foreach (var updateIngredient in IceCreamIngredients)
                {
                    updateIngredient.Count = model.IceCreamIngredients[updateIngredient.IngredientId].Item2;
                    model.IceCreamIngredients.Remove(updateIngredient.IngredientId);
                }
                context.SaveChanges();
            }


            foreach (var IceCreamIngredient in model.IceCreamIngredients)
            {
                context.IceCreamIngredients.Add(new IceCreamIngredient
                {
                    IceCreamId = iceCream.Id,
                    IngredientId = IceCreamIngredient.Key,
                    Count = IceCreamIngredient.Value.Item2
                });
                context.SaveChanges();
            }

            return iceCream;
        }

        public List<IceCreamViewModel> GetFullList()
        {
            using (var context = new IceCreamDatabase())
            {
                return context.IceCreams
                    .Include(rec => rec.IceCreamIngredients)
                    .ThenInclude(rec => rec.Ingredient)
                    .ToList()
                    .Select(rec => new IceCreamViewModel
                    {
                        Id = rec.Id,
                        IceCreamName = rec.IceCreamName,
                        Price = rec.Price,
                        IceCreamIngredients = rec.IceCreamIngredients
                            .ToDictionary(recIceCreamIngredients => recIceCreamIngredients.IngredientId,
                            recIceCreamIngredients => (recIceCreamIngredients.Ingredient?.IngredientName,
                            recIceCreamIngredients.Count))
                    })
                    .ToList();
            }
        }

        public List<IceCreamViewModel> GetFilteredList(IceCreamBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new IceCreamDatabase())
            {
                return context.IceCreams
                    .Include(rec => rec.IceCreamIngredients)
                    .ThenInclude(rec => rec.Ingredient)
                    .Where(rec => rec.IceCreamName.Contains(model.IceCreamName))
                    .ToList()
                    .Select(rec => new IceCreamViewModel
                    {
                        Id = rec.Id,
                        IceCreamName = rec.IceCreamName,
                        Price = rec.Price,
                        IceCreamIngredients = rec.IceCreamIngredients
                            .ToDictionary(recIceCreamIngredient => recIceCreamIngredient.IngredientId,
                            recIceCreamIngredient => (recIceCreamIngredient.Ingredient?.IngredientName,
                            recIceCreamIngredient.Count))
                    })
                    .ToList();
            }
        }

        public IceCreamViewModel GetElement(IceCreamBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new IceCreamDatabase())
            {
                var IceCream = context.IceCreams
                    .Include(rec => rec.IceCreamIngredients)
                    .ThenInclude(rec => rec.Ingredient)
                    .FirstOrDefault(rec => rec.IceCreamName == model.IceCreamName ||
                    rec.Id == model.Id);

                return IceCream != null ?
                    new IceCreamViewModel
                    {
                        Id = IceCream.Id,
                        IceCreamName = IceCream.IceCreamName,
                        Price = IceCream.Price,
                        IceCreamIngredients = IceCream.IceCreamIngredients
                            .ToDictionary(recIceCreamIngredient => recIceCreamIngredient.IngredientId,
                            recIceCreamIngredient => (recIceCreamIngredient.Ingredient?.IngredientName,
                            recIceCreamIngredient.Count))
                    } :
                    null;
            }
        }

        public void Insert(IceCreamBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new IceCream(), context);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(IceCreamBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var secure = context.IceCreams.FirstOrDefault(rec => rec.Id == model.Id);

                        if (secure == null)
                        {
                            throw new Exception("Продукт не найден");
                        }

                        context.IceCreams.Add(CreateModel(model, new IceCream(), context));
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(IceCreamBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                var IceCream = context.IceCreams.FirstOrDefault(rec => rec.Id == model.Id);

                if (IceCream == null)
                {
                    throw new Exception("Компонент не найден");
                }

                context.IceCreams.Remove(IceCream);
                context.SaveChanges();
            }
        }
    }
}
