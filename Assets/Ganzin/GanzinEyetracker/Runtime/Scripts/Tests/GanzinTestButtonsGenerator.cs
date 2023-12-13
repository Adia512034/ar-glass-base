using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// Generate UI Buttons for 2D gaze testing.
    /// (Must be under Canvas)
    /// </summary>
    public class GanzinTestButtonsGenerator : MonoBehaviour
    {
        public GameObject Prefab;
        [Tooltip("Short Side Length / Height")]
        public float ButtonSideLengthRatioShort = 0.1f;
        [Tooltip("Long Side Length / Height")]
        public float ButtonSideLengthRatioLong = 0.7f;
        [Tooltip("Square Length / Height")]
        public float ButtonSideLengthRatioSquare = 0.2f;
        [ReadOnly]
        public float ScreenWidth = 200;
        [ReadOnly]
        public float ScreenHeight = 100;
        [ReadOnly]
        public float ButtonSideLengthShort = 10.0f;
        [ReadOnly]
        public float ButtonSideLengthLong = 70.0f;
        [ReadOnly]
        public float ButtonSideLengthSquare = 30.0f;
        private const float MarginSpaceRatio = 0.05f;
        private float MarginSpace = 5.0f;
        private int Number;        
        private Dictionary<int, string> ButtonsName = new Dictionary<int, string>();
        [ReadOnly]
        public Dictionary<string, Vector2> ButtonsPosition = new Dictionary<string, Vector2>();
        // Start is called before the first frame update
        void Start()
        {
            ScreenWidth = gameObject.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.width;
            ScreenHeight = gameObject.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.height;
            float Screen_ShortSide = Mathf.Min(ScreenWidth, ScreenHeight);

            // Get Length
            ButtonSideLengthShort = Screen_ShortSide * ButtonSideLengthRatioShort;
            ButtonSideLengthLong = Screen_ShortSide * ButtonSideLengthRatioLong;
            ButtonSideLengthSquare = Screen_ShortSide * ButtonSideLengthRatioSquare;
            MarginSpace = Screen_ShortSide * MarginSpaceRatio;

            // Get Offset
            float CornerOffsetX = (ScreenWidth / 2.0f) - MarginSpace - (ButtonSideLengthShort / 2.0f);
            float CornerOffsetY = (ScreenHeight / 2.0f) - MarginSpace - (ButtonSideLengthShort / 2.0f);
            float CenterOffsetX = ((ScreenWidth / 2.0f) - MarginSpace - ButtonSideLengthShort) / 2.0f;
            float CenterOffsetY = ((ScreenHeight / 2.0f) - MarginSpace - ButtonSideLengthShort) / 2.0f;

            // Position
            ButtonsName.Add(0, "ButtonsPosition_LSide");
            ButtonsName.Add(1, "ButtonsPosition_LTSide");
            ButtonsName.Add(2, "ButtonsPosition_LBSide");
            ButtonsName.Add(3, "ButtonsPosition_LTCenter");
            ButtonsName.Add(4, "ButtonsPosition_LBCenter");
            ButtonsName.Add(5, "ButtonsPosition_RSide");
            ButtonsName.Add(6, "ButtonsPosition_RTSide");
            ButtonsName.Add(7, "ButtonsPosition_RBSide");
            ButtonsName.Add(8, "ButtonsPosition_RTCenter");
            ButtonsName.Add(9, "ButtonsPosition_RBCenter");
            //ButtonsName.Add(10, "ButtonsPosition_LTCorner");
            //ButtonsName.Add(11, "ButtonsPosition_LBCorner");
            //ButtonsName.Add(12, "ButtonsPosition_RTCorner");
            //ButtonsName.Add(13, "ButtonsPosition_RBCorner");
            Number = ButtonsName.Count;


            ButtonsPosition.Add("ButtonsPosition_LSide", new Vector2(-CornerOffsetX, 0));            
            ButtonsPosition.Add("ButtonsPosition_LTSide", new Vector2(-CenterOffsetX, CornerOffsetY));
            ButtonsPosition.Add("ButtonsPosition_LBSide", new Vector2(-CenterOffsetX, -CornerOffsetY));
            ButtonsPosition.Add("ButtonsPosition_LTCenter", new Vector2(-CenterOffsetX, CenterOffsetY));
            ButtonsPosition.Add("ButtonsPosition_LBCenter", new Vector2(-CenterOffsetX, -CenterOffsetY));            
            ButtonsPosition.Add("ButtonsPosition_RSide", new Vector2(CornerOffsetX, 0));            
            ButtonsPosition.Add("ButtonsPosition_RTSide", new Vector2(CenterOffsetX, CornerOffsetY));
            ButtonsPosition.Add("ButtonsPosition_RBSide", new Vector2(CenterOffsetX, -CornerOffsetY));
            ButtonsPosition.Add("ButtonsPosition_RTCenter", new Vector2(CenterOffsetX, CenterOffsetY));
            ButtonsPosition.Add("ButtonsPosition_RBCenter", new Vector2(CenterOffsetX, -CenterOffsetY));
            ButtonsPosition.Add("ButtonsPosition_LTCorner", new Vector2(-CornerOffsetX, CornerOffsetY));
            ButtonsPosition.Add("ButtonsPosition_LBCorner", new Vector2(-CornerOffsetX, -CornerOffsetY));
            ButtonsPosition.Add("ButtonsPosition_RTCorner", new Vector2(CornerOffsetX, CornerOffsetY));
            ButtonsPosition.Add("ButtonsPosition_RBCorner", new Vector2(CornerOffsetX, -CornerOffsetY));

            // Generate
            GameObject[] testButton = new GameObject[Number];
            for (int i = 0; i < Number; i++)
            {
                testButton[i] = Instantiate(Prefab, transform);
                testButton[i].transform.localScale = Vector3.one;
                testButton[i].GetComponent<RectTransform>().anchoredPosition = ButtonsPosition[ButtonsName[i]];

                if (i == 0 || i == 5)  // Vertical
                {
                    testButton[i].GetComponent<RectTransform>().sizeDelta = new Vector2(ButtonSideLengthShort, ButtonSideLengthLong);
                    testButton[i].GetComponent<BoxCollider>().size = new Vector3(ButtonSideLengthShort + MarginSpace, ButtonSideLengthLong + MarginSpace, 1);
                }
                else if (i == 1 || i == 2 || i == 6 || i == 7)    // Horizontal
                {
                    testButton[i].GetComponent<RectTransform>().sizeDelta = new Vector2(ButtonSideLengthLong, ButtonSideLengthShort);
                    testButton[i].GetComponent<BoxCollider>().size = new Vector3(ButtonSideLengthLong + MarginSpace, ButtonSideLengthShort + MarginSpace, 1);
                }
                else if (i == 3 || i == 4 || i == 8 || i == 9)    // Center
                {
                    testButton[i].GetComponent<RectTransform>().sizeDelta = new Vector2(ButtonSideLengthSquare, ButtonSideLengthSquare);
                    testButton[i].GetComponent<BoxCollider>().size = new Vector3(ButtonSideLengthSquare + MarginSpace, ButtonSideLengthSquare + MarginSpace, 1);
                }
                else if (i == 10 || i == 11 || i == 12 || i == 13)   // Corner
                {
                    testButton[i].GetComponent<RectTransform>().sizeDelta = new Vector2(ButtonSideLengthShort, ButtonSideLengthShort);
                    testButton[i].GetComponent<BoxCollider>().size = new Vector3(ButtonSideLengthShort + MarginSpace, ButtonSideLengthShort + MarginSpace, 1);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}