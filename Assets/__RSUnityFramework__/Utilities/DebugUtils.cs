using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wigi.Utilities
{
    public static class DebugUtils
    {
        public static void Log(string message, params object[] args)
        {
            foreach (var arg in args)
                message += ", " + arg.ToString();
            Debug.Log(message);
        }
    }

    public static class GizmosUtil
    {
        public static void DrawArrow(Vector3 from, Vector3 direction, float lengthArrow = 0.2f)
        {
            Gizmos.DrawRay(from, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(150, 0, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(210, 0, 0) * new Vector3(0, 0, 1);

            Gizmos.DrawRay(from + direction, right * lengthArrow);
            Gizmos.DrawRay(from + direction, left * lengthArrow);
        }
    }
}
