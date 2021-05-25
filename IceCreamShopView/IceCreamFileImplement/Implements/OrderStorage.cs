using System;
using System.Collections.Generic;
using IceCreamFileImplement.Models;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using System.Linq;
using System.Text;

namespace IceCreamFileImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly FileDataListSingleton source;

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

        private OrderViewModel CreateModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                IceCreamName = source.IceCreams.FirstOrDefault(iceCream => iceCream.Id == order.IceCreamId)?.IceCreamName,
                IceCreamId = order.IceCreamId,
                Count = order.Count,
                Sum = order.Sum,
                Status = order.Status,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement
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
            if (model.DateTo != null && model.DateFrom != null)
            {
                return source.Orders.Where(rec => rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo)
                    .Select(CreateModel).ToList();
            }
            return source.Orders
                .Where(rec => rec.IceCreamId.ToString().Contains(model.IceCreamId.ToString()))
                .Select(CreateModel).ToList();
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var order = source.Orders.FirstOrDefault(recOder => recOder.Id == model.Id);

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
