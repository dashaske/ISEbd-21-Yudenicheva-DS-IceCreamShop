using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.ViewModels;
using WareHouseWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WareHouseWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            if (!Program.IsAuthorized)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<WareHouseViewModel>>($"api/warehouse/getwarehouses"));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Enter()
        {
            return View();
        }

        [HttpPost]
        public void Enter(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                bool check = password == configuration["Password"];

                if (!check)
                {
                    throw new Exception("Попробуйте другой пароль");
                }

                Program.IsAuthorized = check;
                Response.Redirect("Index");
                return;
            }

            throw new Exception("Введите пароль");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public void Create(string name, string FIO)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FIO))
            {
                return;
            }
            APIClient.PostRequest("api/warehouse/createwarehouse", new WareHouseBindingModel
            {
                WareHouseName = name,
                ResponsiblePersonFCS = FIO,
                WareHouseIngredients = new Dictionary<int, (string, int)>()
            });
            Response.Redirect("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("Склад не найден");
            }

            var storeHouse = APIClient.GetRequest<List<WareHouseViewModel>>(
                $"api/warehouse/getwarehouses").FirstOrDefault(rec => rec.Id == id);
            if (storeHouse == null)
            {
                throw new Exception("Склад не найден");
            }

            return View(storeHouse);
        }

        [HttpPost]
        public IActionResult Edit(int id, string name, string FIO)
        {
            var storeHouse = APIClient.GetRequest<List<WareHouseViewModel>>(
                $"api/warehouse/getwarehouses").FirstOrDefault(rec => rec.Id == id);


            APIClient.PostRequest("api/warehouse/updatewarehouse", new WareHouseBindingModel
            {
                Id = id,
                WareHouseName = name,
                ResponsiblePersonFCS = FIO,
                WareHouseIngredients = storeHouse.WareHouseIngredients
            });
            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Склад не найден");
            }

            var wareHouse = APIClient.GetRequest<List<WareHouseViewModel>>(
                 $"api/warehouse/getwarehouses").FirstOrDefault(rec => rec.Id == id);
            if (wareHouse == null)
            {
                throw new Exception("Склад не найден");
            }

            return View(wareHouse);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            APIClient.PostRequest("api/warehouse/deletewarehouse", new WareHouseBindingModel { Id = id });
            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public IActionResult Replenishment()
        {
            if (!Program.IsAuthorized)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.WareHouses = APIClient.GetRequest<List<WareHouseViewModel>>("api/warehouse/getwarehouses");
            ViewBag.Ingredients = APIClient.GetRequest<List<IngredientViewModel>>($"api/warehouse/getingredients");

            return View();
        }

        [HttpPost]
        public IActionResult Replenishment(int wareHouseId, int ingredientId, int count)
        {
            if (wareHouseId == 0 || ingredientId == 0 || count <= 0)
            {
                throw new Exception("Проверьте данные");
            }

            var storeHouse = APIClient.GetRequest<List<WareHouseViewModel>>(
                 $"api/warehouse/getwarehouses").FirstOrDefault(rec => rec.Id == wareHouseId);

            if (storeHouse == null)
            {
                throw new Exception("Склад не найден");
            }

            var component = APIClient.GetRequest<List<WareHouseViewModel>>(
                "api/warehouse/getingredients").FirstOrDefault(rec => rec.Id == ingredientId);

            if (component == null)
            {
                throw new Exception("Компонент не найден");
            }

            APIClient.PostRequest("api/warehouse/replenishment", new WareHouseReplenishmentBindingModel
            {
                WareHouseId = wareHouseId,
                IngredientId = ingredientId,
                Count = count
            });
            return Redirect("~/Home/Index");
        }
    }
}
