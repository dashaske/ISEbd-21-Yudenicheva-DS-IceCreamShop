using Microsoft.EntityFrameworkCore;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IceCreamDatabaseImplement.Implements
{
    public class WareHouseStorage : IWareHouseStorage
    {
        private WareHouse CreateModel(WareHouseBindingModel model, WareHouse wareHouse, IceCreamDatabase context)
        {
            wareHouse.WareHouseName = model.WareHouseName;
            wareHouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;
            if (wareHouse.Id == 0)
            {
                wareHouse.DateCreate = DateTime.Now;
                context.WareHouses.Add(wareHouse);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var wareHouseIngredients = context.WareHouseIngredients
                    .Where(rec => rec.WareHouseId == model.Id.Value)
                    .ToList();

                context.WareHouseIngredients.RemoveRange(wareHouseIngredients
                    .Where(rec => !model.WareHouseIngredients.ContainsKey(rec.IngredientId))
                    .ToList());
                context.SaveChanges();

                foreach (var updateIngredient in wareHouseIngredients)
                {
                    updateIngredient.Count = model.WareHouseIngredients[updateIngredient.IngredientId].Item2;
                    model.WareHouseIngredients.Remove(updateIngredient.IngredientId);
                }
                context.SaveChanges();
            }

            foreach (var wareHouseIngredient in model.WareHouseIngredients)
            {
                context.WareHouseIngredients.Add(new WareHouseIngredient
                {
                    WareHouseId = wareHouse.Id,
                    IngredientId = wareHouseIngredient.Key,
                    Count = wareHouseIngredient.Value.Item2
                });
                context.SaveChanges();
            }

            return wareHouse;
        }

        public bool CheckAndTake(int count, Dictionary<int, (string, int)> ingredients)
        {
            using (var context = new IceCreamDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var wareHouseIngredient in ingredients)
                        {
                            int requiredCount = wareHouseIngredient.Value.Item2 * count;
                            int countInWareHouses = context.WareHouseIngredients
                                .Where(rec => rec.IngredientId == wareHouseIngredient.Key)
                                .Sum(rec => rec.Count);
                            if (requiredCount > countInWareHouses)
                            {
                                throw new Exception("На складе недостаточно ингредиентов");
                            }

                            IEnumerable<WareHouseIngredient> wareHouseIngredients = context.WareHouseIngredients
                                .Where(rec => rec.IngredientId == wareHouseIngredient.Key);
                            foreach (var ingredient in wareHouseIngredients)
                            {
                                if (ingredient.Count <= requiredCount)
                                {
                                    requiredCount -= ingredient.Count;
                                    context.WareHouseIngredients.Remove(ingredient);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    ingredient.Count -= requiredCount;
                                    context.SaveChanges();
                                    requiredCount = 0;
                                }
                                if (requiredCount == 0)
                                {
                                    break;
                                }
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(WareHouseBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                var wareHouse = context.WareHouses.FirstOrDefault(rec => rec.Id == model.Id);

                if (wareHouse == null)
                {
                    throw new Exception("Склад не найден");
                }

                context.WareHouses.Remove(wareHouse);
                context.SaveChanges();
            }
        }

        public WareHouseViewModel GetElement(WareHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new IceCreamDatabase())
            {
                var wareHouse = context.WareHouses
                    .Include(rec => rec.WareHouseIngredients)
                    .ThenInclude(rec => rec.Ingredient)
                    .FirstOrDefault(rec => rec.WareHouseName == model.WareHouseName ||
                    rec.Id == model.Id);

                return wareHouse != null ?
                    new WareHouseViewModel
                    {
                        Id = wareHouse.Id,
                        WareHouseName = wareHouse.WareHouseName,
                        ResponsiblePersonFCS = wareHouse.ResponsiblePersonFCS,
                        DateCreate = wareHouse.DateCreate,
                        WareHouseIngredients = wareHouse.WareHouseIngredients
                            .ToDictionary(recWareHouseIngredient => recWareHouseIngredient.IngredientId,
                            recWareHouseIngredient => (recWareHouseIngredient.Ingredient?.IngredientName,
                            recWareHouseIngredient.Count))
                    } :
                    null;
            }
        }

        public List<WareHouseViewModel> GetFilteredList(WareHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new IceCreamDatabase())
            {
                return context.WareHouses
                    .Include(rec => rec.WareHouseIngredients)
                    .ThenInclude(rec => rec.Ingredient)
                    .Where(rec => rec.WareHouseName.Contains(model.WareHouseName))
                    .ToList()
                    .Select(rec => new WareHouseViewModel
                    {
                        Id = rec.Id,
                        WareHouseName = rec.WareHouseName,
                        ResponsiblePersonFCS = rec.ResponsiblePersonFCS,
                        DateCreate = rec.DateCreate,
                        WareHouseIngredients = rec.WareHouseIngredients
                            .ToDictionary(recWareHouseIngredient => recWareHouseIngredient.IngredientId,
                            recWareHouseIngredient => (recWareHouseIngredient.Ingredient?.IngredientName,
                            recWareHouseIngredient.Count))
                    })
                    .ToList();
            }
        }

        public List<WareHouseViewModel> GetFullList()
        {
            using (var context = new IceCreamDatabase())
            {
                return context.WareHouses.Count() == 0 ? new List<WareHouseViewModel>() :
                    context.WareHouses
                    .Include(rec => rec.WareHouseIngredients)
                    .ThenInclude(rec => rec.Ingredient)
                    .ToList()
                    .Select(rec => new WareHouseViewModel
                    {
                        Id = rec.Id,
                        WareHouseName = rec.WareHouseName,
                        ResponsiblePersonFCS = rec.ResponsiblePersonFCS,
                        DateCreate = rec.DateCreate,
                        WareHouseIngredients = rec.WareHouseIngredients
                            .ToDictionary(recWareHouseIngredients => recWareHouseIngredients.IngredientId,
                            recWareHouseIngredients => (recWareHouseIngredients.Ingredient?.IngredientName,
                            recWareHouseIngredients.Count))
                    })
                    .ToList();
            }
        }

        public void Insert(WareHouseBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new WareHouse(), context);
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

        public void Update(WareHouseBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var wareHouse = context.WareHouses.FirstOrDefault(rec => rec.Id == model.Id);

                        if (wareHouse == null)
                        {
                            throw new Exception("Склад не найден");
                        }

                        CreateModel(model, wareHouse, context);
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
    }
}
