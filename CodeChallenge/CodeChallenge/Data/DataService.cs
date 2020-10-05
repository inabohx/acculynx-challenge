using CodeChallenge.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    /// <summary>
    /// Class containing methods to look up info for the data used in any views.
    /// This should encapsulate everything about interacting with the Stack Exchange web APIs and be agnostic about platform.
    /// </summary>
    public class DataService
    {
        #region Public methods for Model data

        /// <summary>
        /// Data lookup method for getting a list of answered questions from Stack Overflow. These questions
        /// are hard-coded to be the most recent ones based on creation date (but still meeting the additional criteria).
        /// 
        /// Note: If this is called enough in a narrow time window, it may fail to look up data. See https://api.stackexchange.com/docs/throttle for more info.
        /// </summary>
        /// <param name="numQuestions">The number of questions to return (must be between 1 and 100)</param>
        /// <param name="minAnswers">The minimum number of answers a question needs to have (must be 1 or greater)</param>
        /// <param name="isAccepted">True to return only questions with accepted answers, False to return only those without one.</param>
        /// <returns>A List of Question objects sorted by creation date - descending</returns>
        public async Task<List<Question>> GetAnsweredQuestions(int numQuestions, int minAnswers, bool isAccepted)
        {
            // Parameter boundry checks
            minAnswers = Math.Max(1, minAnswers);        // Enforce a miniumum of 1 answer
            numQuestions = Math.Min(100, numQuestions);  // Enforce a max of 100 questions
            numQuestions = Math.Max(1, numQuestions);    // Enforce a minimum of 1 question

            // Set up the API URL
            string apiUrl = "http://api.stackexchange.com/2.2/search/advanced?"
                + "page=1"                       // Hard-code to just one page
                + $"&pagesize={numQuestions}"    // Set page size to number of questions (1 to 100)
                + "&order=desc&sort=creation"    // Request the most recently created questions
                + $"&accepted={isAccepted}"      // True to return only questions with accepted answers, False to return only those without one
                + $"&answers={minAnswers}"       // Set the minimum number of answers returned questions must have
                + "&site=stackoverflow"          // Set the site to Stack Overflow
                + "&filter=!4(L6lo9D9N4OcoUIa";  // Filter generated from stackexchange to reduce the amount of question data in the response

            // Fire off the request
            string response = await MakeRequest(apiUrl);

            // Parse the response into the model
            List<Question> result = new List<Question>();
            var questionData = JObject.Parse(response);
            foreach (var question in questionData["items"])
            {
                result.Add(new Question(question));
            }

            return result;
        }

        /// <summary>
        /// Data lookup method for getting a question body and a list of its answers from Stack Overflow.
        /// 
        /// Note: If this is called enough in a narrow time window, it may fail to look up data. See https://api.stackexchange.com/docs/throttle for more info.
        /// </summary>
        /// <param name="id">The unique identifier of the question to lookup data for</param>
        /// <returns>A QuestionAndAnswers model class for the view</returns>
        public async Task<QuestionAndAnswers> GetQuestionAndAnswers(int id)
        {
            // Set up the API URL
            string apiUrl = "http://api.stackexchange.com/2.2/questions/"   //  64215481 ?order=desc&sort=creation &site=stackoverflow&filter=!0W--8I9YKGezChPc18LqqIhtD
                + $"{id.ToString()}/"                 // This supports multiple questions but we only care about our one
                + "?order=desc&sort=creation"         // Sorting info (kind of unnecessary since there's only one question, but good to have)
                + "&site=stackoverflow"               // Set the site to Stack Overflow
                + "&filter=!L_(IB3u73lMJDwuiLsf3k1";  // Filter generated from stackexchange to reduce the amount of data in the response plus request the full bodies of the question and answers

            // Fire off the request
            string response = await MakeRequest(apiUrl);

            // Parse the response into the model
            QuestionAndAnswers result = new QuestionAndAnswers();
            var allData = JObject.Parse(response);
            var questionData = allData["items"][0];
            
            result.Question = new Question(questionData);
            foreach(var answer in questionData["answers"])
            {
                result.Answers.Add(new Answer(answer));
            }

            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Wrapper around making an HTTP request for a given URL.
        /// 
        /// Note: Is hard-coded decompress the request using GZip or Deflate.
        /// </summary>
        /// <param name="apiUrl">An API URL to request data from</param>
        /// <returns>A JSON string of the requested data</returns>
        private async Task<string> MakeRequest(string apiUrl)
        {
            // Set up an HttpClient for request/response from an API URL
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;  // Stack Exchange compresses their API responses (see https://api.stackexchange.com/docs/compression for more info)
            using var httpClient = new HttpClient(handler);

            // Make the request
            httpClient.BaseAddress = new Uri(apiUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string response = await httpClient.GetStringAsync(apiUrl);

            return response;
        }

        #endregion
    }
}
