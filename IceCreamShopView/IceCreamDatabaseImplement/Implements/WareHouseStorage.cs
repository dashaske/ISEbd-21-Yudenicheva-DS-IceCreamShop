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
        public List<WareHouseViewModel> GetFullList()
        {
            using (var context = new IceCreamDatabase())
            {
                return context.WareHouses
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
                .ToDictionary(recPC => recPC.IngredientId, recPC =>
                (recPC.Ingredient?.IngredientName, recPC.Count))
                })
                .ToList();
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
                .ToDictionary(recPC => recPC.IngredientId, recPC =>
                (recPC.Ingredient?.IngredientName, recPC.Count))
                })
                .ToList();
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
                var warehouse = context.WareHouses
                .Include(rec => rec.WareHouseIngredients)
                .ThenInclude(rec => rec.Ingredient)
                .FirstOrDefault(rec => rec.WareHouseName.Equals(model.WareHouseName) || rec.Id
                == model.Id);
                return warehouse != null ?
                new WareHouseViewModel
                {
                    Id = warehouse.Id,
                    WareHouseName = warehouse.WareHouseName,
                    ResponsiblePersonFCS = warehouse.ResponsiblePersonFCS,
                    DateCreate = warehouse.DateCreate,
                    WareHouseIngredients = warehouse.WareHouseIngredients
                .ToDictionary(recPC => recPC.IngredientId, recPC =>
                (recPC.Ingredient?.IngredientName, recPC.Count))
                } : null;
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
                        WareHouse warehouse = CreateModel(model, new WareHouse());
                        context.WareHouses.Add(warehouse);
                        context.SaveChanges();
                        CreateModel(model, warehouse, context);

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
                        var element = context.WareHouses.FirstOrDefault(rec => rec.Id == model.Id);
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

        public void Delete(WareHouseBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                WareHouse element = context.WareHouses.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.WareHouses.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private WareHouse CreateModel(WareHouseBindingModel model, WareHouse wareHouse)
        {
            wareHouse.WareHouseName = model.WareHouseName;
            wareHouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;
            wareHouse.DateCreate = model.DateCreate;
            return wareHouse;
        }

        private WareHouse CreateModel(WareHouseBindingModel model, WareHouse wareHouse,
       IceCreamDatabase context)
        {
            wareHouse.WareHouseName = model.WareHouseName;
            wareHouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;
            wareHouse.DateCreate = model.DateCreate;
            if (model.Id.HasValue)
            {
                var productIngridient = context.WareHouseIngredients.Where(rec =>
                rec.WareHouseId == model.Id.Value).ToList();
                // удалили те, которых нет в модели
                context.WareHouseIngredients.RemoveRange(productIngridient.Where(rec =>
                !model.WareHouseIngredients.ContainsKey(rec.IngredientId)).ToList());
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateIngridient in productIngridient)
                {
                    updateIngridient.Count =
                    model.WareHouseIngredients[updateIngridient.IngredientId].Item2;
                    model.WareHouseIngredients.Remove(updateIngridient.IngredientId);
                }
                context.SaveChanges();
            }
            // добавили новые
            foreach (var pc in model.WareHouseIngredients)
            {
                context.WareHouseIngredients.Add(new WareHouseIngredient
                {
                    WareHouseId = wareHouse.Id,
                    IngredientId = pc.Key,
                    Count = pc.Value.Item2,
                });
                try
                {
                    context.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
            return wareHouse;
        }

        public bool CheckAndTake(int IceCreamId, int Count)
        {
            using (var context = new IceCreamDatabase())
            {
                var list = GetFullList();
                var DCount = context.IceCreamIngredients.Where(rec => rec.IceCreamId == IceCreamId)
                    .ToDictionary(rec => rec.IngredientId, rec => rec.Count * Count);

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var key in DCount.Keys.ToArray())
                        {
                            foreach (var wareHouseIngridient in context.WareHouseIngredients.Where(rec => rec.IngredientId == key))
                            {
                                if (wareHouseIngridient.Count > DCount[key])
                                {
                                    wareHouseIngridient.Count -= DCount[key];
                                    DCount[key] = 0;
                                    break;
                                }
                                else
                                {
                                    DCount[key] -= wareHouseIngridient.Count;
                                    wareHouseIngridient.Count = 0;
                                }
                            }
                            if (DCount[key] > 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        context.SaveChanges();
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
    }
}
