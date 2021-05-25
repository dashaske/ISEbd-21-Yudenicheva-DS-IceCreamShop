using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IceCreamShopBusinessLogic.ViewModel;
using IceCreamShopBusinessLogic.BindingModels;
using IceCreamShopBusinessLogic.BindingModel;
using IceCreamClientApp.Models;
using IceCreamShopBusinessLogic.ViewModels;

namespace IceCreamClientApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (Program.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<OrderViewModel>>($"api/main/getorders?clientId={Program.Client.Id}"));
        }

        [HttpPost]
        public void Privacy(string login, string password, string fio)
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(fio))
            {
                APIClient.PostRequest("api/client/updatedata", new ClientBindingModel
                {
                    Id = Program.Client.Id,
                    ClientFIO = fio,
                    Email = login,
                    Password = password
                });
                Program.Client.ClientFIO = fio;
                Program.Client.Email = login;
                Program.Client.Password = password;
                Response.Redirect("Index");
                return;
            }
            throw new Exception("Введите логин, пароль и ФИО");
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            if (Program.Client == null)
            {
                return Redirect("~/Home/Enter");
            }

            return View(Program.Client);
        }

        [HttpGet]
        public IActionResult Mail(int pageNumber)
        {
            Console.WriteLine(Program.PageNumber);
            if (Program.Client == null)
            {
                return Redirect("~/Home/Enter");
            }

            List<MessageInfoViewModel> model = new List<MessageInfoViewModel>();
            if (pageNumber > 0)
            {
                model = APIClient.GetRequest<List<MessageInfoViewModel>>($"api/client/getmessages?clientId={Program.Client.Id}&pageNumber={pageNumber}");
            }

            if (model.Count == 0)
            {
                model = APIClient.GetRequest<List<MessageInfoViewModel>>($"api/client/getmessages?clientId={Program.Client.Id}&pageNumber={Program.PageNumber}");
            }
            else
            {
                Program.PageNumber = pageNumber;
            }
            ViewBag.Id = Program.PageNumber;
            return View(model);
        }

        [HttpGet]
        public IActionResult NextMailPage()
        {
            return Redirect($"~/Home/Mail?pageNumber={Program.PageNumber + 1}");
        }

        [HttpGet]
        public IActionResult PrevMailPage()
        {
            if (Program.PageNumber > 1)
            {
                return Redirect($"~/Home/Mail?pageNumber={Program.PageNumber - 1}");
            }

            return Redirect($"~/Home/Mail?pageNumber={Program.PageNumber}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        [HttpGet]
        public IActionResult Enter()
        {
            return View();
        }

        [HttpPost]
        public void Enter(string login, string password)
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                Program.Client = APIClient.GetRequest<ClientViewModel>($"api/client/login?login={login}&password={password}");

                if (Program.Client == null)
                {
                    throw new Exception("Неверный логин/пароль");
                }

                Response.Redirect("Index");
                return;
            }

            throw new Exception("Введите логин, пароль");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public void Register(string login, string password, string fio)
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(fio))
            {
                APIClient.PostRequest("api/client/register", new ClientBindingModel
                {
                    ClientFIO = fio,
                    Email = login,
                    Password = password
                });
                Response.Redirect("Enter");
                return;
            }
            throw new Exception("Введите логин, пароль и ФИО");
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Secures = APIClient.GetRequest<List<IceCreamViewModel>>("api/main/geticecreamlist");
            return View();
        }

        [HttpPost]
        public void Create(int secure, int count, decimal sum)
        {
            if (count == 0 || sum == 0)
            {
                return;
            }
            APIClient.PostRequest("api/main/createorder", new CreateOrderBindingModel
            {
                IceCreamId = secure,
                ClientId = (int)Program.Client.Id,
                Sum = sum,
                Count = count
            });
            Response.Redirect("Index");
        }

        [HttpPost]
        public decimal Calc(decimal count, int icecream)
        {
            IceCreamViewModel ice = APIClient.GetRequest<IceCreamViewModel>($"api/main/geticecream?icecreamId={icecream}");
            return count * ice.Price;
        }
    }
}