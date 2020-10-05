using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CodeChallenge.Models
{
    /// <summary>
    /// Model class representing one question from Stack Overflow and its list of answers
    /// </summary>
    public class QuestionAndAnswers
    {
        #region Constructors

        public QuestionAndAnswers()
        {
            Answers = new List<Answer>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The question this class is for
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// A list of all answers to this class's question
        /// </summary>
        public List<Answer> Answers { get; set; }

        #endregion
    }
}
