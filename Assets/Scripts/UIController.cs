using States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIStateControllers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Zenject;

public class UIController 
{
    private GameDataFactory _gameDataFactory;
    private GameData _actualData;
    public StateMachine StateMachine { get; private set; }

    private Dictionary<Type, IUIState> _statesControllers;

    [Inject]
    public void Construct(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public  UIController(GameDataFactory gameDataFactory)
    {
        _gameDataFactory = gameDataFactory;

        _statesControllers = new Dictionary<Type, IUIState>()
        {
            {typeof(MainState), new MainUIStateController(this, _gameDataFactory) },
            {typeof(GameState), new GameUIStateController(this) },
            {typeof(ActionsState), new ActionsUIStateController(this) },
            {typeof(ReviewsState), new ReviewsUIStateController(this) },
            {typeof(ReviewInputState), new ReviewsInputUIStateController(this) },
            {typeof(GamesInfoState), new GamesInfoUIStateController(this) },
            {typeof(GamesInfoInputState), new GamesInfoInputUIStateController(this) },
        };
    }
    public void SetActualData(GameData gameData) => _actualData = gameData;
    public GameData GetActualData => _actualData;

    public IUIState GetController(BaseState state)
    {
        return _statesControllers[state.GetType()];
    } 
    public async void LoadImageAsync(VisualElement image, string url)
    {
        Texture2D texture = await GetTextureFromUrlAsync(url);
        if (texture != null)
            image.style.backgroundImage = new StyleBackground(texture);
    }

    private async Task<Texture2D> GetTextureFromUrlAsync(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            var asyncOperation = www.SendWebRequest();

            while (!asyncOperation.isDone)
                await Task.Yield();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                return null;
            }
            else
                return DownloadHandlerTexture.GetContent(www);
        }
    }

    public void SetInputFieldColor(VisualElement textField, Color color, int boardSize)
    {
        textField.style.borderBottomColor = color;
        textField.style.borderTopColor = color;
        textField.style.borderLeftColor = color;
        textField.style.borderRightColor = color;

        textField.style.borderBottomWidth = boardSize;
        textField.style.borderTopWidth = boardSize;
        textField.style.borderLeftWidth = boardSize;
        textField.style.borderRightWidth = boardSize;
    }
}
