using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// Must be under Canvas
    /// 
    /// Functions of Keys:
    ///     p: Show Pupile Display
    /// </summary>
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class GanzinPupilDisplaysController : MonoBehaviour
    {
        public KeyCode ShowHotKey = KeyCode.P;
        public bool AutoLayout = true;
        [ReadOnly]
        public float ScreenWidth = 200;
        [ReadOnly]
        public float ScreenHeight = 100;
        private GanzinSinglePupilDisplayController[] PupilDisplayCtrls;

        [Header("Config of AutoLayout")]
        [Tooltip("The pupil display area's width (ratio to screen short side length).")]
        public float PupilDisplayAreaWidth = 1.0f;
        [Tooltip("The pupil display area's height (ratio to screen short side length).")]
        public float PupilDisplayAreaHeight = 0.5f;
        [Tooltip("The space between pupil displays (ratio to screen short side length).")]
        public float PupilDisplaySpace = 0.1f;
        // Start is called before the first frame update
        void Start()
        {
            ScreenWidth = gameObject.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.width;
            ScreenHeight = gameObject.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.height;
            if (AutoLayout)
            {
                float Screen_ShortSide = Mathf.Min(ScreenWidth, ScreenHeight);
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen_ShortSide * PupilDisplayAreaWidth, Screen_ShortSide * PupilDisplayAreaHeight);
                gameObject.GetComponent<HorizontalLayoutGroup>().spacing = Screen_ShortSide * PupilDisplaySpace;
            }

            PupilDisplayCtrls = Resources.FindObjectsOfTypeAll<GanzinSinglePupilDisplayController>();
        }

        // Update is called once per frame
        void Update()
        {
            // User Interaction
            if (ShowHotKey != KeyCode.None)
            {
                if (Input.GetKeyDown(ShowHotKey))
                {
                    foreach (var display in PupilDisplayCtrls)
                        display.gameObject.SetActive(!display.gameObject.activeInHierarchy);
                }
            }
        }
    }
}