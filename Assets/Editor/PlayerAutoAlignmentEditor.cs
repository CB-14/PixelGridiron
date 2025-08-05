#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerAutoAlignment))]
public class PlayerAutoAlignmentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerAutoAlignment tool = (PlayerAutoAlignment)target;
        if (GUILayout.Button("Capture Offsets from Scene"))
        {
            tool.CaptureOffsets();
        }
    }
}
#endif
