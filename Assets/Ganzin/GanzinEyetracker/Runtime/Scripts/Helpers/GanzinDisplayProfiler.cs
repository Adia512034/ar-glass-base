using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// (ONLY RUN IN EDITOR MODE.)
    /// For setting automatically.
    /// Camera
    /// 1. Set Main Camera vertical FOV.
    /// 1.1. Set VFOV mean VFOV determine HFOV by dsiplay W/H ratio. Resolution Height Setting will be fixed, and Resolution Width will be variable.
    /// 1.2. Set HFOV mean HFOV determine VFOV by dsiplay H/W ratio. Resolution Width Setting will be fixed, and Resolution Height will be variable.
    /// Canvas
    /// 1. Set Display Resolution or Fixed Canvas width & height
    /// 2. Distance to Canvas
    /// 3. Scale = Meter / Pixel
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class GanzinDisplayProfiler : MonoBehaviour
    {
        public enum FovDirection { Vertical
#if UNITY_2019_1_OR_NEWER
                , Horizontal 
#endif
        }
        [Header("Main Camera")]
        public FovDirection FOVAxis = FovDirection.Vertical;
        [Tooltip("The field of view based on FOV Axis")]
        public float FOV = 36.0f;
        private float VFOV { get {
#if UNITY_2019_1_OR_NEWER
                if (FOVAxis == FovDirection.Horizontal)
                {
                    float aspect_ratio = Camera.main.aspect;
                    return Camera.HorizontalToVerticalFieldOfView(FOV, aspect_ratio);
                }
                else
                    return FOV;
#else
                return FOV;
#endif
            } }

        [Header("Display")]
        public Vector2 DisplayResolution = new Vector2(1920, 1080);
        [Header("Canvas")]
        public Vector2 Resolution = new Vector2(1920, 1080);
        public float Depth = 0.5f;
        public float Scale = 0.0003f;
        public Canvas[] CanvasList;
        // Start is called before the first frame update
        private void Awake()
        {
            ApplyDisplayParameters();
        }
        void Start()
        {
            ApplyDisplayParameters();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                ApplyDisplayParameters();
            }
#endif
        }

        /// <summary>
        /// Apply the setting to each canvas in the list.
        /// </summary>
        private void ApplyDisplayParameters()
        {            
            // Already auto calculate VFOV for device display
            Camera.main.fieldOfView = VFOV;

            if (FOVAxis == FovDirection.Vertical)
            {
                Resolution.x = Resolution.y * Camera.main.aspect;
            }
            else        // Horizontal
            {
                Resolution.y = Resolution.x / Camera.main.aspect;
            }

            if (CanvasList != null)
            {
                foreach (var canvas in CanvasList)
                {
                    RectTransform rectTransform = canvas.gameObject.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = Resolution;
                    rectTransform.localPosition = new Vector3(0, 0, Depth);
                    rectTransform.localScale = new Vector3(Scale, Scale, Scale);
                }
            }
        }
    }
}