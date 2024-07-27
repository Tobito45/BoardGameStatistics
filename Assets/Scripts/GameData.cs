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
    public GameData(int id, string name, float mark, int countGames, string url)
    {
        this.id = id;
        this.name = name;
        this.mark = mark;
        this.countGames = countGames;
        this.url = url;
    }

    public void Installization(VisualElement root)
    {
        root.Q<Label>("Name").text = name;
        root.Q<Label>("MarkText").text = mark.ToString("F1");
        root.Q<Label>("GamesText").text = countGames.ToString();
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
