using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    /// <summary>
    /// Model class representing a list of questions and other data the view might need
    /// </summary>
    public class QuestionList
    {
        public QuestionList(List<Question> questions)
        {
            Questions = questions;
        }

        /// <summary>
        /// List of questions the user can click on
        /// </summary>
        public List<Question> Questions { get; set; }

        /// <summary>
        /// Populate this when there is something that should be displayed after the view loads
        /// </summary>
        public string AlertMessage { get; set; }
    }
}
