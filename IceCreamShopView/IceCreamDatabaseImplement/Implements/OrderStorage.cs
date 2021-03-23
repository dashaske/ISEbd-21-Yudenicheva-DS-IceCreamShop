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
    public class OrderStorage : IOrderStorage
    {
        public List<OrderViewModel> GetFullList()
        {
            using (var context = new IceCreamDatabase())
            {
                return context.Orders.Select(rec => new OrderViewModel
                {
                    Id = rec.Id,
                    IceCreamName = context.IceCreams.FirstOrDefault(r => r.Id == rec.IceCreamId).IceCreamName,
                    IceCreamId = rec.IceCreamId,
                    Count = rec.Count,
                    Sum = rec.Sum,
                    Status = rec.Status,
                    DateCreate = rec.DateCreate,
                    DateImplement = rec.DateImplement
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
            if (model.DateFrom != null && model.DateTo != null)
            {
                using (var context = new IceCreamDatabase())
                {
                    return context.Orders.Where(rec => rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo)
                        .Select(rec => new OrderViewModel
                        {
                            Id = rec.Id,
                            IceCreamName = context.IceCreams.FirstOrDefault(r => r.Id == rec.IceCreamId).IceCreamName,
                            IceCreamId = rec.IceCreamId,
                            Count = rec.Count,
                            Sum = rec.Sum,
                            Status = rec.Status,
                            DateCreate = rec.DateCreate,
                            DateImplement = rec.DateImplement
                        }).ToList();
                }
            }
            using (var context = new IceCreamDatabase())
            {
                return context.Orders
                .Where(rec => rec.Id.Equals(model.Id))
                .Select(rec => new OrderViewModel
                {
                    Id = rec.Id,
                    IceCreamName = context.IceCreams.FirstOrDefault(r => r.Id == rec.IceCreamId).IceCreamName,
                    IceCreamId = rec.IceCreamId,
                    Count = rec.Count,
                    Sum = rec.Sum,
                    Status = rec.Status,
                    DateCreate = rec.DateCreate,
                    DateImplement = rec.DateImplement
                }).ToList();

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
                var order = context.Orders
                .FirstOrDefault(rec => rec.Id == model.Id);
                return order != null ?
                new OrderViewModel
                {
                    Id = order.Id,
                    IceCreamName = context.IceCreams.FirstOrDefault(r => r.Id == order.IceCreamId).IceCreamName,
                    IceCreamId = order.IceCreamId,
                    Count = order.Count,
                    Sum = order.Sum,
                    Status = order.Status,
                    DateCreate = order.DateCreate,
                    DateImplement = order.DateImplement
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
            return order;
        }
    }
}
