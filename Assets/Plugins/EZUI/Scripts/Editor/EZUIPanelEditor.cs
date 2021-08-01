using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using EZUI;

namespace EZUI_Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(EZUIPanel))]
	public class EZUIPanelEditor : EZUIEditorBase
	{
		private static Color _defaultColor;

		private static AnimBoolGroup _showAnimBoolsGroup;
		private static AnimBoolGroup _hideAnimBoolsGroup;

		private SerializedProperty key;
		private SerializedProperty useCustomStartPosition;
		private SerializedProperty customStartPosition;
		private SerializedProperty showData;
		private SerializedProperty hideData;

		protected override void OnEnable()
		{
			base.OnEnable();
			
			_showAnimBoolsGroup = new AnimBoolGroup(Repaint);
			_hideAnimBoolsGroup = new AnimBoolGroup(Repaint);

			Init();
		}

		private void Init()
		{
			key = serializedObject.FindProperty("key");
			useCustomStartPosition = serializedObject.FindProperty("useCustomStartPosition");
			customStartPosition = serializedObject.FindProperty("customStartPosition");
			showData = serializedObject.FindProperty("showData");
			hideData = serializedObject.FindProperty("hideData");
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();
			
			EZUIPanel panel = (EZUIPanel) target;
			bool previewEnabled = targets.Length == 1;
			_defaultColor = GUI.backgroundColor;
			
			EditorGUILayout.BeginVertical("Box", GUILayout.Height(50));
			EditorGUILayout.LabelField(HeadingTemplate("EZUI Panel", 35), HeadingStyle);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space(20);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Key", GUILayout.Width(135));
			EditorGUILayout.PropertyField(key, GUIContent.none);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space(20);
			
			EditorGUILayout.BeginHorizontal();
			useCustomStartPosition.boolValue = EditorGUILayout.Toggle(useCustomStartPosition.boolValue, GUILayout.Width(20));
			EditorGUILayout.LabelField("Custom Start Position", GUILayout.Width(135));
			EditorGUILayout.PropertyField(customStartPosition, GUIContent.none);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Get Position", GUILayout.Height(25)))
				customStartPosition.vector3Value = panel.transform.localPosition;
			if (GUILayout.Button("Set Position", GUILayout.Height(25)))
				panel.transform.localPosition = customStartPosition.vector3Value;
			if (GUILayout.Button("Reset Position", GUILayout.Height(25)))
				customStartPosition.vector3Value = StaticData.ZeroVector;
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space(20);
			
			DrawAnimationData("Show Panel", previewEnabled, panel, panel.showData, showData, _showAnimBoolsGroup);
			DrawAnimationData("Hide Panel", previewEnabled, panel, panel.hideData, hideData, _hideAnimBoolsGroup);
			
			GUI.backgroundColor = _defaultColor;
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}