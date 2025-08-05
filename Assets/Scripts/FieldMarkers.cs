using UnityEngine;

public class FieldMarkers : MonoBehaviour
{
    public Transform lineOfScrimmageMarker;
    public Transform firstDownMarker;
    public Transform ball;

    // Unity units that represent 100 yards (excluding end zones)
    public float yardsToUnits = 0.05598444f;

    [Header("Touchdown Settings")]
    public float fieldLengthInYards = 100f;
    public bool playStopped = false;

    private float currentLOSYard;
    private float currentFirstDownYard;

    void Start()
    {
        if (ball == null)
        {
            Debug.LogWarning("Ball transform is not assigned!");
            return;
        }

        currentLOSYard = ConvertPositionToYards(ball.position.x);
        currentFirstDownYard = currentLOSYard + 10f;

        UpdateMarkers();
    }

    void Update()
    {
        if (!playStopped && SnapManager.playStarted)
        {
            float ballYard = ConvertPositionToYards(ball.position.x);

            if (ballYard >= fieldLengthInYards)
            {
                HandleTouchdown();
            }
        }
    }

    void UpdateMarkers()
    {
        Vector3 losPos = new Vector3(currentLOSYard * yardsToUnits, lineOfScrimmageMarker.position.y, lineOfScrimmageMarker.position.z);
        Vector3 firstDownPos = new Vector3(currentFirstDownYard * yardsToUnits, firstDownMarker.position.y, firstDownMarker.position.z);

        if (lineOfScrimmageMarker != null)
            lineOfScrimmageMarker.position = losPos;

        if (firstDownMarker != null)
            firstDownMarker.position = firstDownPos;
    }

    float ConvertPositionToYards(float xPos)
    {
        return xPos / yardsToUnits;
    }

    public void AdvanceLineOfScrimmage(float newLOSInYards)
    {
        currentLOSYard = newLOSInYards;
        currentFirstDownYard = currentLOSYard + 10f;
        UpdateMarkers();
    }

    public bool CheckIfFirstDown(float ballXPos)
    {
        float ballYard = ConvertPositionToYards(ballXPos);
        return ballYard >= currentFirstDownYard;
    }

    void HandleTouchdown()
    {
        playStopped = true;
        SnapManager.playStarted = false;

        Debug.Log("TOUCHDOWN!");

        // 🏈 Call your touchdown graphic/UI logic here
        // Example: TouchdownUIManager.ShowTouchdown();

        // Optional: Freeze all movement or notify other managers
    }
}
