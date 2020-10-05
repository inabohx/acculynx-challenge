using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Models;
using CodeChallenge.Data;

namespace CodeChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataService _dataService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _dataService = new DataService();
        }

        public async Task<IActionResult> Index()
        {
            List<Question> questionData = await _dataService.GetAnsweredQuestions(15, 2, true);

            return View(questionData);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
