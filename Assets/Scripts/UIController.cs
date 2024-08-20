using Data;
using States;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
    private Texture2D _loadingSprite, _errorSprite;

    public StateMachine StateMachine { get; private set; }
    public  Character ActualCharater { get; set; }
    public Game ActualGame { get; set; }

    private Dictionary<Type, IUIState> _statesControllers;

    [Inject]
    public void Construct(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public UIController(GameDataFactory gameDataFactory)
    {
        _gameDataFactory = gameDataFactory;
        _loadingSprite = Resources.Load<Texture2D>("Pictures/loading");
        _errorSprite = Resources.Load<Texture2D>("Pictures/error");

        _statesControllers = new Dictionary<Type, IUIState>()
        {
            {typeof(MainState), new MainUIStateController(this, _gameDataFactory) },
            {typeof(GameState), new GameUIStateController(this) },
            {typeof(ActionsState), new ActionsUIStateController(this) },
            {typeof(ReviewsState), new ReviewsUIStateController(this) },
            {typeof(ReviewInputState), new ReviewsInputUIStateController(this) },
            {typeof(GamesInfoState), new GamesInfoUIStateController(this) },
            {typeof(GamesInfoInputState), new GamesInfoInputUIStateController(this) },
            {typeof(CharactersState), new CharactersUIStateController(this) },
            {typeof(CharacterNewInputState), new CharacterNewInputUIStateController(this) },
            {typeof(CharacterChangeInputState), new CharacterChangeInputUIStateController(this) },
            {typeof(GameNewInputState), new GameNewInputUIStateController(this, _gameDataFactory) },
            {typeof(StartScreenState), new StartScreenStateController(this) },
            {typeof(UrlInputState), new UrlInputUIStateController(this) },
            {typeof(GamesCharacterInputState), new GamesCharacterInputUIStateController(this) },
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
        image.style.backgroundImage = _loadingSprite;
        if (url != string.Empty)
        {
            Texture2D texture = await GetTextureFromUrlAsync(url);
            if (texture != null)
            {
                image.style.backgroundImage = new StyleBackground(texture);
                return;
            }
        }
        image.style.backgroundImage = _errorSprite;
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
    public void PickImage(VisualElement image, ref string pathImage, TextField textField)
    {
        string pathsave = string.Empty;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                LoadImageFromLocalStorage(image, path);
                textField.value = path;
                pathsave = path;
            } else
            {
                image.style.backgroundImage = _errorSprite;
            }
        });
        pathImage = pathsave;
    }
    private void LoadImageFromLocalStorage(VisualElement image, string path)
    {
        Texture2D texture = NativeGallery.LoadImageAtPath(path);
        if (texture == null)
        {
            image.style.backgroundImage = _errorSprite;
            return;
        }
        image.style.backgroundImage = texture;
    }

    public void LoadImage(VisualElement image, string path)
    {
        string urlPattern = @"^(http|https|ftp)://";
        if  (path.StartsWith("http://") || path.StartsWith("https://"))//(Regex.IsMatch(path, urlPattern)) 
            LoadImageAsync(image, path);
        else
            LoadImageFromLocalStorage(image, path);
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
