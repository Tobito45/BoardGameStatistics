using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class GameData
{
    private float mark;
    private int countMinutes;
    public string Name { get; private set; }
    public int Games { get; private set; }
    public int Players { get; private set; } 
    public string Url { get; private set; }
    public string Description { get; private set; }
    public GameData(int id, string name, float mark, int countGames, string url, int countMinutes, int countPlayers, string description)
    {
        this.countMinutes = countMinutes;
        this.mark = mark;
        Name = name;
        Games = countGames;
        Url = url;
        Players = countPlayers;
        Description = description;
    }

    public float Mark => mark / Players;
    public float Time => (float)countMinutes / Games;
}
