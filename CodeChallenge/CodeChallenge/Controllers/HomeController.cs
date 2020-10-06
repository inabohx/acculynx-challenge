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

            return View(new QuestionList(questionData));
        }

        /// <summary>
        /// Question-level view to show its body and all answers
        /// </summary>
        public async Task<IActionResult> Question(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            QuestionAndAnswers data = await _dataService.GetQuestionAndAnswers(id.Value);
            return View(data);
        }

        /// <summary>
        /// Checks if an answer is the Accepted answer and populated an alert to show, then redirects back to the question list view
        /// </summary>
        /// <param name="id">The unique id of the answer to check</param>
        public async Task<IActionResult> AnswerGuess(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Prep the question list data again since we'll reset to there
            List<Question> questionData = await _dataService.GetAnsweredQuestions(15, 2, true);
            QuestionList data = new QuestionList(questionData);

            // Check if the answer is Accepted or not and populate a little alert message
            // Would probably be nicer to stay on the question page and highlight the right/wrong answer cards, but skipping over that for the challenge
            bool isAccepted = await _dataService.CheckAcceptedAnswer(id.Value);
            data.AlertMessage = isAccepted ? "Correct!" : "Wrong!";
            
            return View("Index", data);
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
