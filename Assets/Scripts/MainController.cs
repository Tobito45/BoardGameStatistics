using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MainController 
{
    private readonly GameDataFactory gameDataFactory;
    private readonly VisualElement _root;
    private ScrollView _listView;
    private VisualTreeAsset _prefabElement;

    public MainController(GameDataFactory gameDataFactory, VisualElement root)
    {
        this.gameDataFactory = gameDataFactory;
        _prefabElement = Resources.Load<VisualTreeAsset>("ElementListView");
        _root = root;
        Installization();
    }

    private void Installization()
    {
        _listView = _root.Q<ScrollView>("List");

        foreach (GameData data in gameDataFactory.GetData())
        {
            VisualElement itemUi = _prefabElement.Instantiate();
            _listView.Add(itemUi);
            data.Installization(itemUi);
        }
    }
}
