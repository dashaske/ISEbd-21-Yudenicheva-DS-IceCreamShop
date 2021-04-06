using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IceCreamShopBusinessLogic.BusinessLogics;
using IceCreamShopBusinessLogic.ViewModels;
using IceCreamShopBusinessLogic.BindingModel;

namespace IceCreamRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly OrderLogic _order;
        private readonly IceCreamLogic _package;
        private readonly OrderLogic _main;
        public MainController(OrderLogic order, IceCreamLogic package, OrderLogic main)
        {
            _order = order;
            _package = package;
            _main = main;
        }
        [HttpGet]
        public List<IceCreamViewModel> GetPackageList() => _package.Read(null)?.ToList();

        [HttpGet]
        public IceCreamViewModel GetPackage(int packageId) => _package.Read(new IceCreamBindingModel { Id = packageId })?[0];

        [HttpGet]
        public List<OrderViewModel> GetOrders(int clientId) => _order.Read(new OrderBindingModel { ClientId = clientId });

        [HttpPost]
        public void CreateOrder(CreateOrderBindingModel model) => _main.CreateOrder(model);
    }
}
