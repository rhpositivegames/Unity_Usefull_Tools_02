using UnityEngine;

namespace MobilePanAndZoom
{
    [System.Serializable]
    public class PanAndZoom
    {
        const int PAN_SPEED_FIX = 100;

        public Camera targetCamera;
        public ZoomType zoomType;
        public GameObject zoomCenter;

        public bool isZoomEnable = true;
        public bool isPanEnable = true;

        // Zoom 2D
        public float maxOrthographicSize = 30;

        // Zoom 3D Physical
        public int maxSingleZoomDistance = 50;
        public int maxZoomForwardDistance = 200;
        public int maxZoomBackDistance = 200;

        // Zoom 3D FOW
        public float fowZoomSpeed = 100;

        // Pan
        public float panSpeed = 2f;
        public int minPixelForPan = 100;

        public float SCREEN_ASPECT { get; set; }
        public bool IsPinchZoom { get; set; }
        public bool IsPan { get; set; }
        public bool IsTouched { get; set; }

        Vector2 touchStartPos;
        Vector3 startCamPos;
        float startDistance;
        float startFOW;
        float startOrthographicSize;

        float zoomPos = 0;
        float zoomAmount = 0;

        public void SetCamera()
        {
            if (!isPanEnable && !isZoomEnable)
                return;

            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            SCREEN_ASPECT = Screen.width * Screen.width + Screen.height * Screen.height;
            targetCamera.orthographic = zoomType == ZoomType.zoom2D ? true : false;
        }

        public void Zoom(Touch touch0, Touch touch1)
        {
            if (isZoomEnable == false)
                return;

            IsPan = false;

            switch (zoomType)
            {
                case ZoomType.zoom2D:
                    Zoom2D(touch0, touch1);
                    break;
                case ZoomType.zoom3DFieldOfView:
                    Zoom3DFOW(touch0, touch1);
                    break;
                case ZoomType.zoom3DPhysical:
                    Zoom3DPhysical(touch0, touch1);
                    break;
                default:
                    break;
            }
        }

        void Zoom2D(Touch touch0, Touch touch1)
        {
            if (IsPinchZoom == false)
            {
                startDistance = (touch0.position - touch1.position).sqrMagnitude;
                startOrthographicSize = targetCamera.orthographicSize;
                IsPinchZoom = true;
            }

            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                float distance = (touch0.position - touch1.position).sqrMagnitude;
                float ortSize = startOrthographicSize / (distance / startDistance);
                ortSize = Mathf.Clamp(ortSize, 0, maxOrthographicSize);
                targetCamera.orthographicSize = ortSize;
            }
        }

        void Zoom3DFOW(Touch touch0, Touch touch1)
        {
            if (IsPinchZoom == false)
            {
                startDistance = (touch0.position - touch1.position).sqrMagnitude;
                startFOW = targetCamera.fieldOfView;
                IsPinchZoom = true;
            }

            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                float distance = (touch0.position - touch1.position).sqrMagnitude;
                targetCamera.fieldOfView = startFOW - ((distance - startDistance) / SCREEN_ASPECT) * fowZoomSpeed;
            }
        }

        void Zoom3DPhysical(Touch touch0, Touch touch1)
        {
            if (IsPinchZoom == false)
            {
                startCamPos = targetCamera.transform.position;
                startDistance = (touch0.position - touch1.position).sqrMagnitude;
                IsPinchZoom = true;
            }

            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                float distance = (touch0.position - touch1.position).sqrMagnitude;
                zoomAmount = ((distance - startDistance) / SCREEN_ASPECT) * maxSingleZoomDistance;
                if (zoomPos + zoomAmount < maxZoomForwardDistance && zoomPos + zoomAmount > -maxZoomBackDistance)
                {
                    targetCamera.transform.position = startCamPos + targetCamera.transform.forward * zoomAmount;
                }
            }
        }

        public void Zoom3DEnd()
        {
            if (isZoomEnable == false)
                return;

            IsPinchZoom = false;
            zoomPos += zoomAmount;
            zoomPos = Mathf.Clamp(zoomPos, -maxZoomBackDistance, maxZoomForwardDistance);
        }

        public void Pan(Touch touch0)
        {
            if (isPanEnable == false)
                return;

            touch0 = Input.GetTouch(0);

            if (touch0.phase == TouchPhase.Moved)
            {
                if (IsTouched == false)
                {
                    touchStartPos = touch0.position;
                    IsTouched = true;
                }

                if (IsPan == false)
                {
                    if ((touch0.position - touchStartPos).sqrMagnitude > minPixelForPan)
                    {
                        IsPan = true;
                    }
                }
                else
                {
                    targetCamera.transform.position -= (Vector3)touch0.deltaPosition * panSpeed / PAN_SPEED_FIX;
                }
            }
        }
    }

    [SerializeField]
    public enum ZoomType
    {
        zoom2D,
        zoom3DPhysical,
        zoom3DFieldOfView
    }

    public static class Clamp
    {
        public static int SINGLE_ZOOM_DISTANCE_MIN = 10;
        public static int SINGLE_ZOOM_DISTANCE_MAX = 200;
        public static int MAX_ZOOM_FORWARD_MIN = 1;
        public static int MAX_ZOOM_FORWARD_MAX = 1000;
        public static int MAX_ZOOM_BACK_MIN = 1;
        public static int MAX_ZOOM_BACK_MAX = 1000;
        public static float FOW_ZOOM_SPEED_MIN = 1;
        public static float FOW_ZOOM_SPEED_MAX = 180;
        public static int MAX_ORTHOGRAPHIC_SIZE = 100;
        public static float PAN_SPEED_MIN = 1f;
        public static float PAN_SPEED_MAX = 5f;
        public static int MIN_PIXEL_FOR_PAN_MIN = 10;
        public static int MIN_PIXEL_FOR_PAN_MAX = 200;
    }
}
