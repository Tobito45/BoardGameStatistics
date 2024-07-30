using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class GameDataController 
{
    private GameData actualData;


    public void SetActualData(GameData gameData) => actualData = gameData;

    public void InstallizationMain(VisualElement root, GameData data, UIController uIController)
    {
        root.Q<Label>("Name").text = data.Name;
        root.Q<Label>("MarkText").text = data.Mark.ToString("F1");
        root.Q<Label>("GamesText").text = data.Games.ToString();
        root.Q<Button>("MoreButton").clickable.clicked += () =>
        {
            SetActualData(data);
            uIController.ActiveMenu(actualData);
        };
        LoadImageAsync(root.Q<VisualElement>("Image"), data.Url);
    }

    public void InstallizationGame(VisualElement root)
    {
        root.Q<Label>("Name").text = actualData.Name;
        root.Q<Label>("GameLength").text = actualData.Games.ToString("F1") + " min";
        root.Q<Label>("Players").text = actualData.Players.ToString();
        root.Q<Label>("CountGames").text = actualData.Games.ToString();
        root.Q<Label>("GameMark").text = actualData.Mark.ToString("F1");
        root.Q<Label>("Description").text = actualData.Description;
        LoadImageAsync(root.Q<VisualElement>("Image"), actualData.Url);
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
