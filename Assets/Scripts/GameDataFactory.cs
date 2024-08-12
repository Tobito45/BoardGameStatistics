using System.Collections.Generic;
using Data;
using UnityEngine;
using Newtonsoft.Json;

public class GameDataFactory
{
    private List<GameData> data;

    public GameDataFactory()
    {
        data = LoadPlayerData();
        if (data != null)
            return;

        data = new List<GameData>();
        data.Add(new GameData("Dead by daylight", "https://cf.geekdo-images.com/fpnjJSQWpAh1WZW3zJ807A__itemrep/img/0FyP7gUgtFxQS8vq2p_VocShE84=/fit-in/246x300/filters:strip_icc()/pic6727475.jpg",
            "Death is not an escape. The hit asymmetric survival horror game comes to tabletop! Take on the role of a ruthless Killer or a resourceful Survivor as you navigate sinister trials.   As a Survivor, plot your moves, coordinate with your allies, and repair generators to power the exit and escape. "
            ));
        //data.Add(new GameData(1, "Spirit Island", "https://cf.geekdo-images.com/kjCm4ZvPjIZxS-mYgSPy1g__itemrep/img/7AXozbOIxk5MDpn_RNlat4omAcc=/fit-in/246x300/filters:strip_icc()/pic7013651.jpg",
        //    "Spirit Island is a complex and thematic cooperative game about defending your island home from colonizing Invaders. Players are different spirits of the land, each with its own unique elemental powers. Every turn, players simultaneously choose which of their power cards to play, paying energy to do so. Using combinations of power cards that match a spirit's elemental affinities can grant free bonus effects."));
        //data.Add(new GameData(3, "Colt express", "https://cf.geekdo-images.com/2HKX0QANk_DY7CIVK5O5fQ__itemrep/img/TvYm-n4tYlxtLfE2iU-aBDeRC5I=/fit-in/246x300/filters:strip_icc()/pic2869710.jpg",
        //     "On the 11th of July, 1899 at 10 a.m., the Union Pacific Express has left Folsom, New Mexico, with 47 passengers on board. After a few minutes, gunfire and hurrying footsteps on the roof can be heard. Heavily armed bandits have come to rob honest citizens of their wallets and jewels. Will they succeed in stealing the suitcase holding the Nice Valley Coal Company's weekly pay, despite it having been placed under the supervision of Marshal Samuel Ford? "));
        //data.Add(new GameData(4, "Unmatched", "https://im9.cz/sk/iR/importprodukt-orig/169/16991d4953b9d1871074915cb3fca125.jpg",
        //     "Unmatched is a highly asymmetrical miniature fighting game for two or four players. Each hero is represented by a unique deck designed to evoke their style and legend. Tactical movement and no-luck combat resolution create a unique play experience that rewards expertise, but just when you've mastered one set, new heroes arrive to provide all new match-ups."
         //   ));

       // CreatingReviews();
       // CreateGames();
        CreateCharacters();

        SaveData();
    }


    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    private List<GameData> LoadPlayerData()
    {
        if(!PlayerPrefs.HasKey("PlayerData"))
        {
            return null;
        }

        string json = PlayerPrefs.GetString("PlayerData", "{}");
        List<GameData> data = JsonConvert.DeserializeObject<List<GameData>>(json);
        return data;
    }

    public void AddBoardGame(GameData gameData) => data.Add(gameData);

    private void CreatingReviews()
    {
        data[0].AddReview(new Review("Dmitro", 7.8f, "Bla bla hui sosi"),
                           new Review("Dmitro2", 7.8f, "Bla bla hui sosi"),
                           new Review("Dushnila", 10f, "Can't recommend this game enough.\r\n\r\nVery good at 2,3 and 5. Components are very well designed. With 3k+ hours on the video game, I can tell that multiple aspects of the game are VERY WELL implemented into the game.\r\n\r\nIt's hard to win as killer until the player in that role has a lot of experience.\r\n\r\nI recommend a SMALL house rule until then, if the players are able to power the door on the last round, you still play the killer's turn. If the killer reaches his wincon before the end of the game, he wins instead of the survivors. It will help smooth out the stats (because even with that small rule, we still had about only a 30% winrate on killer)."));
        data[1].AddReview(new Review("Ja ja", 2.3f, "Lublu etu ihru"));
    }
    private void CreateGames()
    {
        data[0].AddGame(new Game(5, 180, ""), new Game(4, 260, "Ebat trahniu mena"), new Game(5, 360, ""), new Game(5, 200, "Can't recommend this game enough.\r\n\r\nVery good at 2,3 and 5. Components are very well designed. With 3k+ hours on the video game, I can tell that multiple aspects of the game are VERY WELL implemented into the game.\r\n\r\nIt's hard to win as killer until the player in that role has a lot of experience.\r\n\r\nI recommend a SMALL house rule until then, if the players are able to power the door on the last round, you still play the killer's turn. If the killer reaches his wincon before the end of the game, he wins instead of the survivors. It will help smooth out the stats (because even with that small rule, we still had about only a 30% winrate on killer)."));
        data[2].AddGame(new Game(4, 60, ""), new Game(6, 140, ""), new Game(4, 80, ""), new Game(5, 120, ""));
    }

    private void CreateCharacters()
    {
        data[0].AddCharacter(new Character("Охотник", 0, 0), new Character("Деревенщина", 0, 0), new Character("Медсестра", 0, 0),
           new Character("Доктор", 0, 0), new Character("Ведьма", 0, 0), new Character("Призрак", 0, 0));
    }

    public IEnumerable<GameData> GetData() => data;
}
