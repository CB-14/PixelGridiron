using UnityEngine;

public class FieldLineFollower : MonoBehaviour
{
    [Header("Marker References")]
    public Transform lineOfScrimmageMarker;
    public Transform firstDownMarker;

    [Header("Line References")]
    public Transform losLine;           // The visual line for LOS
    public Transform firstDownLine;     // The visual line for first down

    void Update()
    {
        if (lineOfScrimmageMarker != null && losLine != null)
        {
            losLine.position = new Vector3(lineOfScrimmageMarker.position.x, losLine.position.y, losLine.position.z);
        }

        if (firstDownMarker != null && firstDownLine != null)
        {
            firstDownLine.position = new Vector3(firstDownMarker.position.x, firstDownLine.position.y, firstDownLine.position.z);
        }
    }
}
