using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.Eyetracker.Unity
{
    /// <summary>
    /// Utilities for 2D coordinate system transformation.
    /// </summary>
    public static class Ganzin2DUtility
    {
        public static float GetGlobalDisplacement_Obj3D_Obj2D(GameObject Obj3D, GameObject Obj2D)
        {
            Plane obj2DLocatedPlane = new Plane(Obj2D.transform.forward, Obj2D.transform.position);
            float displacement = Mathf.Abs(obj2DLocatedPlane.GetDistanceToPoint(Obj3D.transform.position));
            return displacement;
        }

        public static float GetLocalDisplacement_Obj3D_Obj2D(GameObject Obj3D, GameObject Obj2D)
        {
            Vector3 Obj3D_localPos_ToObj2D = Obj2D.transform.parent.transform.InverseTransformPoint(Obj3D.transform.position);

            Plane obj2DLocatedPlane = new Plane(Vector3.forward, Obj2D.transform.localPosition);
            float displacement = Mathf.Abs(obj2DLocatedPlane.GetDistanceToPoint(Obj3D_localPos_ToObj2D));
            return displacement;
        }

        public static float GetLocalDistance_ByFOV(GameObject view_origin, GameObject point_on_plane, float fov_in_degree)
        {
            if (fov_in_degree >= 90.0f) return float.MaxValue;
            float localDisplacement = GetLocalDisplacement_Obj3D_Obj2D(view_origin, point_on_plane);
            return localDisplacement * Mathf.Tan(Mathf.Deg2Rad * fov_in_degree);
        }
    }
}