using System.IO;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public string weatherApiKey;
    public static ConfigManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the ConfigManager alive between scenes.
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("Hi");
        string path = Path.Combine(Application.streamingAssetsPath, "settings.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Config config = JsonUtility.FromJson<Config>(json);
            weatherApiKey = config.WeatherApiKey;
        }
    }
}

[System.Serializable]
public class Config
{
    public string WeatherApiKey;
}