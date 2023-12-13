using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// Detect selection on UI using 2D gaze.
    /// (Must has a Ganzin2DGazeHitDetect.)
    /// </summary>
    [RequireComponent(typeof(Ganzin2DGazeHitDetect))]
    [RequireComponent(typeof(Button))]
    public class GanzinButton2DGazeSelect : MonoBehaviour
    {
        private Ganzin2DGazeHitDetect GazeHitDetector;
        private Button ThisButton;
        // Start is called before the first frame update
        void Start()
        {
            GazeHitDetector = GetComponent<Ganzin2DGazeHitDetect>();
            if (GazeHitDetector == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin 2D Gaze Hit Detect.");
                return;
            }
            ThisButton = GetComponent<Button>();
            if (ThisButton == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Button.");
                return;
            }
        }

        // Update is called once per frame
        void Update()
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (GazeHitDetector.IsHit)
            {
                //ThisButton.Select();
                StartCoroutine(SelectButtonLater());
            }
        }

        // Delay one frame to fix unhighlight bug in Unity 2019.4.
        IEnumerator SelectButtonLater()
        {
            yield return null;
            ThisButton.Select();
        }
    }
}