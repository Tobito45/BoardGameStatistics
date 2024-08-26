using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class GameData
    {
        private List<Review> _reviews = new List<Review>();
        private List<Game> _games = new List<Game>();
        private List<Character> _characters = new List<Character>();
        private List<string> _urls = new List<string>();
        public int Index {get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public string Description { get; private set; }
        public float Players => _games.Count == 0 ? -1 : (float)_games.Sum(n => n.Players) / _games.Count;

        public int Games => _games.Count;
        private float SumMarkReview => _reviews.Count == 0 ? -1 : _reviews.Sum(n => n.Mark);
        private int PlayersReview => _reviews.Count == 0 ? 1 : _reviews.Count;
        private int CountMinutes => _games.Count == 0 ? 0 : _games.Sum(n => n.Time);

        public GameData(string name, string url, string description)
        {
            Name = name;
            Url = url;
            Description = description;
            Index = 0;
        }
        public void DebugInfo()
        {
            Debug.Log(_urls.Count);
            Debug.Log(_reviews.Count);
            Debug.Log(Mark);
        }
        public void AddReview(params Review[] reviews) => _reviews.AddRange(reviews);
        public void AddGame(params Game[] games) => _games.AddRange(games);
        public void AddCharacter(params Character[] characters) => _characters.AddRange(characters);
        public void AddUrl(params string[] urls) => _urls.AddRange(urls);

        public IEnumerable<Review> GetReviews => _reviews;
        public IEnumerable<Game> GetGames => _games;
        public IEnumerable<Character> GetCharacters => _characters;
        public IEnumerable<string> GetUrls => _urls;
        public void RemoveUrl(string str) => _urls.Remove(str);
        public void RemoveGame(Game game) => _games.Remove(game);
        public void RemoveReview(Review review) => _reviews.Remove(review);
        public bool ContainsUrl(string str) => _urls.Contains(str); //mb slow
        public void SetNewListUrl(List<string> strs)
        {
            _urls = strs;
            Index = 0;
            Debug.Log(_urls.Count);
        }
        public string GetCurrent() =>  _urls[Index];
        public string MoveNextPicture()
        {
            Index++;
            if (Index >= _urls.Count) Index = 0;
            return GetCurrent();
        }
        public string MovePreviusPicture()
        {
            Index--;
            if (Index < 0) Index = _urls.Count - 1;
            return GetCurrent();
        }
        public void AddUrlNotLast(string url)
        {
            int count = _urls.Count;
            if (count == 0)
            {
                _urls.Add(url);
            }
            else
            {
                _urls.Insert(count - 1, url);
            }
        }
        public float Mark => SumMarkReview / PlayersReview;
        public float Time => _games.Count == 0 ? -1 : (float)CountMinutes / Games;
    }

    public class Review
    {
        public string Name { get; set; }
        public float Mark { get; set; }
        public string Text { get; set; }

        public Review(string name, float mark, string text)
        {
            Name = name;
            Mark = mark;
            Text = text;
        }
    }

    public class Game
    {
        public int Players { get; set; }
        public int Time { get; set; }
        public List<(Character, int)> Winners { get; private set; }
        public List<(Character, int)> Losers { get; private set; }
        public bool isLoser = false;
        public string Comment { get; set; }
        public Game(int players, int time, string comment)
        {
            Players = players;
            Time = time;
            Comment = comment;
            CreateLists();
        }
        public Game()
        {
            CreateLists();
        }

        public void CreateLists()
        {
            Winners = new List<(Character, int)>();
            Losers = new List<(Character, int)>();
        }

        public void AddNewCharacter(Character character, int points)
        {
            if (isLoser)
                Losers.Add((character, points));
            else
                Winners.Add((character, points));
        }
    }

    public class Character
    {
        public string Name { get; private set; }
        public int Games { get; private set; }
        public int Wins { get; private set; }
        public int Points { get; private set; }
        public float Percent => Games == 0 ? 0 : (float)Wins / Games * 100;
        public float AveragePoints => Games == 0 ? 0 : (float)Points / Games;
        public Character(string name)//, int games, int wins)
        {
            Name = name;
           // Games = games;
           // Wins = wins;
        }
        public void ChangeStats(int newGames, int newWins)
        {
            Games = newGames;
            Wins = newWins;
        }
        public void CalculateData(IEnumerable<List<(Character, int)>> winsList, IEnumerable<List<(Character, int)>> losesList)
        {
            int wins = 0, games = 0, points = 0;

            foreach(var list in winsList) 
                CalculateList(list, ref wins, ref games, ref points);
            foreach (var list in losesList)
                CalculateList(list, ref wins, ref games, ref points, false);
            Wins = wins;
            Games = games;
            Points = points;
        }

        private void CalculateList(List<(Character, int)> list, ref int wins, ref int games, ref int points, bool isWins = true)
        {
            foreach ((Character character, int characterPoints) in list)
            {
                if (character.Name.Equals(this.Name))
                {
                    if (isWins) wins++;
                    games++;
                    points += characterPoints;
                }
            }
        }
    }
}