using States;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Zenject;

public class UIController : MonoBehaviour
{
    private GameData _actualData;
    private StateMachine _stateMachine;

    [Inject]
    public void Construct(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public void SetActualData(GameData gameData) => _actualData = gameData;

    public void InstallizationMain(VisualElement root, GameData data)
    {
        root.Q<Label>("Name").text = data.Name;
        root.Q<Label>("MarkText").text = data.Mark.ToString("F1");
        root.Q<Label>("GamesText").text = data.Games.ToString();
        root.Q<Button>("MoreButton").clickable.clicked += () =>
        {
            SetActualData(data);
            _stateMachine.SetGameState();
        };
        LoadImageAsync(root.Q<VisualElement>("Image"), data.Url);
    }

    public void InstallizationGame(VisualElement root)
    {
        root.Q<Label>("Name").text = _actualData.Name;
        root.Q<Label>("GameLength").text = _actualData.Games.ToString("F1") + " min";
        root.Q<Label>("Players").text = _actualData.Players.ToString();
        root.Q<Label>("CountGames").text = _actualData.Games.ToString();
        root.Q<Label>("GameMark").text = _actualData.Mark.ToString("F1");
        root.Q<Label>("Description").text = _actualData.Description;
        root.Q<Button>("BackButton").clickable.clicked += () => _stateMachine.SetMainState();
        root.Q<Button>("ButtonActions").clickable.clicked += () => _stateMachine.SetActionsState();
        LoadImageAsync(root.Q<VisualElement>("Image"), _actualData.Url);
    }

    public void ClearGame(VisualElement root)
    {
        root.Q<Button>("BackButton").clickable.clicked -= () => _stateMachine.SetMainState();
        root.Q<Button>("ButtonActions").clickable.clicked -= () => _stateMachine.SetActionsState();
    }

    public void InstallizationActions(VisualElement root)
    {
        root.Q<Button>("BackButton").clickable.clicked += () => _stateMachine.SetGameState();
    }
    public void ClearActions(VisualElement root)
    {
        root.Q<Button>("BackButton").clickable.clicked -= () => _stateMachine.SetGameState();
    }

    private async void LoadImageAsync(VisualElement image, string url)
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
}
