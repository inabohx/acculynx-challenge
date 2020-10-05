using Newtonsoft.Json.Linq;
using System;

namespace CodeChallenge.Models
{
    /// <summary>
    /// Model class representing one question from Stack Overflow
    /// </summary>
    public class Question
    {
        #region Constructors

        public Question()
        {

        }

        /// <summary>
        /// Constructor that takes a JSON token representing question data coming from a Stack Exchange API.
        /// See https://api.stackexchange.com/docs/advanced-search for structure/sample.
        /// </summary>
        /// <param name="question">JToken object representing one question's info</param>
        public Question(JToken question)
        {
            Id = Int32.Parse(question["question_id"].ToString());
            Title = question["title"].ToString();

            if (question["creation_date"] != null)
            {
                DateTimeOffset creationDate = DateTimeOffset.FromUnixTimeSeconds(Int32.Parse(question["creation_date"].ToString()));
                CreationDateLocal = creationDate.LocalDateTime;
            }

            if (question["body"] != null)
            {
                BodyHTML = question["body"].ToString();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unique identifier for a question on Stack Overflow
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the question that is ready for display (is already HTML encoded)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Date and time of when the question was created (in local time zone)
        /// </summary>
        public DateTime CreationDateLocal { get; set; }

        /// <summary>
        /// The body of the question (is already HTML encoded)
        /// </summary>
        public string BodyHTML { get; set; }

        #endregion
    }
}
