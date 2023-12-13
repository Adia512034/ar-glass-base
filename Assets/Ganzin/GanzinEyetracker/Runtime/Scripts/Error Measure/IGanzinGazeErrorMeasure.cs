using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// The interface for GanzinErrorMeasure to let scripts access.
    /// </summary>
    public class IGanzinGazeErrorMeasure : MonoBehaviour
    {
        public Text ErrorText = null;
        public int ErrorSampleTimes = 150;
        protected int ErrorSampleCount = 0;
        protected float AccumulatedError = 0.0f;
        protected float UpdatedError = 0.0f;
    }
}