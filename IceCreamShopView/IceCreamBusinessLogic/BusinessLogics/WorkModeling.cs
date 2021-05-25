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
            // ищем заказы, которые уже в работе (вдруг исполнителя прервали)
            var runOrders = await Task.Run(() => _orderStorage.GetFilteredList(new
            OrderBindingModel
            { ImplementerId = implementer.Id }));

            var needComponentOrders = await Task.Run(() => _orderStorage.GetFilteredList(new
            OrderBindingModel
            {
                NeedIngredientOrders = true,
            }));

            foreach (var order in runOrders)
            {
                // делаем работу заново
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                _orderLogic.FinishOrder(new ChangeStatusBindingModel
                {
                    OrderId = order.Id,
                    ImplementerId = implementer.Id
                });
                // отдыхаем
                Thread.Sleep(implementer.PauseTime);
            }

            foreach (var order in needComponentOrders)
            {
                RunOrder(order, implementer);
            }
            await Task.Run(() =>
            {
                foreach (var order in orders)
                {
                    RunOrder(order, implementer);
                }
            });
        }
        private void RunOrder(OrderViewModel order, ImplementerViewModel implementer)
        {
            // попытка назначить заказ на исполнителя
            try
            {
                _orderLogic.TakeOrderInWork(new ChangeStatusBindingModel
                {
                    OrderId = order.Id,
                    ImplementerId = implementer.Id
                });
                // работа
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) *
                order.Count);
                _orderLogic.FinishOrder(new ChangeStatusBindingModel
                {
                    OrderId = order.Id,
                    ImplementerId = implementer.Id
                });
                // отдых
                Thread.Sleep(implementer.PauseTime);
            }
            catch (Exception) { }
        }
    }
}
