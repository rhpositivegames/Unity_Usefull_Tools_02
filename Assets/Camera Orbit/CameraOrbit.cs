using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamOrbit
{
    public class CameraOrbit : MonoBehaviour
    {
        public Camera cam;
        public GameObject targetObject;
        public float rotateSpeed = 8f;
        public float moveSpeed = 40;

        [HideInInspector] public float radius = 3f;
        [HideInInspector] public float radiusMinArea = 1.5f;

        Vector3 targetScale;
        float scaleFactor;

        private void Start()
        {
            targetScale = transform.localScale;

            StartCoroutine(CameraScale());
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                float h = rotateSpeed * Input.GetAxis("Mouse X");
                float v = rotateSpeed * Input.GetAxis("Mouse Y");

                if (transform.eulerAngles.z + v <= 0.1f || transform.eulerAngles.z + v >= 179.9f)
                    v = 0;

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + h, transform.eulerAngles.z + v);
            }

            float scrollFactor = Input.GetAxis("Mouse ScrollWheel");

            if (scrollFactor != 0)
            {
                scaleFactor = transform.localScale.x * (1f - scrollFactor);
                scaleFactor = Mathf.Clamp(scaleFactor, radiusMinArea, Mathf.Infinity);
                targetScale = Vector3.one * scaleFactor;
            }

            LookToTarget();
        }

        IEnumerator CameraScale()
        {
            while (true)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, Time.deltaTime * moveSpeed);
                yield return null;
            }
        }

        public void AdjustCamera()
        {
            if (cam == null)
                return;

            cam.transform.position = transform.position + transform.up * radius;
            cam.transform.SetParent(transform);
        }

        public void MoveToTarget()
        {
            if (targetObject == null)
                return;

            transform.position = targetObject.transform.position;
        }

        public void LookToTarget()
        {
            if (cam == null || targetObject == null)
                return;

            cam.transform.rotation = Quaternion.Euler(90, 0, -90);
            cam.transform.LookAt(targetObject.transform.position);
        }
    }
}
