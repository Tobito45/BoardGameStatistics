using States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Zenject;

public class UIController : MonoBehaviour
{
    private GameData _actualData;
    private StateMachine _stateMachine;

    VisualTreeAsset prefabReviewElement;
    VisualTreeAsset prefabMainElement;

    private void Awake()
    {
        prefabReviewElement = Resources.Load<VisualTreeAsset>("ReviewElement");
        prefabMainElement = Resources.Load<VisualTreeAsset>("MainElement");
    }

    [Inject]
    public void Construct(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;

    }
    public void SetActualData(GameData gameData) => _actualData = gameData;

    public void InstallizationMain(VisualElement root, IEnumerable<GameData> allData)
    {
        ScrollView listView = root.Q<ScrollView>("List");
        foreach (GameData data in allData)
        {
            if (prefabMainElement == null)
                Awake();

            VisualElement itemUi = prefabMainElement.Instantiate();
            listView.Add(itemUi);
            itemUi.Q<Label>("Name").text = data.Name;
            itemUi.Q<Label>("MarkText").text = data.Mark.ToString("F1");
            itemUi.Q<Label>("GamesText").text = data.Games.ToString();
            itemUi.Q<Button>("MoreButton").clicked += () =>
            {
                SetActualData(data);
                _stateMachine.SetGameState();
            };
            LoadImageAsync(itemUi.Q<VisualElement>("Image"), data.Url);
        }

        
    }

    public void InstallizationGame(VisualElement root)
    {
        root.Q<Button>("BackButton").clicked += () => _stateMachine.SetMainState();
        root.Q<Button>("ButtonActions").clicked += () => _stateMachine.SetActionsState();
    }

    public void UpdateGame(VisualElement root)
    {
        root.Q<Label>("Name").text = _actualData.Name;
        root.Q<Label>("GameLength").text = _actualData.Games.ToString("F1") + " min";
        root.Q<Label>("Players").text = _actualData.Players.ToString();
        root.Q<Label>("CountGames").text = _actualData.Games.ToString();
        root.Q<Label>("GameMark").text = _actualData.Mark.ToString("F1");
        root.Q<Label>("Description").text = _actualData.Description;
        LoadImageAsync(root.Q<VisualElement>("Image"), _actualData.Url);
    }

    public void InstallizationActions(VisualElement root)
    {
        root.Q<Button>("BackButton").clicked += () => _stateMachine.SetGameState();
        root.Q<Button>("ButtonReview").clicked += () => _stateMachine.SetReviewsState();
    }

    public void UpdateActions(VisualElement root)
    {
        root.Q<Label>("Name").text = _actualData.Name;
    }

    public void InstallizationReviews(VisualElement root)
    {
        root.Q<Button>("BackButton").clicked += () => _stateMachine.SetActionsState();
    }

    public void UpdateReviews(VisualElement root)
    {
        if (root.Q<Label>("Name").text != _actualData.Name || _actualData.GetReviews.Count() != root.Q<ScrollView>("List").childCount)
        {
            root.Q<ScrollView>("List").Clear();

            root.Q<Label>("Name").text = _actualData.Name;
            ScrollView listView = root.Q<ScrollView>("List");
            foreach (Review review in _actualData.GetReviews)
            {
                VisualElement itemUi = prefabReviewElement.Instantiate();
                itemUi.Q<Label>("Name").text = review.Name;
                itemUi.Q<Label>("Text").text = review.Text;
                itemUi.Q<Label>("Mark").text = review.Mark.ToString("F1");
                listView.Add(itemUi);
            }
        }
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
