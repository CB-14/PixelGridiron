using UnityEngine;

public class ChaseManager : MonoBehaviour
{
    public DefenderAI[] defenders;          // Assign in Inspector or auto-detect
    public Transform quarterback;           // Drag QB here
    public float lineOfScrimmageX;          // Set this to the LOS X value in Inspector

    private bool hasTriggeredChase = false;

    void Start()
    {
        // Auto-detect defenders if none assigned
        if (defenders == null || defenders.Length == 0)
        {
            defenders = FindObjectsOfType<DefenderAI>();
        }

        if (quarterback == null)
        {
            GameObject qbObject = GameObject.FindGameObjectWithTag("QB");
            if (qbObject != null)
            {
                quarterback = qbObject.transform;
            }
        }
    }

    void Update()
    {
        if (!hasTriggeredChase && SnapManager.playStarted && quarterback != null)
        {
            // Trigger chase when QB crosses the LOS (going right)
            if (quarterback.position.x > lineOfScrimmageX)
            {
                foreach (var defender in defenders)
                {
                    defender.StartChasing();
                }

                hasTriggeredChase = true;
                Debug.Log("QB crossed LOS – defenders chasing!");
            }
        }
    }
}
