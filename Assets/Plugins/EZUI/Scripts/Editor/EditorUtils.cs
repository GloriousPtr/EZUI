using System.Reflection;
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
		
		public static object GetTargetObjectOfProperty(SerializedProperty prop)
		{
			if (prop == null) return null;

			var path = prop.propertyPath.Replace(".Array.data[", "[");
			object obj = prop.serializedObject.targetObject;
			var elements = path.Split('.');
			foreach (var element in elements)
			{
				if (element.Contains("["))
				{
					var elementName = element.Substring(0, element.IndexOf("["));
					var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
					obj = GetValue_Imp(obj, elementName, index);
				}
				else
				{
					obj = GetValue_Imp(obj, element);
				}
			}
			return obj;
		}
		
		private static object GetValue_Imp(object source, string name)
		{
			if (source == null)
				return null;
			var type = source.GetType();

			while (type != null)
			{
				var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (f != null)
					return f.GetValue(source);

				var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (p != null)
					return p.GetValue(source, null);

				type = type.BaseType;
			}
			return null;
		}

		private static object GetValue_Imp(object source, string name, int index)
		{
			var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
			if (enumerable == null) return null;
			var enm = enumerable.GetEnumerator();
			//while (index-- >= 0)
			//    enm.MoveNext();
			//return enm.Current;

			for (int i = 0; i <= index; i++)
			{
				if (!enm.MoveNext()) return null;
			}
			return enm.Current;
		}
	}
}