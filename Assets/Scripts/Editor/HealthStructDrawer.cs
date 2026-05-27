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

        

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);
            maxHealth = property.FindPropertyRelative(nameof(maxHealth));

            EditorGUI.PropertyField(position, maxHealth);
        }
    }
}
