using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Ganzin.Eyetracker.Unity;
using System.Runtime.InteropServices;

public class Ganzin2DScreenPosCtrl : MonoBehaviour
{
    private GanzinEyetrackerManager EyetrackerManager;
    private GanzinDisplayProfiler DisplayProfiler;
    private Canvas TargetCanvas;

    public static bool isEyetrackerActive = false;

    public Text textX, textY;
    // public GameObject circlePrefab;
    public Image      image;

    void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        EyetrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
        DisplayProfiler = FindObjectOfType<GanzinDisplayProfiler>();
        TargetCanvas = GetComponentInParent<Canvas>();
    }


    // Update is called once per frame
    void Update()
    {
        IEyetracker eyetracker = EyetrackerManager.GetEyetracker() as IEyetracker;
        if (eyetracker.Status != IEyetracker.EyetrackerStatus.ACTIVE)
        {
            Debug.Log("IEyetracker test open fail");
            return;
        }
        else
        {
            isEyetrackerActive = true;
        }

        Vector2 position = Vector2.zero;
        if (eyetracker != null && eyetracker.GetEyetrackingData(out EventArgs et_data))
        {
            var data = et_data as EyetrackingData;
            if (data != null)
            {
                // You can get eyetracking data here
                // Take Screen gaze position for example:
                position = data.combined_screen_gaze.position;
                // float hitPointX = position.x * DisplayProfiler.DisplayResolution.x * 0.5f;
                // float hitPointY = position.y * DisplayProfiler.DisplayResolution.y * 0.5f;
                textX.text = "X :"+position.x + ", " + position.x * 0.5f * 1920 + 960;
                textY.text = "Y :"+position.y + ", " + position.x * 0.5f * 1080 + 540;
                this.image.gameObject.GetComponent<RectTransform>().position = new Vector2(position.x * 0.5f * 1920 + 960,
                                                                                           position.y * 0.5f * 1080 + 540);
                this.gameObject.transform.position = new Vector2(   position.x * 0.5f * 1920 + 960,
                                                                    position.y * 0.5f * 1080 + 540);

                // Debug.Log("Hit Point X: " + hitPointX + ", Hit Point Y: " + hitPointY);
                // Vector3 hitPoint = TargetCanvas.gameObject.transform.TransformPoint(new Vector2(hitPointX, hitPointY));
                // Vector3 hitPoint = TargetCanvas.gameObject.transform.TransformPoint(position);
        
                // this.gameObject.transform.position = hitPoint;
            }
            else
            {
                Debug.Log("get eye position fail ");
            }
        }
        Debug.Log("Hit position: " + position);
    }
}
