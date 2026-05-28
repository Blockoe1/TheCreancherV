/*****************************************************************************
// File Name : HealthStructDrawer.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : 
*****************************************************************************/
using UnityEditor;
using UnityEngine;

namespace FoolsBrand.Editor
{
    [CustomPropertyDrawer(typeof(HealthData))]
    public class HealthStructDrawer : PropertyDrawer
    {
        private SerializedProperty maxHealth;
        private SerializedProperty health;

        

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            maxHealth = property.FindPropertyRelative(nameof(maxHealth));
            health = property.FindPropertyRelative(nameof(health));

            float maxHealthHeight = EditorGUI.GetPropertyHeight(maxHealth);
            Rect maxHealthRect = new Rect(position.x, position.y, position.width, maxHealthHeight);
            //base.OnGUI(position, property, label);
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(maxHealthRect, maxHealth);
            if (EditorGUI.EndChangeCheck())
            {
                health.intValue = maxHealth.intValue;
            }

            float yOffset = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            float healthHeight = EditorGUI.GetPropertyHeight(health);
            Rect healthRect = new Rect(position.x, yOffset, position.width, healthHeight);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(healthRect, health);
            EditorGUI.EndDisabledGroup();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 2 * base.GetPropertyHeight(property, label) + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
