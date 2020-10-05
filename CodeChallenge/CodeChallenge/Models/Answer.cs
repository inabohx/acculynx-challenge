using Newtonsoft.Json.Linq;
using System;

namespace CodeChallenge.Models
{
    /// <summary>
    /// Model class representing one answer for a question on Stack Overflow
    /// </summary>
    public class Answer
    {
        #region Constructors

        public Answer()
        {

        }

        /// <summary>
        /// Constructor that takes a JSON token representing answer data coming from a Stack Exchange API.
        /// See https://api.stackexchange.com/docs/questions-by-ids for structure/sample.
        /// </summary>
        /// <param name="answer">JToken object representing one answer's info</param>
        public Answer(JToken answer)
        {
            Id = Int32.Parse(answer["answer_id"].ToString());

            if (answer["body"] != null)
            {
                BodyHTML = answer["body"].ToString();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unique identifier for an answer on Stack Overflow
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The body of the answer (is already HTML encoded)
        /// </summary>
        public string BodyHTML { get; set; }

        #endregion
    }
}
