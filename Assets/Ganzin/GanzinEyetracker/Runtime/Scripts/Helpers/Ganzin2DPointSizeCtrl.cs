using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// Size controller for the object that represent gaze on canvas.
    /// (Must be under Canvas.)
    /// </summary>
    [DisallowMultipleComponent]
    public class Ganzin2DPointSizeCtrl : MonoBehaviour
    {
        private GanzinEyetrackerManager EyetrackerManager;
        public float RadiusDegree = 1.5f;
        // Start is called before the first frame update
        void Start()
        {
            EyetrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyetrackerManager == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
                return;
            }
            // Change circle radius to a fixed degree range
            float radiusInPixel = Ganzin2DUtility.GetLocalDistance_ByFOV(EyetrackerManager.gameObject, gameObject, RadiusDegree);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(radiusInPixel * 2.0f, radiusInPixel * 2.0f);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
