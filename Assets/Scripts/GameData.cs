using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class GameData
    {
        private List<Review> _reviews = new List<Review>();
        private List<Game> _games = new List<Game>();
        private List<Character> _characters = new List<Character>();
        public string Name { get; private set; }
        public string Url { get; private set; }
        public string Description { get; private set; }
        public float Players => _games.Count == 0 ? -1 : (float)_games.Sum(n => n.Players) / _games.Count;

        public int Games => _games.Count;
        private float SumMarkReview => _reviews.Count == 0 ? -1 : _reviews.Sum(n => n.Mark);
        private int PlayersReview => _reviews.Count == 0 ? 1 : _reviews.Count;
        private int CountMinutes => _games.Count == 0 ? 0 : _games.Sum(n => n.Time);

        public GameData(int id, string name, string url, string description)
        {
            Name = name;
            Url = url;
            Description = description;
        }

        public void AddReview(params Review[] reviews) => _reviews.AddRange(reviews);
        public void AddGame(params Game[] games) => _games.AddRange(games);
        public void AddCharacter(params Character[] characters) => _characters.AddRange(characters);
        public IEnumerable<Review> GetReviews => _reviews;
        public IEnumerable<Game> GetGames => _games;
        public IEnumerable<Character> GetCharacters => _characters;

        public float Mark => SumMarkReview / PlayersReview;
        public float Time => _games.Count == 0 ? -1 : (float)CountMinutes / Games;
    }

    public class Review
    {
        public string Name { get; private set; }
        public float Mark { get; private set; }
        public string Text { get; private set; }

        public Review(string name, float mark, string text)
        {
            Name = name;
            Mark = mark;
            Text = text;
        }
    }

    public class Game
    {
        public int Players { get; private set; }
        public int Time { get; private set; }

        public string Comment { get; private set; }
        public Game(int players, int time, string comment)
        {
            Players = players;
            Time = time;
            Comment = comment;
        }
    }

    public class Character
    {
        public string Name { get; private set; }
        public int Games { get; private set; }
        public int Wins { get; private set; }

        public float Percent => Games == 0 ? 0 : (float)Wins / Games * 100;
        public Character(string name, int games, int wins)
        {
            Name = name;
            Games = games;
            Wins = wins;
        }
        public void ChangeStats(int newGames, int newWins)
        {
            Games = newGames;
            Wins = newWins;
        }
    }
}