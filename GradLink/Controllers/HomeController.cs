
using GradLink.Model.ViewModel;
using GradLink.Repository.MSSQL.ORM.Context;
using GradLink.Repository.MSSQL.ORM.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GradLink.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

    }
}
