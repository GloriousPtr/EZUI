using UnityEditor;
using UnityEngine;

namespace EZUI_Editor
{
	public class EditorUtils
	{
		public static void DrawField(SerializedProperty serializedProperty, int labelWidth, string propertyName, string customName = null)
		{
			SerializedProperty prop = serializedProperty.FindPropertyRelative(propertyName);
			if (string.IsNullOrEmpty(customName))
				customName = prop.displayName;
			
			if(labelWidth != 0)
				EditorGUILayout.LabelField(customName, GUILayout.Width(labelWidth));
			
			EditorGUILayout.PropertyField(prop, labelWidth == 0 ? new GUIContent(customName) : GUIContent.none);
		}
		
		public static void DrawField(SerializedObject serializedObject, int labelWidth, string propertyName, string customName = null)
		{
			SerializedProperty prop = serializedObject.FindProperty(propertyName);
			if (string.IsNullOrEmpty(customName))
				customName = prop.displayName;
			
			if(labelWidth != 0)
				EditorGUILayout.LabelField(customName, GUILayout.Width(labelWidth));
			
			EditorGUILayout.PropertyField(prop, labelWidth == 0 ? new GUIContent(customName) : GUIContent.none);
		}

		public static void BeginDrawFieldsHorizontal(SerializedProperty serializedProperty, bool autoEnd, int labelWidth, params string[] propNames)
		{
			EditorGUILayout.BeginHorizontal();
			for (int i = 0; i < propNames.Length; i++)
			{
				SerializedProperty prop = serializedProperty.FindPropertyRelative(propNames[i]);
				
				if(labelWidth != 0)
					EditorGUILayout.LabelField(prop.displayName, GUILayout.Width(labelWidth));
				
				EditorGUILayout.PropertyField(prop, labelWidth == 0 ? new GUIContent(prop.displayName) : GUIContent.none);
				
				if (i < propNames.Length - 1)
					EditorGUILayout.Space(10.0f);
			}

			if (autoEnd)
				EndDrawFieldsHorizontal();
		}
		
		public static void BeginDrawFieldsHorizontal(SerializedObject serializedObject, bool autoEnd, int labelWidth, params string[] propNames)
		{
			EditorGUILayout.BeginHorizontal();
			for (int i = 0; i < propNames.Length; i++)
			{
				SerializedProperty prop = serializedObject.FindProperty(propNames[i]);
				
				if(labelWidth != 0)
					EditorGUILayout.LabelField(prop.displayName, GUILayout.Width(labelWidth));
				
				EditorGUILayout.PropertyField(prop, labelWidth == 0 ? new GUIContent(prop.displayName) : GUIContent.none);
				
				if (i < propNames.Length - 1)
					EditorGUILayout.Space(10.0f);
			}

			if (autoEnd)
				EndDrawFieldsHorizontal();
		}

		public static void EndDrawFieldsHorizontal()
		{
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space(3);
		}

		public static void BeginDrawFieldsVertical(SerializedProperty serializedProperty, bool autoEnd, int labelWidth, params string[] propNames)
		{
			EditorGUILayout.BeginVertical();
			for (int i = 0; i < propNames.Length; i++)
			{
				SerializedProperty prop = serializedProperty.FindPropertyRelative(propNames[i]);
				
				if(labelWidth != 0)
					EditorGUILayout.LabelField(prop.displayName, GUILayout.Width(labelWidth));
				
				EditorGUILayout.PropertyField(prop, labelWidth == 0 ? new GUIContent(prop.displayName) : GUIContent.none);
			}
			
			if (autoEnd)
				EditorGUILayout.EndVertical();
		}
		
		public static void BeginDrawFieldsVertical(SerializedObject serializedObject, bool autoEnd, int labelWidth, params string[] propNames)
		{
			EditorGUILayout.BeginVertical();
			for (int i = 0; i < propNames.Length; i++)
			{
				SerializedProperty prop = serializedObject.FindProperty(propNames[i]);
				
				if(labelWidth != 0)
					EditorGUILayout.LabelField(prop.displayName, GUILayout.Width(labelWidth));
				
				EditorGUILayout.PropertyField(prop, labelWidth == 0 ? new GUIContent(prop.displayName) : GUIContent.none);
			}
			
			if (autoEnd)
				EditorGUILayout.EndVertical();
		}

		public static void EndDrawFieldsVertical() => EditorGUILayout.EndVertical();
		
		public static void InspectorWidth(float currentInspectorWidth, out float width)
		{
			width = currentInspectorWidth;
			GUILayout.Label(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(0));
			if (Event.current.type != EventType.Repaint) return;
			Rect lastRect = GUILayoutUtility.GetLastRect();
			if (lastRect.width > 1) width = lastRect.width;
		}
	}
}