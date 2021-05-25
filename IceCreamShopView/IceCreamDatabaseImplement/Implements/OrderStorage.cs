using System;
using System.Collections.Generic;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.BindingModel;
using IceCreamShopBusinessLogic.Enums;
using System.Linq;
using IceCreamDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace IceCreamDatabaseImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        public List<OrderViewModel> GetFullList()
        {
            using (var context = new IceCreamDatabase())
            {
                return context.Orders.Include(rec => rec.IceCream).Include(rec => rec.Client)
                    .Include(rec => rec.Implementer).Select(rec => new OrderViewModel
                    {
                        Id = rec.Id,
                        IceCreamName = rec.IceCream.IceCreamName,
                        IceCreamId = rec.IceCreamId,
                        Count = rec.Count,
                        Sum = rec.Sum,
                        Status = rec.Status,
                        DateCreate = rec.DateCreate,
                        DateImplement = rec.DateImplement,
                        ClientId = rec.ClientId,
                        ClientFIO = rec.Client.ClientFIO,
                        ImplementerId = rec.ImplementerId,
                        ImplementerFIO = rec.ImplementerId.HasValue ? rec.Implementer.ImplementerFIO : string.Empty
                    })
                .ToList();
            }
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new IceCreamDatabase())
            {
                return context.Orders
                    .Include(rec => rec.IceCream)
                    .Include(rec => rec.Client)
                    .Include(rec => rec.Implementer)
                    .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue &&
                    rec.DateCreate.Date == model.DateCreate.Date) ||
                    (model.DateFrom.HasValue && model.DateTo.HasValue &&
                    rec.DateCreate.Date >= model.DateFrom.Value.Date && rec.DateCreate.Date <=
                    model.DateTo.Value.Date) ||
                    (model.ClientId.HasValue && rec.ClientId == model.ClientId) ||
                    (model.FreeOrders.HasValue && model.FreeOrders.Value && rec.Status == OrderStatus.Принят) ||
                    (model.ImplementerId.HasValue && rec.ImplementerId ==
                    model.ImplementerId && rec.Status == OrderStatus.Выполняется))
                    .Select(rec => new OrderViewModel
                    {
                        Id = rec.Id,
                        Count = rec.Count,
                        DateCreate = rec.DateCreate,
                        DateImplement = rec.DateImplement,
                        IceCreamId = rec.IceCreamId,
                        IceCreamName = rec.IceCream.IceCreamName,
                        ClientId = rec.ClientId,
                        ClientFIO = rec.Client.ClientFIO,
                        ImplementerId = rec.ImplementerId,
                        ImplementerFIO = rec.ImplementerId.HasValue ? rec.Implementer.ImplementerFIO : string.Empty,
                        Status = rec.Status,
                        Sum = rec.Sum
                    })
                    .ToList();
            }
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new IceCreamDatabase())
            {
                var order = context.Orders.Include(rec => rec.IceCream).Include(rec => rec.Client).Include(rec => rec.Implementer)
                .FirstOrDefault(rec => rec.Id == model.Id);
                return order != null ?
                new OrderViewModel
                {
                    Id = order.Id,
                    IceCreamName = order.IceCream.IceCreamName,
                    IceCreamId = order.IceCreamId,
                    Count = order.Count,
                    Sum = order.Sum,
                    Status = order.Status,
                    DateCreate = order.DateCreate,
                    DateImplement = order.DateImplement,
                    ClientId = order.ClientId,
                    ClientFIO = order.Client.ClientFIO,
                    ImplementerId = order.ImplementerId,
                    ImplementerFIO = order.ImplementerId.HasValue ? order.Implementer.ImplementerFIO : string.Empty
                } :
                null;
            }
        }

        public void Insert(OrderBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                context.Orders.Add(CreateModel(model, new Order()));
                context.SaveChanges(); 
            }
        }

        public void Update(OrderBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                var element = context.Orders.FirstOrDefault(rec => rec.Id ==
                model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }

        public void Delete(OrderBindingModel model)
        {
            using (var context = new IceCreamDatabase())
            {
                Order element = context.Orders.FirstOrDefault(rec => rec.Id ==
                model.Id);
                if (element != null)
                {
                    context.Orders.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.IceCreamId = model.IceCreamId;
            order.Count = model.Count;
            order.Sum = model.Sum;
            order.Status = model.Status;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            order.ClientId = (int)model.ClientId;
            order.ImplementerId = model.ImplementerId;
            return order;
        }
    }
}
