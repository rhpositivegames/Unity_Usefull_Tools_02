using UnityEngine;
using UnityEditor;
using MobilePanAndZoom;

[CustomEditor(typeof(InputControl))]
public class InputControlEditor : Editor
{
    InputControl ic;
    Camera targetCamera;

    float maxOrthographicSize;
    int maxSingleZoomDistance;
    int maxZoomForward;
    int maxZoomBack;
    float fowZoomSpeed;
    float panSpeed;
    int minPixelForPan;
    bool alwaysDrawGizmos;

    ZoomType zoomType;

    void OnEnable()
    {
        ic = (InputControl)target;

        if (ic.PAZ == null)
        {
            ic.PAZ = new PanAndZoom();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (ic.PAZ.isZoomEnable || ic.PAZ.isPanEnable)
        {
            EditorGUI.BeginChangeCheck();
            targetCamera = (Camera)EditorGUILayout.ObjectField("Target Camera", ic.PAZ.targetCamera, typeof(Camera), true);
            if (EditorGUI.EndChangeCheck())
            {
                if (ic.PAZ.targetCamera != null)
                {
                    Undo.RecordObject(ic.PAZ.targetCamera.transform, "Add Camera");
                }
                else
                {
                    Undo.RecordObject(ic, "Add Camera");
                }
                ic.PAZ.targetCamera = targetCamera;
                ic.PAZ.SetCamera();
                SceneView.RepaintAll(); EditorUtility.SetDirty(ic);
            }
        }

        if (ic != null)
        {
            GUILayout.Space(10);

            ic.PAZ.isZoomEnable = EditorGUILayout.Toggle("Zoom", ic.PAZ.isZoomEnable);
            if (ic.PAZ.isZoomEnable)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUI.BeginChangeCheck();
                zoomType = (ZoomType)EditorGUILayout.EnumPopup("Zoom Type", ic.PAZ.zoomType);
                if (EditorGUI.EndChangeCheck())
                {
                    ic.PAZ.zoomType = zoomType;
                    ic.PAZ.SetCamera();
                    SceneView.RepaintAll();
                }
                GUILayout.Space(5);

                if (zoomType == ZoomType.zoom3DPhysical)
                {
                    EditorGUI.BeginChangeCheck();
                    maxSingleZoomDistance = EditorGUILayout.IntSlider("Max Single Zoom Distance", ic.PAZ.maxSingleZoomDistance, Clamp.SINGLE_ZOOM_DISTANCE_MIN, Clamp.SINGLE_ZOOM_DISTANCE_MAX);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(ic, "Zoom Panel Distance");
                        ic.PAZ.maxSingleZoomDistance = maxSingleZoomDistance;
                        SceneView.RepaintAll();
                    }
                    EditorGUI.BeginChangeCheck();
                    maxZoomForward = EditorGUILayout.IntSlider("Max Zoom Forward", ic.PAZ.maxZoomForwardDistance, Clamp.MAX_ZOOM_FORWARD_MIN, Clamp.MAX_ZOOM_FORWARD_MAX);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(ic, "Max Zoom Forward");
                        ic.PAZ.maxZoomForwardDistance = maxZoomForward;
                        SceneView.RepaintAll();
                    }
                    EditorGUI.BeginChangeCheck();
                    maxZoomBack = EditorGUILayout.IntSlider("Max Zoom Back", ic.PAZ.maxZoomBackDistance, Clamp.MAX_ZOOM_BACK_MIN, Clamp.MAX_ZOOM_BACK_MAX);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(ic, "Max Zoom Back");
                        ic.PAZ.maxZoomBackDistance = maxZoomBack;
                        SceneView.RepaintAll();
                    }

                    GUILayout.Space(5);

                    EditorGUI.BeginChangeCheck();
                    alwaysDrawGizmos = EditorGUILayout.Toggle("Always Draw Gizmos", ic.alwaysDrawGizmos);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(ic, "Always Draw Gizmos");
                        ic.alwaysDrawGizmos = alwaysDrawGizmos;
                        SceneView.RepaintAll();
                    }
                }
                else if (zoomType == ZoomType.zoom3DFieldOfView)
                {
                    EditorGUI.BeginChangeCheck();
                    fowZoomSpeed = EditorGUILayout.Slider("Zoom Speed", ic.PAZ.fowZoomSpeed, Clamp.FOW_ZOOM_SPEED_MIN, Clamp.FOW_ZOOM_SPEED_MAX);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(ic, "Fow Zoom Speed");
                        ic.PAZ.fowZoomSpeed = fowZoomSpeed;
                    }
                }
                else if (zoomType == ZoomType.zoom2D)
                {
                    EditorGUI.BeginChangeCheck();
                    maxOrthographicSize = EditorGUILayout.Slider("Max Orthographic Size", ic.PAZ.maxOrthographicSize, ic.PAZ.targetCamera.orthographicSize, Clamp.MAX_ORTHOGRAPHIC_SIZE);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(ic, "Max Orthographic Size");
                        ic.PAZ.maxOrthographicSize = maxOrthographicSize;
                    }
                }

                EditorGUILayout.EndVertical();

                SceneView.RepaintAll();
                GUILayout.Space(10);
            }

            ic.PAZ.isPanEnable = EditorGUILayout.Toggle("Pan", ic.PAZ.isPanEnable);
            if (ic.PAZ.isPanEnable)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUI.BeginChangeCheck();
                panSpeed = EditorGUILayout.Slider("Pan Speed", ic.PAZ.panSpeed, Clamp.PAN_SPEED_MIN, Clamp.PAN_SPEED_MAX);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(ic, "Pan Speed");
                    ic.PAZ.panSpeed = panSpeed;
                    SceneView.RepaintAll();
                }
                EditorGUI.BeginChangeCheck();
                minPixelForPan = EditorGUILayout.IntSlider("Min Pixel For Pan", ic.PAZ.minPixelForPan, Clamp.MIN_PIXEL_FOR_PAN_MIN, Clamp.MIN_PIXEL_FOR_PAN_MAX);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(ic, "Min Pixel For Pan");
                    ic.PAZ.minPixelForPan = minPixelForPan;
                    SceneView.RepaintAll();
                }
                EditorGUILayout.EndVertical();
                SceneView.RepaintAll();


            }
        }

        EditorUtility.SetDirty(ic);
    }
}
