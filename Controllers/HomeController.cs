using BayshoreCodingChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BayshoreCodingChallenge.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new LifeGameModel();
            model.GenerateNewCellBoard();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(LifeGameModel model)
        {
            model.EvolveGenerations();
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}