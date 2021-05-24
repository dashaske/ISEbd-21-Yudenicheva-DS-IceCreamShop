using System;
using IceCreamShopBusinessLogic.BindingModel;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopBusinessLogic.HelperModels;
using IceCreamShopBusinessLogic.Enums;
using System.Collections.Generic;

namespace IceCreamShopBusinessLogic.BusinessLogics
{
    public class OrderLogic
    {
        private readonly IOrderStorage _orderStorage;

        private readonly IClientStorage _clientStorage;

        private readonly object locker = new object();

        private readonly IWareHouseStorage _warehouseStorage;
        public OrderLogic(IOrderStorage orderStorage, IClientStorage clientStorage, IWareHouseStorage warehouseStorage)
        {
            _orderStorage = orderStorage;
            _clientStorage = clientStorage;
            _warehouseStorage = warehouseStorage;
        }
        public List<OrderViewModel> Read(OrderBindingModel model)
        {
            if (model == null)
            {
                return _orderStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<OrderViewModel> { _orderStorage.GetElement(model) };
            }
            return _orderStorage.GetFilteredList(model);
        }
        public void CreateOrder(CreateOrderBindingModel model)
        {
            _orderStorage.Insert(new OrderBindingModel
            {
                IceCreamId = model.IceCreamId,
                Count = model.Count,
                Sum = model.Sum,
                DateCreate = DateTime.Now,
                Status = OrderStatus.Принят,
                ClientId = model.ClientId
            });

            MailLogic.MailSendAsync(new MailSendInfo
            {
                MailAddress = _clientStorage.GetElement(new ClientBindingModel
                {
                    Id = model.ClientId
                })?.Email,
                Subject = $"Новый заказ",
                Text = $"Заказ от {DateTime.Now} на сумму {model.Sum:N2} принят."
            });
        }
        public void TakeOrderInWork(ChangeStatusBindingModel model)
        {
            lock (locker)
            {
                var order = _orderStorage.GetElement(new OrderBindingModel
                {
                    Id = model.OrderId
                });
                if (order == null)
                {
                    throw new Exception("Не найден заказ");
                }
                if (order.Status != OrderStatus.Принят)
                {
                    throw new Exception("Заказ не в статусе \"Принят\"");
                }
                if (order.ImplementerId.HasValue)
                {
                    throw new Exception("У заказа уже есть исполнитель");
                }
                OrderBindingModel orderModel = new OrderBindingModel
                {
                    Id = order.Id,
                    IceCreamId = order.IceCreamId,
                    ImplementerId = model.ImplementerId,
                    Count = order.Count,
                    Sum = order.Sum,
                    DateCreate = order.DateCreate,
                    DateImplement = DateTime.Now,
                    Status = OrderStatus.Выполняется,
                    ClientId = order.ClientId
                };
                if (!_warehouseStorage.CheckAndTake(order.IceCreamId, order.Count))
                {
                    orderModel.Status = OrderStatus.ТребуютсяMатериалы;
                    orderModel.ImplementerId = null;
                }
                _orderStorage.Update(orderModel);

                MailLogic.MailSendAsync(new MailSendInfo
                {
                    MailAddress = _clientStorage.GetElement(new ClientBindingModel
                    {
                        Id = order.ClientId
                    })?.Email,
                    Subject = $"Заказ №{order.Id}",
                    Text = $"Заказ №{order.Id} передан в работу."
                });
            }
        }

        public void FinishOrder(ChangeStatusBindingModel model)
        {
            var order = _orderStorage.GetElement(new OrderBindingModel { Id = model.OrderId });
            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (order.Status != OrderStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            _orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                IceCreamId = order.IceCreamId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Готов,
                ClientId = order.ClientId,
                ImplementerId = model.ImplementerId
            });

            MailLogic.MailSendAsync(new MailSendInfo
            {
                MailAddress = _clientStorage.GetElement(new ClientBindingModel
                {
                    Id = order.ClientId
                })?.Email,
                Subject = $"Заказ №{order.Id}",
                Text = $"Заказ №{order.Id} выполнен."
            });
        }
        public void PayOrder(ChangeStatusBindingModel model)
        {
            var order = _orderStorage.GetElement(new OrderBindingModel
            {
                Id = model.OrderId
            });
            if (order == null)
            {
                throw new Exception("Не найден заказ!");
            }
            if (order.Status != OrderStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }
            _orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                IceCreamId = order.IceCreamId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Оплачен,
                ClientId = order.ClientId
            });

            MailLogic.MailSendAsync(new MailSendInfo
            {
                MailAddress = _clientStorage.GetElement(new ClientBindingModel
                {
                    Id = order.ClientId
                })?.Email,
                Subject = $"Заказ №{order.Id}",
                Text = $"Заказ №{order.Id} оплачен."
            });
        }
    }
}
