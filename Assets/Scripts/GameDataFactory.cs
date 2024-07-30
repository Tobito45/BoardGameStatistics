using System.Collections.Generic;

public class GameDataFactory
{
    private List<GameData> data = new List<GameData>();

    public GameDataFactory()
    {
        data.Add(new GameData(0, "Dead by daylight", 120.2f, 10, "https://cf.geekdo-images.com/fpnjJSQWpAh1WZW3zJ807A__itemrep/img/0FyP7gUgtFxQS8vq2p_VocShE84=/fit-in/246x300/filters:strip_icc()/pic6727475.jpg",
            600, 10));
        data.Add(new GameData(1, "Spirit Island", 622.5f, 115, "https://cf.geekdo-images.com/kjCm4ZvPjIZxS-mYgSPy1g__itemrep/img/7AXozbOIxk5MDpn_RNlat4omAcc=/fit-in/246x300/filters:strip_icc()/pic7013651.jpg",
            300, 20));
        data.Add(new GameData(3, "Colt express", 600f, 2, "https://cf.geekdo-images.com/2HKX0QANk_DY7CIVK5O5fQ__itemrep/img/TvYm-n4tYlxtLfE2iU-aBDeRC5I=/fit-in/246x300/filters:strip_icc()/pic2869710.jpg",
            100, 20));
        data.Add(new GameData(4, "Unmatched", 60f, 22, "https://im9.cz/sk/iR/importprodukt-orig/169/16991d4953b9d1871074915cb3fca125.jpg",
            300, 5));
    }

    public IEnumerable<GameData> GetData() => data;
}
