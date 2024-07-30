using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class GameData
{
    private int id;
    private string name;
    private float mark;
    private int countGames;
    private string url;
    private int countMinutes;
    private int countPlayers;
    public GameData(int id, string name, float mark, int countGames, string url, int countMinutes, int countPlayers)
    {
        this.id = id;
        this.name = name;
        this.mark = mark;
        this.countGames = countGames;
        this.url = url;
        this.countMinutes = countMinutes;
        this.countPlayers = countPlayers;
    }

    public void InstallizationMain(VisualElement root, MainController mainController)
    {
        root.Q<Label>("Name").text = name;
        root.Q<Label>("MarkText").text = (mark / countPlayers).ToString("F1");
        root.Q<Label>("GamesText").text = countGames.ToString();
        root.Q<Button>("MoreButton").clickable.clicked += () => mainController.ActiveMenu(this);
        LoadImageAsync(root.Q<VisualElement>("Image"));
    }

    public void InstallizationGame(VisualElement root)
    {
        root.Q<Label>("Name").text = name;
        root.Q<Label>("GameLength").text = ((float)countMinutes /countGames).ToString("F1") + " min";
        root.Q<Label>("Players").text = countPlayers.ToString();
        root.Q<Label>("CountGames").text = countGames.ToString();
        root.Q<Label>("GameMark").text = (mark / countPlayers).ToString("F1");
        LoadImageAsync(root.Q<VisualElement>("Image"));
    }

    private async void LoadImageAsync(VisualElement image)
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
