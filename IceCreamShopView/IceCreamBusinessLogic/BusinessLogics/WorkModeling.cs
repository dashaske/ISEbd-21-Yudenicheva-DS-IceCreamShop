using System;
using System.Collections.Generic;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.BindingModel;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopBusinessLogic.Enums;
using IceCreamShopBusinessLogic.ViewModel;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using IceCreamShopBusinessLogic.Enums;

namespace IceCreamShopBusinessLogic.BusinessLogics
{
    public class WorkModeling
    {
        private readonly IImplementerStorage _implementerStorage;

        private readonly IOrderStorage _orderStorage;

        private readonly OrderLogic _orderLogic;

        private readonly Random rnd;

        public WorkModeling(IImplementerStorage implementerStorage, IOrderStorage
        orderStorage, OrderLogic orderLogic)
        {
            this._implementerStorage = implementerStorage;
            this._orderStorage = orderStorage;
            this._orderLogic = orderLogic;
            rnd = new Random(1000);
        }
        /// <summary>
        /// Запуск работ
        /// </summary>
        public void DoWork()
        {
            var implementers = _implementerStorage.GetFullList();
            var orders = _orderStorage.GetFilteredList(new OrderBindingModel
            {
                FreeOrders = true
            });
            foreach (var implementer in implementers)
            {
                WorkerWorkAsync(implementer, orders);
            }
        }
        /// <summary>
        /// Иммитация работы исполнителя
        /// </summary>
        /// <param name="implementer"></param>
        /// <param name="orders"></param>
        private async void WorkerWorkAsync(ImplementerViewModel implementer,
            List<OrderViewModel> orders)
        {
            var runOrders = await Task.Run(() => _orderStorage.GetFilteredList(new OrderBindingModel
            {
                ImplementerId = implementer.Id
            }));

            foreach (var order in runOrders)
            {
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);

                _orderLogic.FinishOrder(new ChangeStatusBindingModel
                {
                    OrderId = order.Id
                });

                Thread.Sleep(implementer.PauseTime);
            }

            var ordersRequiringMaterials = await Task.Run(() => _orderStorage.GetFullList()
            .Where(rec => rec.Status == OrderStatus.ТребуютсяMатериалы).ToList());
            foreach (var order in ordersRequiringMaterials)
            {
                try
                {
                    _orderLogic.TakeOrderInWork(new ChangeStatusBindingModel
                    {
                        OrderId = order.Id,
                        ImplementerId = implementer.Id
                    });
                    if (_orderStorage.GetElement(new OrderBindingModel { Id = order.Id }).Status == OrderStatus.ТребуютсяMатериалы)
                    {
                        continue;
                    }
                    Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                    _orderLogic.FinishOrder(new ChangeStatusBindingModel
                    {
                        OrderId = order.Id
                    });
                    Thread.Sleep(implementer.PauseTime);
                }
                catch (Exception) { }
            }

            await Task.Run(() =>
            {
                foreach (var order in orders)
                {
                    try
                    {
                        _orderLogic.TakeOrderInWork(new ChangeStatusBindingModel
                        {
                            OrderId = order.Id,
                            ImplementerId = implementer.Id
                        });
                        if (_orderStorage.GetElement(new OrderBindingModel { Id = order.Id }).Status == OrderStatus.ТребуютсяMатериалы)
                        {
                            continue;
                        }
                        Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                        _orderLogic.FinishOrder(new ChangeStatusBindingModel
                        {
                            OrderId = order.Id
                        });
                        Thread.Sleep(implementer.PauseTime);
                    }
                    catch (Exception) { }
                }
            });
        }
    }
}
