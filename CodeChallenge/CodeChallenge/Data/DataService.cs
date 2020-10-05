﻿using CodeChallenge.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // Parameter checks
            minAnswers = Math.Max(1, minAnswers);        // Enforce a miniumum of 1 answer
            numQuestions = Math.Min(100, numQuestions);  // Enforce a max of 100 questions
            numQuestions = Math.Max(1, numQuestions);    // Enforce a minimum of 1 question

            // Set up the API URL
            string apiUrl = "http://api.stackexchange.com/2.2/search/advanced?"
                + "page=1"                     // Hard-code to just one page
                + $"&pagesize={numQuestions}"  // Set page size to number of questions (1 to 100)
                + "&order=desc&sort=creation"  // Request the most recently created questions
                + $"&accepted={isAccepted}"    // True to return only questions with accepted answers, False to return only those without one
                + $"&answers={minAnswers}"     // Set the minimum number of answers returned questions must have
                + "&site=stackoverflow";       // Lastly, set the site to Stack Overflow

            // Fire off the request
            string response = await MakeRequest(apiUrl);

            // Parse the response into our model (see https://api.stackexchange.com/docs/advanced-search for structure)
            List<Question> result = new List<Question>();
            var questionData = JObject.Parse(response);
            foreach (var question in questionData["items"])
            {
                int id = Int32.Parse(question["question_id"].ToString());
                Question q = new Question(id, question["title"].ToString());

                DateTimeOffset creationDate = DateTimeOffset.FromUnixTimeSeconds(Int32.Parse(question["creation_date"].ToString()));
                q.CreationDateLocal = creationDate.LocalDateTime;

                result.Add(q);
            }

            return result;
        }

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
    }
}