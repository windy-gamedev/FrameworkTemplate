﻿using UnityEditor;
using UnityEngine;

namespace Wigi.Actions
{
    [CustomEditor(typeof(DOTweenPunchRotation))]
    public class DOTweenPunchRotationInspector : DOTweenTransitionInspector
    {

        private DOTweenPunchRotation dOTweenPunchRotation;
        private SerializedProperty targetProperty;
        private SerializedProperty fromCurrentProperty;
        private SerializedProperty fromProperty;
        private SerializedProperty punchProperty;
        private SerializedProperty vibratoProperty;
        private SerializedProperty elasticityProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            dOTweenPunchRotation = transition as DOTweenPunchRotation;
            targetProperty = serializedObject.FindProperty("target");
            fromCurrentProperty = serializedObject.FindProperty("fromCurrent");
            fromProperty = serializedObject.FindProperty("from");
            punchProperty = serializedObject.FindProperty("punch");
            vibratoProperty = serializedObject.FindProperty("vibrato");
            elasticityProperty = serializedObject.FindProperty("elasticity");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(targetProperty);

            EditorGUILayout.PropertyField(fromCurrentProperty);
            if (!dOTweenPunchRotation.FromCurrent)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(fromProperty);
                if (GUILayout.Button("Set From", GUILayout.Width(100)))
                {
                    dOTweenPunchRotation.SetFromState();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(punchProperty);
            if (GUILayout.Button("Set To", GUILayout.Width(100)))
            {
                dOTweenPunchRotation.SetToState();
            }
            GUILayout.EndHorizontal();


            EditorGUILayout.PropertyField(vibratoProperty);
            EditorGUILayout.PropertyField(elasticityProperty);

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

    }
}
