using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// Control the object represent the 2D gaze on canvas.
    /// The 2D gaze's origin is current camera's position.
    /// </summary>
    [DisallowMultipleComponent]
    public class Ganzin2DGazeCanvasPointPosCtrl : MonoBehaviour
    {
        private GanzinEyetrackerManager EyetrackerManager;
        private GanzinDisplayProfiler DisplayProfiler;
        private Canvas TargetCanvas;
        // Start is called before the first frame update
        void Start()
        {
            EyetrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyetrackerManager == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
                return;
            }
            DisplayProfiler = FindObjectOfType<GanzinDisplayProfiler>();
            if (DisplayProfiler == null)
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Display Profiler.");
            TargetCanvas = GetComponentInParent<Canvas>();
            if (TargetCanvas == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Canvas in its parent object.");
                return;
            }
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

            Vector2 combineScreenGazePosition = Vector2.zero;
            if (eyetracker != null && eyetracker.GetEyetrackingData(out EventArgs et_data))
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

            gameObject.transform.position = hitPoint;
        }
    }
}