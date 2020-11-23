using UnityEngine;

namespace MobilePanAndZoom
{
    public class InputControl : MonoBehaviour
    {
        [HideInInspector] public PanAndZoom PAZ;
        [HideInInspector] public bool alwaysDrawGizmos = true;

        int touchCount;
        Touch touch0;
        Touch touch1;

        void Start()
        {
            PAZ.SetCamera();
        }

        void Update()
        {
            touchCount = Input.touchCount;

            if (touchCount == 2)
            {
                touch0 = Input.GetTouch(0);
                touch1 = Input.GetTouch(1);
                PAZ.Zoom(touch0, touch1);
            }

            if (touchCount == 1 && PAZ.IsPinchZoom == false)
            {
                touch0 = Input.GetTouch(0);
                PAZ.Pan(touch0);
            }

            if (touchCount == 1 && touch0.phase == TouchPhase.Ended)
            {
                if (!PAZ.IsPan && !PAZ.IsPinchZoom)
                {
                    Touch();
                }
                PAZ.IsTouched = false;
                PAZ.IsPan = false;
            }

            if (touchCount == 2 && (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended))
            {
                PAZ.Zoom3DEnd();
            }
        }

        void Touch()
        {

        }

        //============================================= DRAW_GIZMOS =============================================

        void OnDrawGizmos()
        {
            if (alwaysDrawGizmos)
                Draw();
        }

        void OnDrawGizmosSelected()
        {
            if (alwaysDrawGizmos == false)
                Draw();
        }

        void Draw()
        {
            if (PAZ == null || PAZ.isZoomEnable == false || PAZ.zoomType != ZoomType.zoom3DPhysical)
                return;

            if (PAZ.targetCamera != null)
            {
                if (PAZ.targetCamera.orthographic == false)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.2f);
                    Vector3 panelCenter = PAZ.targetCamera.transform.position + PAZ.targetCamera.transform.forward * PAZ.maxSingleZoomDistance;
                    DrawCube(panelCenter, PAZ.targetCamera.transform.rotation, new Vector3(1 * PAZ.maxSingleZoomDistance, 1 * PAZ.maxSingleZoomDistance, .1f));
                    Vector3[] corners = new Vector3[4];
                    corners[0] = panelCenter + (PAZ.targetCamera.transform.right + PAZ.targetCamera.transform.up) * PAZ.maxSingleZoomDistance / 2;
                    corners[1] = panelCenter - (PAZ.targetCamera.transform.right + PAZ.targetCamera.transform.up) * PAZ.maxSingleZoomDistance / 2;
                    corners[2] = panelCenter + (PAZ.targetCamera.transform.right - PAZ.targetCamera.transform.up) * PAZ.maxSingleZoomDistance / 2;
                    corners[3] = panelCenter - (PAZ.targetCamera.transform.right - PAZ.targetCamera.transform.up) * PAZ.maxSingleZoomDistance / 2;
                    Gizmos.color = new Color(1, 0, 0, .6f);
                    for (int i = 0; i < corners.Length; i++)
                    {
                        Gizmos.DrawLine(PAZ.targetCamera.transform.position, corners[i]);
                    }

                    Gizmos.color = new Color(0, 1, 0, .6f);
                    Vector3 maxPoint = PAZ.targetCamera.transform.position + PAZ.targetCamera.transform.forward * PAZ.maxZoomForwardDistance;
                    DrawCube(maxPoint, PAZ.targetCamera.transform.rotation, new Vector3(1f, 1f, 1f));
                    Gizmos.DrawLine(PAZ.targetCamera.transform.position, maxPoint);
                    maxPoint = PAZ.targetCamera.transform.position - PAZ.targetCamera.transform.forward * PAZ.maxZoomBackDistance;
                    DrawCube(maxPoint, PAZ.targetCamera.transform.rotation, new Vector3(1f, 1f, 1f));
                    Gizmos.DrawLine(PAZ.targetCamera.transform.position, maxPoint);
                }
            }
        }

        void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

            Gizmos.matrix *= cubeTransform;

            Gizmos.DrawCube(Vector3.zero, Vector3.one);

            Gizmos.matrix = oldGizmosMatrix;
        }
    }
}
