using System.Collections.Generic;
using UnityEngine;

public class GameDataFactory
{
    private List<GameData> data = new List<GameData>();

    public GameDataFactory()
    {
        data.Add(new GameData(0, "Dead by daylight", 7.2f, 10, "https://cf.geekdo-images.com/fpnjJSQWpAh1WZW3zJ807A__itemrep/img/0FyP7gUgtFxQS8vq2p_VocShE84=/fit-in/246x300/filters:strip_icc()/pic6727475.jpg"));
        data.Add(new GameData(1, "Spirit Island", 6.25f, 115, "https://cf.geekdo-images.com/kjCm4ZvPjIZxS-mYgSPy1g__itemrep/img/7AXozbOIxk5MDpn_RNlat4omAcc=/fit-in/246x300/filters:strip_icc()/pic7013651.jpg"));
        data.Add(new GameData(2, "Colt express", 6f, 2, "https://cf.geekdo-images.com/2HKX0QANk_DY7CIVK5O5fQ__itemrep/img/TvYm-n4tYlxtLfE2iU-aBDeRC5I=/fit-in/246x300/filters:strip_icc()/pic2869710.jpg"));
        data.Add(new GameData(2, "Colt express Colt expres", 6f, 2, "https://cf.geekdo-images.com/2HKX0QANk_DY7CIVK5O5fQ__itemrep/img/TvYm-n4tYlxtLfE2iU-aBDeRC5I=/fit-in/246x300/filters:strip_icc()/pic2869710.jpg"));
        Debug.Log("yes?");
    }

    public IEnumerable<GameData> GetData() => data;
}
