using System;
using System.Collections.Generic;
using IceCreamFileImplement.Models;
using IceCreamShopBusinessLogic.BindingModel;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopBusinessLogic.Enums;
using System.Linq;
using System.Text;

namespace IceCreamFileImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly FileDataListSingleton source;

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.ClientId = model.ClientId.Value;
            order.ImplementerId = model.ImplementerId.Value;
            order.IceCreamId = model.IceCreamId;
            order.Status = model.Status;
            order.Sum = model.Sum;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            order.Count = model.Count;

            return order;
        }

        private OrderViewModel CreateModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                ClientId = order.ClientId,
                IceCreamId = order.IceCreamId,
                ImplementerId = order.ImplementerId,
                Status = order.Status,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Count = order.Count,
                IceCreamName = source.IceCreams.FirstOrDefault(rec => rec.Id == order.IceCreamId)?.IceCreamName,
                ClientFIO = source.Clients.FirstOrDefault(rec => rec.Id == order.ClientId)?.ClientFIO,
                ImplementerFIO = source.Implementers.FirstOrDefault(rec => rec.Id == order.ImplementerId)?.ImplementerFIO
            };
        }

        public OrderStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<OrderViewModel> GetFullList()
        {
            return source.Orders.Select(CreateModel).ToList();
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Orders
                    .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue &&
                    rec.DateCreate.Date == model.DateCreate.Date) ||
                    (model.DateFrom.HasValue && model.DateTo.HasValue &&
                    rec.DateCreate.Date >= model.DateFrom.Value.Date && rec.DateCreate.Date <=
                    model.DateTo.Value.Date) ||
                    (model.ClientId.HasValue && rec.ClientId == model.ClientId) ||
                    (model.FreeOrders.HasValue && model.FreeOrders.Value && rec.Status == OrderStatus.Принят) ||
                    (model.ImplementerId.HasValue && rec.ImplementerId ==
                    model.ImplementerId && rec.Status == OrderStatus.Выполняется))
                    .Select(CreateModel).ToList();
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var order = source.Orders.FirstOrDefault(rec => rec.IceCreamId == model.IceCreamId || rec.Id == model.Id);
            return order != null ? CreateModel(order) : null;
        }

        public void Insert(OrderBindingModel model)
        {
            int maxId = source.Orders.Count > 0 ? source.Orders.Max(recOder => recOder.Id) : 0;
            var order = new Order { Id = maxId + 1 };
            source.Orders.Add(CreateModel(model, order));
        }

        public void Update(OrderBindingModel model)
        {
            var order = source.Orders.FirstOrDefault(recOder => recOder.Id == model.Id);

            if (order == null)
            {
                throw new Exception("Заказ не найден");
            }
            CreateModel(model, order);
        }

        public void Delete(OrderBindingModel model)
        {
            var order = source.Orders.FirstOrDefault(recOrder => recOrder.Id == model.Id);

            if (order != null)
            {
                source.Orders.Remove(order);
            }
            else
            {
                throw new Exception("Заказ не найден");
            }
        }
    }
}
