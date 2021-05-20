using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IceCreamShopBusinessLogic.BusinessLogics;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopBusinessLogic.BindingModels;

namespace IceCreamRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly OrderLogic _order;
        private readonly IceCreamLogic _icecream;
        private readonly OrderLogic _main;
        public MainController(OrderLogic order, IceCreamLogic icecream, OrderLogic main)
        {
            _order = order;
            _icecream = icecream;
            _main = main;
        }
        [HttpGet]
        public List<IceCreamViewModel> GetIceCreamList() => _icecream.Read(null)?.ToList();

        [HttpGet]
        public IceCreamViewModel GetIceCream(int icecreamId) => _icecream.Read(new IceCreamBindingModel { Id = icecreamId })?[0];

        [HttpGet]
        public List<OrderViewModel> GetOrders(int clientId) => _order.Read(new OrderBindingModel { ClientId = clientId });

        [HttpPost]
        public void CreateOrder(CreateOrderBindingModel model) => _main.CreateOrder(model);
    }
}
