using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CodeChallenge.Controllers
{
    /// <summary>
    /// Default controller for this web application.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataService _dataService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _dataService = new DataService();
        }

        /// <summary>
        /// Default view to show a list of questions
        /// </summary>
        public async Task<IActionResult> Index()
        {
            List<Question> questionData = await _dataService.GetAnsweredQuestions(15, 2, true);

            return View(questionData);
        }

        /// <summary>
        /// Question-level view to show its answers
        /// </summary>
        public async Task<IActionResult> Question()
        {
            // TODO: Lookup data
            return View();
        }

        /// <summary>
        /// Displays the About view
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Generated code -- error view
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
