using UnityEngine;
using UnityEditor;
using CamOrbit;

[CustomEditor(typeof(CameraOrbit))]
public class CameraOrbitEditor : Editor
{
    CameraOrbit cameraOrbit;
    Camera camera;
    GameObject targetObject;

    float radius;
    float radiusMinArea;

    private void OnEnable()
    {
        if (cameraOrbit == null)
        {
            cameraOrbit = (CameraOrbit)target;
            cameraOrbit.transform.localScale = Vector3.one * cameraOrbit.radius;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        radius = EditorGUILayout.FloatField("Radius", cameraOrbit.radius);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cameraOrbit, "Change Radius");
            cameraOrbit.radius = radius;
            if (cameraOrbit.radiusMinArea > radius) cameraOrbit.radiusMinArea = radius;
            cameraOrbit.transform.localScale = Vector3.one * radius;
            cameraOrbit.AdjustCamera();
            SceneView.RepaintAll();
        }

        EditorGUI.BeginChangeCheck();
        radiusMinArea = EditorGUILayout.Slider("Min Area Radius", cameraOrbit.radiusMinArea, 0, cameraOrbit.radius);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cameraOrbit, "Change Min Area Radius");
            cameraOrbit.radiusMinArea = radiusMinArea;
            SceneView.RepaintAll();
        }

        if (camera != cameraOrbit.cam)
        {
            if (cameraOrbit.cam != null)
            {
                cameraOrbit.AdjustCamera();
                camera = cameraOrbit.cam;
            }
        }

        if (targetObject != cameraOrbit.targetObject)
        {
            if (cameraOrbit.targetObject != null)
            {
                cameraOrbit.MoveToTarget();
                targetObject = cameraOrbit.targetObject;
            }
        }
    }

    private void OnSceneGUI()
    {
        cameraOrbit.LookToTarget();
        Draw();
    }

    void Draw()
    {
        Handles.color = new Color(0, 1, 0, 0.1f);
        Handles.SphereHandleCap(0, cameraOrbit.transform.position, cameraOrbit.transform.rotation, cameraOrbit.radius * 2, EventType.Repaint);
        Handles.color = new Color(1, 0, 0, 0.2f);
        Handles.SphereHandleCap(0, cameraOrbit.transform.position, cameraOrbit.transform.rotation, cameraOrbit.radiusMinArea * 2, EventType.Repaint);
    }

}