using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [Header("Settings UI")]
    public GameObject settingsPanel;

    [Header("Weather Manager Reference")]
    public WeatherManager weatherManager;

    public void ToggleSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    public void SetSunnyWeather()
    {
        if (weatherManager != null)
            weatherManager.SetSunny();
    }

    public void SetSnowWeather()
    {
        if (weatherManager != null)
            weatherManager.SetSnow();
    }
}
