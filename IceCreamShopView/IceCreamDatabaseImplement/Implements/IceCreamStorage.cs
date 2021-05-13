using System;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopBusinessLogic.BindingModel;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using IceCreamDatabaseImplement.Models;
using System.Windows.Forms;

namespace IceCreamDatabaseImplement.Implements
{
    public class IceCreamStorage : IIceCreamStorage
    {
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
                .ToDictionary(recPC => recPC.IngredientId, recPC =>
                (recPC.IceCream?.IceCreamName, recPC.Count))
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
                .ToDictionary(recPC => recPC.IngredientId, recPC =>
                (recPC.IceCream?.IceCreamName, recPC.Count))
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
                var ingredient = context.IceCreams
                .Include(rec => rec.IceCreamIngredients)
                .ThenInclude(rec => rec.Ingredient)
                .FirstOrDefault(rec => rec.IceCreamName.Equals(model.IceCreamName) || rec.Id
                == model.Id);
                return ingredient != null ?
                new IceCreamViewModel
                {
                    Id = ingredient.Id,
                    IceCreamName = ingredient.IceCreamName,
                    Price = ingredient.Price,
                    IceCreamIngredients = ingredient.IceCreamIngredients
                .ToDictionary(recPC => recPC.IngredientId, recPC =>
                (recPC.IceCream?.IceCreamName, recPC.Count))
                } : null;
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
                        IceCream icecream = CreateModel(model, new IceCream());
                        context.IceCreams.Add(icecream);
                        context.SaveChanges();
                        icecream = CreateModel(model, icecream, context);

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
                        var element = context.IceCreams.FirstOrDefault(rec => rec.Id ==
                        model.Id);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }
                        CreateModel(model, element, context);
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
                IceCream element = context.IceCreams.FirstOrDefault(rec => rec.Id ==
                model.Id);
                if (element != null)
                {
                    context.IceCreams.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private IceCream CreateModel(IceCreamBindingModel model, IceCream icecream)
        {
            icecream.IceCreamName = model.IceCreamName;
            icecream.Price = model.Price;
            return icecream;
        }

        private IceCream CreateModel(IceCreamBindingModel model, IceCream icecream,
        IceCreamDatabase context)
        {
            icecream.IceCreamName = model.IceCreamName;
            icecream.Price = model.Price;
            if (model.Id.HasValue)
            {
                var icecreamIngredients = context.IceCreamIngredients.Where(rec =>
                rec.IceCreamId == model.Id.Value).ToList();
                context.IceCreamIngredients.RemoveRange(icecreamIngredients.Where(rec =>
                !model.IceCreamIngredients.ContainsKey(rec.IngredientId)).ToList());
                context.SaveChanges();
                foreach (var updateComponent in icecreamIngredients)
                {
                    updateComponent.Count =
                    model.IceCreamIngredients[updateComponent.IngredientId].Item2;
                    model.IceCreamIngredients.Remove(updateComponent.IngredientId);
                }
                context.SaveChanges();
            }
            foreach (var pc in model.IceCreamIngredients)
            {
                context.IceCreamIngredients.Add(new IceCreamIngredient
                {
                    IceCreamId = icecream.Id,
                    IngredientId = pc.Key,
                    Count = pc.Value.Item2,
                });
                context.SaveChanges();                             
            }
            return icecream;
        }
    }
}
