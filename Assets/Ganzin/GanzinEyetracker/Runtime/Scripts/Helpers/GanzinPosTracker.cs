using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// Make the gameobject track to the target object.
    /// </summary>
    [DisallowMultipleComponent]
    public class GanzinPosTracker : MonoBehaviour
    {
        public GameObject trackedObject = null;
        public enum TrackingMode { Rotation, Position, RotationAndPosition }
        public TrackingMode trackingMode = TrackingMode.RotationAndPosition;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Check
            if (trackedObject == null)
            {
                Debug.LogError("[AP ][Unity] " + "Cannot find Tracked GameObject.");
                return;
            }
            if (trackingMode == TrackingMode.RotationAndPosition)
            {
                gameObject.transform.position = trackedObject.transform.position;
                gameObject.transform.rotation = trackedObject.transform.rotation;
            }
            else if (trackingMode == TrackingMode.Rotation)
            {
                gameObject.transform.rotation = trackedObject.transform.rotation;
            }
            else if (trackingMode == TrackingMode.Position)
            {
                gameObject.transform.position = trackedObject.transform.position;
            }
        }
    }
}