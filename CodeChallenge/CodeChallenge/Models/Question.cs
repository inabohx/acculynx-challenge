using System;

namespace CodeChallenge.Models
{
    public class Question
    {
        public Question(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreationDateLocal { get; set; }

    }
}
