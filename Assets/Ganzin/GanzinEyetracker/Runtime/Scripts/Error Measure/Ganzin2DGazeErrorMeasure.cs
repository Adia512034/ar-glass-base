using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// Attach this component at any GameObject you want to measure error.
    /// (Must has a Ganzin2DGazeHitDetect.)
    /// </summary>
    [RequireComponent(typeof(Ganzin2DGazeHitDetect))]
    public class Ganzin2DGazeErrorMeasure : IGanzinGazeErrorMeasure
    {
        private Ganzin2DGazeHitDetect GazeHitDetector;
        // Start is called before the first frame update
        void Start()
        {
            GazeHitDetector = GetComponent<Ganzin2DGazeHitDetect>();
            if (GazeHitDetector == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Gaze Hit Detect.");
                return;
            }

            if (ErrorText != null)
                ErrorText.gameObject.SetActive(false);
        }
        void OnDisable()
        {
            if (ErrorText != null)
                ErrorText.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            // Check Hit
            if (GazeHitDetector.IsHit && GazeHitDetector.HitInfo.collider != null)
            {
                Vector3 globalGroundTruthDirection = GazeHitDetector.HitInfo.collider.gameObject.transform.position - GazeHitDetector.GlobalGazeRayOrigin;
                float CurrentError = Vector3.Angle(globalGroundTruthDirection, GazeHitDetector.GlobalGazeRayDirection);

                AccumulatedError += CurrentError;
                ErrorSampleCount++;

                if (ErrorSampleCount >= ErrorSampleTimes)
                {
                    UpdatedError = AccumulatedError / ErrorSampleCount;
                    AccumulatedError = 0.0f;
                    ErrorSampleCount = 0;

                    ErrorText.gameObject.SetActive(true);
                    ErrorText.text = UpdatedError.ToString("F3") + " Deg.";
                }
            }
        }
    }
}