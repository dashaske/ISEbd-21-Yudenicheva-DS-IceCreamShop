using Microsoft.AspNetCore.Mvc;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.BusinessLogics;
using IceCreamShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuritySystemRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WareHouseController : Controller
    {
        private readonly WareHouseLogic _wareHouseLogic;

        private readonly IngredientLogic _ingredientLogic;

        public WareHouseController(WareHouseLogic wareHouseLogic, IngredientLogic ingredientLogic)
        {
            _wareHouseLogic = wareHouseLogic;
            _ingredientLogic = ingredientLogic;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public List<WareHouseViewModel> GetWareHouses() => _wareHouseLogic.Read(null);

        [HttpPost]
        public void CreateWareHouse(WareHouseBindingModel model) => _wareHouseLogic.CreateOrUpdate(model);

        [HttpPost]
        public void UpdateWareHouse(WareHouseBindingModel model) => _wareHouseLogic.CreateOrUpdate(model);

        [HttpPost]
        public void DeleteWareHouse(WareHouseBindingModel model) => _wareHouseLogic.Delete(model);

        [HttpPost]
        public void Replenishment(WareHouseReplenishmentBindingModel model) => _wareHouseLogic.Replenishment(model);

        [HttpGet]
        public List<IngredientViewModel> GetIngredients() => _ingredientLogic.Read(null);
    }
}
