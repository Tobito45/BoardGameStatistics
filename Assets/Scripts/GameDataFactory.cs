using System.Collections.Generic;

public class GameDataFactory
{
    private List<GameData> data = new List<GameData>();

    public GameDataFactory()
    {
        data.Add(new GameData(0, "Dead by daylight", 120.2f, 10, "https://cf.geekdo-images.com/fpnjJSQWpAh1WZW3zJ807A__itemrep/img/0FyP7gUgtFxQS8vq2p_VocShE84=/fit-in/246x300/filters:strip_icc()/pic6727475.jpg",
            600, 10,
            "Death is not an escape. The hit asymmetric survival horror game comes to tabletop! Take on the role of a ruthless Killer or a resourceful Survivor as you navigate sinister trials.   As a Survivor, plot your moves, coordinate with your allies, and repair generators to power the exit and escape. "
            ));
        data.Add(new GameData(1, "Spirit Island", 622.5f, 115, "https://cf.geekdo-images.com/kjCm4ZvPjIZxS-mYgSPy1g__itemrep/img/7AXozbOIxk5MDpn_RNlat4omAcc=/fit-in/246x300/filters:strip_icc()/pic7013651.jpg",
            300, 20, "Spirit Island is a complex and thematic cooperative game about defending your island home from colonizing Invaders. Players are different spirits of the land, each with its own unique elemental powers. Every turn, players simultaneously choose which of their power cards to play, paying energy to do so. Using combinations of power cards that match a spirit's elemental affinities can grant free bonus effects."));
        data.Add(new GameData(3, "Colt express", 600f, 2, "https://cf.geekdo-images.com/2HKX0QANk_DY7CIVK5O5fQ__itemrep/img/TvYm-n4tYlxtLfE2iU-aBDeRC5I=/fit-in/246x300/filters:strip_icc()/pic2869710.jpg",
            100, 20, "On the 11th of July, 1899 at 10 a.m., the Union Pacific Express has left Folsom, New Mexico, with 47 passengers on board. After a few minutes, gunfire and hurrying footsteps on the roof can be heard. Heavily armed bandits have come to rob honest citizens of their wallets and jewels. Will they succeed in stealing the suitcase holding the Nice Valley Coal Company's weekly pay, despite it having been placed under the supervision of Marshal Samuel Ford? "));
        data.Add(new GameData(4, "Unmatched", 60f, 22, "https://im9.cz/sk/iR/importprodukt-orig/169/16991d4953b9d1871074915cb3fca125.jpg",
            300, 5, "Unmatched is a highly asymmetrical miniature fighting game for two or four players. Each hero is represented by a unique deck designed to evoke their style and legend. Tactical movement and no-luck combat resolution create a unique play experience that rewards expertise, but just when you've mastered one set, new heroes arrive to provide all new match-ups."
            ));
    }

    public IEnumerable<GameData> GetData() => data;
}
