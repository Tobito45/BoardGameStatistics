using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class MainController : MonoBehaviour
{
    [SerializeField]
    private UIDocument _main, _game;
    private VisualElement _mainRoot, _gameRoot;

    private GameDataFactory gameDataFactory;
    private ScrollView _listView;
    private VisualTreeAsset _prefabElement;

    [Inject]
    public void Construct(GameDataFactory gameDataFactory)
    {
        this.gameDataFactory = gameDataFactory;
        _prefabElement = Resources.Load<VisualTreeAsset>("ElementListView");
        _mainRoot = _main.rootVisualElement;
        _gameRoot = _game.rootVisualElement;

        InstallizationMain();
        InstallizationGame();
        ActiveMenu(null);
    }

    private void InstallizationMain()
    {

        _listView = _mainRoot.Q<ScrollView>("List");

        foreach (GameData data in gameDataFactory.GetData())
        {
            VisualElement itemUi = _prefabElement.Instantiate();
            _listView.Add(itemUi);
            data.InstallizationMain(itemUi, this);
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
            gameData.InstallizationGame(_gameRoot);
        }
    }
}
