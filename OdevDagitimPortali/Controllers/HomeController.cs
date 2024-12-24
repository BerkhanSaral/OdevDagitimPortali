using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OdevDagitimPortali.Repository;
using OdevDagitimPortali.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace OdevDagitimPortali.Controllers
{
    public class HomeController : Controller
    {
   
        public HomeController()
        {
          
        }

        public IActionResult Index()
        {
            ViewData["Layout"] = "_LayoutUsers";
            return View();
        }

        public async Task<IActionResult> Login(LoginModel model)
        {
            return View(model);
        }


        public async Task<IActionResult> Register(RegisterModel model)
        {
            return View(model);

        }






    }
}
