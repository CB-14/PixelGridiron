using UnityEngine;

public class SimpleMoveTest : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime;
    }
}
