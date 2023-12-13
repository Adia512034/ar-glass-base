#define Collider_Raycast
//#define Physics_Raycast

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// UI Hit detection for 2D gaze.
    /// (Must under the Canvas and has a 3D collider to hit.)
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Ganzin2DGazeHitDetect : MonoBehaviour
    {
        private GanzinEyetrackerManager EyetrackerManager;
        private GanzinDisplayProfiler DisplayProfiler;
        private Canvas TargetCanvas;
        [ReadOnly]
        public bool IsHit = false;
        private Collider ThisCollider3D = null;
        [HideInInspector]
        public RaycastHit HitInfo;
        [HideInInspector]
        public Vector3 GlobalGazeRayOrigin;
        [HideInInspector]
        public Vector3 GlobalGazeRayDirection;
        // Start is called before the first frame update
        void Start()
        {
            EyetrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyetrackerManager == null)
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
            DisplayProfiler = FindObjectOfType<GanzinDisplayProfiler>();
            if (DisplayProfiler == null)
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Display Profiler.");
            TargetCanvas = GetComponentInParent<Canvas>();
            if (TargetCanvas == null)
                Debug.LogError("[AP ][Unity] " + "There is no Canvas in its parent object.");

            ThisCollider3D = GetComponent<Collider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (EyetrackerManager == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
                EyetrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
                return;
            }
            IEyetracker eyetracker = EyetrackerManager.GetEyetracker() as IEyetracker;
            if (eyetracker == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Eye Tracker In Manager.");
                return;
            }
            if (eyetracker.Status != IEyetracker.EyetrackerStatus.ACTIVE)
            {
                Debug.LogWarning("[AP ][Unity] " + "Ganzin Eye Tracker does not work.");
                return;
            }
            if (DisplayProfiler == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Display Profiler.");
                DisplayProfiler = FindObjectOfType<GanzinDisplayProfiler>();
                return;
            }
            if (TargetCanvas == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Canvas in its parent object.");
                return;
            }

            // Check Hit
            if (ThisCollider3D != null)
            {
                Vector2 combineScreenGazePosition = Vector2.zero;
                if (eyetracker.GetEyetrackingData(out EventArgs et_data))
                {
                    var data = et_data as EyetrackingData;
                    if (data != null)
                    {
                        combineScreenGazePosition = data.combined_screen_gaze.position;
                    }
                }

                float hitPointX = combineScreenGazePosition.x * DisplayProfiler.DisplayResolution.x * 0.5f;
                float hitPointY = combineScreenGazePosition.y * DisplayProfiler.DisplayResolution.y * 0.5f;
                Vector3 hitPoint = TargetCanvas.gameObject.transform.TransformPoint(new Vector2(hitPointX, hitPointY));

                GlobalGazeRayOrigin = Camera.main.transform.position;
                GlobalGazeRayDirection = EyetrackerManager.gameObject.transform.TransformDirection(hitPoint - GlobalGazeRayOrigin);

                if (GlobalGazeRayDirection != Vector3.zero)
                {
                    Ray combined_gaze_ray = new Ray(GlobalGazeRayOrigin, GlobalGazeRayDirection);
#if Collider_Raycast
                    IsHit = ThisCollider3D.Raycast(combined_gaze_ray, out HitInfo, Mathf.Infinity);
                    if (IsHit)
                    {
                        //Debug.Log($"[AP ][Unity] Is Hit! Target Name: {this.transform.name}");
                        Debug.DrawRay(GlobalGazeRayOrigin, GlobalGazeRayDirection * HitInfo.distance, Color.red);
                    }
#endif
#if Physics_Raycast
                    IsHit = false;
                    if (Physics.Raycast(combined_gaze_ray, out RaycastHit hitInfo, Mathf.Infinity))
                    {
                        if (GameObject.ReferenceEquals(hitInfo.collider.gameObject, gameObject))
                        {
                            //Debug.Log($"[AP ][Unity] Is Hit! Target Name: {this.transform.name}");
                            IsHit = true;
                            Debug.DrawRay(GlobalGazeRayOrigin, GlobalGazeRayDirection * HitInfo.distance, Color.red);
                        }
                    }
#endif
                }
            }
        }
    }
}