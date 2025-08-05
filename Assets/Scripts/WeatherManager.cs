using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [Header("Field Appearance")]
    public SpriteRenderer fieldRenderer;
    public Sprite sunnyFieldSprite;
    public Sprite snowFieldSprite;

    private void Start()
    {
        SetSunny(); // Default on start
    }

    public void SetSunny()
    {
        if (fieldRenderer && sunnyFieldSprite)
        {
            fieldRenderer.sprite = sunnyFieldSprite;
            Debug.Log("Weather set to Sunny.");
        }
    }

    public void SetSnow()
    {
        if (fieldRenderer && snowFieldSprite)
        {
            fieldRenderer.sprite = snowFieldSprite;
            Debug.Log("Weather set to Snow.");
        }
    }
}
