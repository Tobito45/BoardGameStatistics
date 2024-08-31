using System.IO;
using UnityEngine;
using NativeFilePickerNamespace;
using System;
using Zenject;

public class ShareController : MonoBehaviour
{
    private UIController _uiController;

    [Inject]
    public void Contruct(UIController uIController)
    {
        _uiController = uIController;
    }

    private void Start()
    {
     //   GettingFile();       
    }

    void GettingFile()
    {
        // Проверка на то, что приложение запущено с "поделившимся" файлом
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
        string action = intent.Call<string>("getAction");

        if (action == "android.intent.action.SEND")
        {
            AndroidJavaObject uri = intent.Call<AndroidJavaObject>("getParcelableExtra", "android.intent.extra.STREAM");

            if (uri != null)
            {
                string path = GetPathFromUri(uri);
                if (!string.IsNullOrEmpty(path))
                {
                    // Вывод пути файла в Unity
                    Debug.Log("Received file path: " + path);

                    string content = System.IO.File.ReadAllText(path);
                    Debug.Log("File content: " + content);
                    _uiController.TryToImportJson(content);
                }
            }
        }
    }

    string GetPathFromUri(AndroidJavaObject uri)
    {
        return uri.Call<string>("getPath");
    }

    public static void ExportData(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        string content = PlayerPrefs.GetString("PlayerData", "{}");

        File.WriteAllText(path, content);
        Debug.Log("File saved at: " + path);
    }

    public static void ShareGeneratedFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            new NativeShare().AddFile(filePath)
                             .SetSubject("Check this out!")
                             .SetText("Here is a file I generated with my data.")
                             .Share();
        }
        else
        {
            Debug.Log("File not found: " + filePath);
        }
    }

    public static void PickFile(Action<string> callBack)
    {
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.Log("Operation cancelled");
                callBack(null);
            }
            else
            {
                Debug.Log("File path: " + path);

                string fileContent = File.ReadAllText(path);
                callBack(fileContent);
            }
        });

        if (permission == NativeFilePicker.Permission.Denied)
        {
            Debug.Log("User denied permission to access files");
            callBack(null);
        }
        else if (permission == NativeFilePicker.Permission.ShouldAsk)
        {
            Debug.Log("Permission should be asked");
            callBack(null);
        }
    }
}
