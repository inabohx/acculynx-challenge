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

        /// <summary>
        /// Method to shuffle the Answers list. Not the strongest of random logic, but should be ok.
        /// Logic taken from https://www.programming-idioms.org/idiom/10/shuffle-a-list/1352/csharp
        /// </summary>
        public void ShuffleAnswers()
        {
            if (Answers == null) { return; }
            if (Answers.Count < 2) { return; }

            Random rng = new Random();  

            int n = Answers.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Answer value = Answers[k];
                Answers[k] = Answers[n];
                Answers[n] = value;
            }
        }
    }
}
