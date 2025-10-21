using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Wigi.Utilities
{
    public static class MathEx
    {
        public const float MOVE_INCH  = 7f / 160;
        public static float ConvertDistanceToInch(Vector2 delta)
        {
            return  delta.magnitude / Screen.dpi;
        }
        #region Number
        /// <summary>
        /// Get delta compare Max abs
        /// </summary>
        /// <param name="a"></param>
        /// <param name="max"> max > 0 </param>
        /// <returns></returns>
        public static float NumberDeltaMax(float a, float max)
        {
            if (max < 0) max = Mathf.Abs(max);
            if(a < 0)
            {
                if (a < -max) return -max;
            }
            else
            {
                if(a > max) return max;
            }
            return a;
        }

        /// <summary>
        /// Change delta by target value.
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="deltaTarget">delta value to target value</param>
        /// <param name="delta">do not depend on the sign of delta</param>
        /// <returns>return true if change == target </returns>
        public static bool ChangeDeltaTarget(ref float raw, float deltaTarget, float delta)
        {
            //check sign of values
            if (deltaTarget < 0) { if (delta > 0) delta = -delta; }
            else { if (delta < 0) delta = -delta; }

            float change = NumberDeltaMax(delta, deltaTarget);
            raw += change;
            return change == deltaTarget;
        }
        #endregion

        #region Rotation Angle

        public static float AngleDelta(float a, float b)
        {
            float delta = AngleAbs(a) - AngleAbs(b);
            if (delta > 180) delta = delta - 360;
            else if(delta < -180) delta = 360 + delta;

            return delta;
        }

        public static float AngleAbs(float a)
        {
            a = a % 360;
            if (a < 0)
                a = a + 360;
            return a;
        }
        #endregion

        #region Vector
        public static Vector3 Rotate(this Vector3 vector, float angle)
        {
            return Quaternion.AngleAxis(angle, Vector3.up) * vector;
        }

        public static Vector3 TransformScale(this Vector3 vector, Vector3 scale)
        {
            return new Vector3(vector.x * scale.x, vector.y * scale.y, vector.z * scale.z);
        }
        public static Vector2 TransformScale(this Vector2 vector, Vector2 scale)
        {
            return new Vector2(vector.x * scale.x, vector.y * scale.y);
        }
        public static Vector3 MultiplyScale(this Vector3 vector, Vector3 scale)
        {
            return new Vector3(vector.x * scale.x, vector.y * scale.y, vector.z * scale.z);
        }
        public static Vector2 MultiplyScale(this Vector2 vector, Vector2 scale)
        {
            return new Vector2(vector.x * scale.x, vector.y * scale.y);
        }

        public static Vector3 Abs(this Vector3 vector)
        {
            return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }
        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2 (Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }

        public static Vector2 MinAbsVector(Vector2 vector, Vector2 min)
        {
            var absV = Abs(vector);
            var minAbs = Abs(min);
            if (absV.x < minAbs.x) vector.x *= minAbs.x / absV.x;
            if (absV.y < minAbs.y) vector.y *= minAbs.y / absV.y;
            return vector;
        }
        public static Vector2 MaxAbsVector(Vector2 vector, Vector2 max)
        {
            var absV = Abs(vector);
            var maxAbs = Abs(max);
            if (absV.x > maxAbs.x) vector.x *= maxAbs.x / absV.x;
            if (absV.y > maxAbs.y) vector.y *= maxAbs.y / absV.y;
            return vector;
        }

        public static float AngleVector(Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x);
        }
        #endregion

        #region Size
        public static Size Size(this Vector2 vSize)
        {
            return new Size(vSize.x, vSize.y);
        }
        public static float width(this Vector2 vSize)
        {
            return vSize.x;
        }
        public static float height(this Vector2 vSize)
        {
            return vSize.y;
        }
        public static Vector2 Vector(this Size size)
        {
            return new Vector2(size.width, size.height);
        }
        #endregion

        #region Rect
        public static Vector3[] RectToCorners(this Rect rect)
        {
            Vector3[] rectangleCorners =
                {
                new Vector3(rect.position.x, rect.position.y, 0),   // Bottom Left
                new Vector3(rect.position.x + rect.size.x, rect.position.y, 0),   // Bottom Right
                new Vector3(rect.position.x + rect.size.x, rect.position.y + rect.size.y, 0),   // Top Right
                new Vector3(rect.position.x, rect.position.y + rect.size.y, 0)    // Top Left
            };
            return rectangleCorners;
        }

        public static Vector2 RandomPoint(this Rect rect)
        {
            return new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
        }

        public static Rect WorldToLocalRect(this Rect worldRect, Transform transform)
        {
            var rectTransform = transform.GetComponent<RectTransform>();
            if (rectTransform == null)
                throw new System.ArgumentNullException();
            Rect localRect = new Rect();
            localRect.position = transform.InverseTransformPoint(worldRect.position);
            Vector2 top = transform.InverseTransformPoint(new Vector3(worldRect.xMax, worldRect.yMax));
            localRect.size = top - localRect.position;
            return localRect;
        }
        public static Rect LocalToWorldRect(this Rect localRect, Transform transform)
        {
            Rect worldRect = new Rect();
            worldRect.position = transform.TransformPoint(localRect.position);
            Vector2 top = transform.TransformPoint(new Vector2(localRect.xMax, localRect.yMax));
            worldRect.size = top - worldRect.position;
            return worldRect;
        }
        #endregion
    }

    //Array Util
    public static class ArrayUtil
    {
        public delegate bool ConditionArray<T>(T a);

        public static void RemoveCondition<T>(this LinkedList<T> list, ConditionArray<T> condition)
        {
            var first = list.First;
            var node = first;
            if (list.Count == 1)
            {
                if (condition(node.Value))
                    list.Clear();
            }
            else
            {
                while (node != null && node.Next != first)
                {
                    if (condition(node.Value))
                    {
                        list.Remove(node);
                    }
                    node = node.Next;
                }
            }
        }
    }

    public struct Size
    {
        public float width;
        public float height;

        public Size(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public Size(Vector2 vector)
        {
            width = vector.x;
            height = vector.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size operator +(Size a, Size b)
        {
            return new Size(a.width + b.width, a.height + b.height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size operator -(Size a, Size b)
        {
            return new Size(a.width - b.width, a.height - b.height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size operator *(Size a, Size b)
        {
            return new Size(a.width * b.width, a.height * b.height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size operator /(Size a, Size b)
        {
            return new Size(a.width / b.width, a.height / b.height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size operator -(Size a)
        {
            return new Size(0f - a.width, 0f - a.height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size operator *(Size a, float d)
        {
            return new Size(a.width * d, a.height * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size operator *(float d, Size a)
        {
            return new Size(a.width * d, a.height * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size operator /(Size a, float d)
        {
            return new Size(a.width / d, a.height / d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Size lhs, Size rhs)
        {
            float num = lhs.width - rhs.width;
            float num2 = lhs.height - rhs.height;
            return num * num + num2 * num2 < 9.99999944E-11f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Size lhs, Size rhs)
        {
            return !(lhs == rhs);
        }
    }
}
