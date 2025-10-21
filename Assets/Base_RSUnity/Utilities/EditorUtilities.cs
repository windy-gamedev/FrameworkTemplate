#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;

namespace Wigi.Utilities
{
    public static class EditorUtils
    {
        public static UnityEngine.Object GetSelectObjectInspector()
        {
            return Selection.activeObject;
        }

        public static void SelectObjectInspector(UnityEngine.Object obj)
        {
            Selection.activeObject = obj;
        }
        public static void OpenNewInspector(UnityEngine.Object obj)
        {
            GUIHelper.OpenInspectorWindow(obj);
        }
        /// <summary>
        /// Show new Inspector Window of Scripable Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="directoryName">path in Assets folder</param>
        /// <param name="assetName">name of asset</param>
        public static void OpenInstanceScripable<T>(string directoryName, string assetName) where T : ScriptableObject
        {
            string directoryPath = Path.Combine(Application.dataPath, directoryName);
            string assetPath = Path.Combine("Assets", directoryName, assetName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var settings = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(settings, assetPath);
            }
            GUIHelper.OpenInspectorWindow(settings);
        }

        public static ValueDropdownList<int> GetClassForDropdown<T>(string nameAdd = "[Add]", params Type[] filters)
        {
            var classList = GetClassList<T>(filters);
            var list = new ValueDropdownList<int>();
            list.Add(nameAdd, 0);
            int i = 1;
            foreach (var type in classList)
            {
                list.Add(type.Name, i);
                i++;
            }
            return list;
        }
        public static System.Type GetClassSelectDropdown<T>(int select, params Type[] filters)
        {
            var classList = GetClassList<T>(filters);
            if (select < 1 && select > classList.Count())
                return null;
            return classList.ElementAt(select - 1);
        }
        public static IEnumerable<System.Type> GetClassList<T>(params Type[] filters)
        {
            var q = typeof(T).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(T).IsAssignableFrom(x))
                .Where(x => !filters.Contains(x));
            return q;
        }

        public static void PropetyField(object value, string label = "")
        {
            if (value is float)
                EditorGUILayout.FloatField(label, (float)value);
            else if (value is int)
                EditorGUILayout.IntField(label, (int)value);
            else if (value is bool)
                EditorGUILayout.Toggle(label, (bool)value);
            else if (value is string)
                EditorGUILayout.TextField(label, (string)value);
            else if (value is double)
                EditorGUILayout.DoubleField(label, (double)value);
            else if (value is long)
                EditorGUILayout.LongField(label, (long)value);
            else if (value is Enum)
                EditorGUILayout.EnumFlagsField(label, (Enum)value);
            else if (value is Vector3)
                EditorGUILayout.Vector3Field(label, (Vector3)value);
            else if (value is Color)
                EditorGUILayout.ColorField(label, (Color)value);
        }
    }

    public static class AssetUtils
    {
        public static string GetObjectToGUID(UnityEngine.Object obj)
        {
            return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
        }
    }

    public static class ToolEditorUtils
    {
        public static Rect ResizeRect(Rect rect, Handles.CapFunction capFunc, Color capCol, Color fillCol, float capSize, float snap)
        {


            Vector3[] rectangleCorners = rect.RectToCorners();

            Handles.color = fillCol;
            Handles.DrawSolidRectangleWithOutline(rectangleCorners, new Color(fillCol.r, fillCol.g, fillCol.b, 0.05f), capCol);
            
            Vector2 halfRectSize = new Vector2(rect.size.x * 0.5f, rect.size.y * 0.5f);
            Vector3[] handlePoints =
                {
                new Vector3(rect.position.x, rect.position.y + halfRectSize.y, 0),   // Left
                new Vector3(rect.position.x + rect.size.x, rect.position.y + halfRectSize.y, 0),   // Right
                new Vector3(rect.position.x + halfRectSize.x, rect.position.y, 0)   , // Bottom 
                new Vector3(rect.position.x + halfRectSize.x, rect.position.y + rect.size.y, 0) // Top
            };
            Vector3 centerPoint = rect.position + halfRectSize;

            Handles.color = capCol;

            var newSize = rect.size;
            var newPosition = rect.position;

            var leftSlider = Handles.Slider(handlePoints[0], -Vector3.right, capSize, capFunc, snap);
            var rightSlider = Handles.Slider(handlePoints[1], Vector3.right, capSize, capFunc, snap);
            var bottomSlider = Handles.Slider(handlePoints[2], -Vector3.up, capSize, capFunc, snap);
            var topSlider = Handles.Slider(handlePoints[3], Vector3.up, capSize, capFunc, snap);
            var fmh_143_66_638965087765465675 = Quaternion.identity; var centerMove = Handles.FreeMoveHandle(centerPoint, capSize, new Vector3(snap, snap, snap), capFunc);

            var leftHandle = leftSlider.x - handlePoints[0].x;
            var rightHandle = rightSlider.x - handlePoints[1].x;
            var bottomHandle = bottomSlider.y - handlePoints[2].y;
            var topHandle = topSlider.y - handlePoints[3].y;
            var centerHandle = centerMove - centerPoint;

            //ReDraw Point
            Handles.DrawWireCube(leftSlider, new Vector3(capSize, capSize, capSize));
            Handles.DrawWireCube(rightSlider, new Vector3(capSize, capSize, capSize));
            Handles.DrawWireCube(topSlider, new Vector3(capSize, capSize, capSize));
            Handles.DrawWireCube(bottomSlider, new Vector3(capSize, capSize, capSize));
            Handles.DrawWireCube(centerPoint, new Vector3(capSize, capSize, capSize));

            newSize = new Vector2(
                Mathf.Max(.1f, newSize.x + rightHandle - leftHandle),
                Mathf.Max(.1f, newSize.y + topHandle - bottomHandle));

            newPosition = new Vector2(
                newPosition.x + leftHandle + centerHandle.x,
                newPosition.y + bottomHandle + centerHandle.y);

            return new Rect(newPosition.x, newPosition.y, newSize.x, newSize.y);
        }
    }
}
#endif
