using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class UIController
{
    private readonly GameDataFactory _gameDataFactory;
    private readonly VisualElement _mainRoot, _gameRoot;
    private readonly GameDataController _gameDataController;

    private ScrollView _listView;
    private VisualTreeAsset _prefabElement;

    public UIController(GameDataFactory gameDataFactory, UIDocument main, UIDocument game, GameDataController gameDataController)
    {
        _gameDataFactory = gameDataFactory;
        _gameDataController = gameDataController;
        _prefabElement = Resources.Load<VisualTreeAsset>("ElementListView");
        _mainRoot = main.rootVisualElement;
        _gameRoot = game.rootVisualElement;

        InstallizationMain();
        InstallizationGame();
        ActiveMenu(null);
    }

    private void InstallizationMain()
    {

        _listView = _mainRoot.Q<ScrollView>("List");

        foreach (GameData data in _gameDataFactory.GetData())
        {
            VisualElement itemUi = _prefabElement.Instantiate();
            _listView.Add(itemUi);
            _gameDataController.InstallizationMain(itemUi, data, this);
        }
    }

    private void InstallizationGame()
    {
        _gameRoot.Q<Button>("BackButton").clickable.clicked += () => ActiveMenu(null);
    }

    public void ActiveMenu(GameData gameData)
    {
        if(gameData == null)
        {
            _mainRoot.style.display = DisplayStyle.Flex;
            _gameRoot.style.display = DisplayStyle.None;
        } else
        {
            _mainRoot.style.display = DisplayStyle.None;
            _gameRoot.style.display = DisplayStyle.Flex;
            _gameDataController.InstallizationGame(_gameRoot);
        }
    }
}
