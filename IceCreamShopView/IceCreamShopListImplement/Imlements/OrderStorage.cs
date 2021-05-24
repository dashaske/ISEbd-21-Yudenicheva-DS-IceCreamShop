using System;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopBusinessLogic.Enums;
using IceCreamShopListImplement.Models;
using System.Collections.Generic;
using System.Linq;

namespace IceCreamShopListImplement.Imlements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly DataListSingleton source;
        public OrderStorage()
        {
            source = DataListSingleton.GetInstance();
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.IceCreamId = model.IceCreamId;
            order.ImplementerId = model.ImplementerId;
            order.ClientId = (int)model.ClientId;
            order.Count = model.Count;
            order.Sum = model.Sum;
            order.Status = model.Status;
            order.DateImplement = model.DateImplement;
            order.DateCreate = model.DateCreate; 

            return order;
        }

        private OrderViewModel CreateModel(Order order)
        {
            string icecreamName = null;
            foreach (var icecream in source.IceCreams)
            {
                if (icecream.Id == order.IceCreamId)
                {
                    icecreamName = icecream.IceCreamName;
                }
            }

            string clientFIO = null;
            foreach (var client in source.Clients)
            {
                if (client.Id == order.ClientId)
                {
                    clientFIO = client.ClientFIO;
                }
            }

            string ImplementerFIO = null;
            foreach (var implementer in source.Implementers)
            {
                if (implementer.Id == order.IceCreamId)
                {
                    ImplementerFIO = implementer.ImplementerFIO;
                }
            }

            return new OrderViewModel
            {
                Id = order.Id,
                ClientId = order.ClientId,
                IceCreamId = order.IceCreamId,
                ImplementerId = order.ImplementerId,
                ImplementerFIO = ImplementerFIO,
                ClientFIO = clientFIO,
                Sum = order.Sum,
                Count = order.Count,
                Status = order.Status,
                IceCreamName = icecreamName,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement
            };
        }

        public List<OrderViewModel> GetFullList()
        {
            List<OrderViewModel> result = new List<OrderViewModel>();
            foreach (var order in source.Orders)
            {
                result.Add(CreateModel(order));
            }
            return result;
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            List<OrderViewModel> result = new List<OrderViewModel>();
            foreach (var order in source.Orders)
            {
                if ((!model.DateFrom.HasValue && !model.DateTo.HasValue &&
                    order.DateCreate.Date == model.DateCreate.Date) ||
                    (model.DateFrom.HasValue && model.DateTo.HasValue &&
                    order.DateCreate.Date >= model.DateFrom.Value.Date && order.DateCreate.Date <=
                    model.DateTo.Value.Date) ||
                    (model.ClientId.HasValue && order.ClientId == model.ClientId) ||
                    (model.FreeOrders.HasValue && model.FreeOrders.Value && order.Status == OrderStatus.Принят) ||
                    (model.ImplementerId.HasValue && order.ImplementerId ==
                    model.ImplementerId && order.Status == OrderStatus.Выполняется))
                {
                    result.Add(CreateModel(order));
                }
            }
            return result;
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var order in source.Orders)
            {
                if (order.Id == model.Id)
                {
                    return CreateModel(order);
                }
            }
            return null;
        }

        public void Insert(OrderBindingModel model)
        {
            Order tempOrder = new Order
            {
                Id = 1
            };
            foreach (var order in source.Orders)
            {
                if (order.Id >= tempOrder.Id)
                {
                    tempOrder.Id = order.Id + 1;
                }
            }
            source.Orders.Add(CreateModel(model, tempOrder));
        }

        public void Update(OrderBindingModel model)
        {
            Order tempOrder = null;

            foreach (var order in source.Orders)
            {
                if (order.Id == model.Id)
                {
                    tempOrder = order;
                }
            }
            if (tempOrder == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempOrder);
        }

        public void Delete(OrderBindingModel model)
        {
            for (int i = 0; i < source.Orders.Count; ++i)
            {
                if (source.Orders[i].Id == model.Id)
                {
                    source.Orders.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
